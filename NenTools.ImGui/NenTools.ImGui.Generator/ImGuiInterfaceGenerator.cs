using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NenTools.ImGui.Generator;

public class ImGuiInterfaceGenerator
{
    private string _implNamespace;
    private string _methodsNamespace;
    private string _interfaceNamespace;

    private readonly List<MemberDeclarationSyntax> _interfaceMethodList = [];
    private readonly List<MemberDeclarationSyntax> _interfaceMembers = [];

    private readonly List<MemberDeclarationSyntax> _implMemberList = [];

    private readonly List<MemberDeclarationSyntax> _bindingsMethodList = [];
    private readonly List<MemberDeclarationSyntax> _bindingsMemberList = [];

    private static readonly HashSet<string> _typesToWrap = [];

    private DearBindingsMetadata _metadata;

    /// <summary>
    /// Functions that we declare manually and are therefore skipped interface and impl wise (mainly because they're a chore to implement and interface through code).
    /// </summary>
    private static readonly FrozenSet<string> ManuallyImplementedFunctions =
    [
        // Functions with out parameters that are vectors
        "ImGuiTextFilter_ImGuiTextRange_split",
        "ImFontGlyphRangesBuilder_BuildRanges",

        "ImTextureRef_GetTexID", // Kind of annoying value type.
    ];


    public ImGuiInterfaceGenerator(string implNamespace, string methodsNamespace, string interfaceNamespace)
    {
        _implNamespace = implNamespace;
        _methodsNamespace = methodsNamespace;
        _interfaceNamespace = interfaceNamespace;
    }

    public void SetMetadata(DearBindingsMetadata metadata)
    {
        _metadata = metadata;
    }

    #region Interface Generation
    public void ExtractInterfaceFromSyntaxTree(SyntaxTree syntaxTree)
    {
        var bindingsRoot = syntaxTree.GetCompilationUnitRoot();

        foreach (var mem in bindingsRoot.Members)
        {
            if (mem is NamespaceDeclarationSyntax namespace_)
            {
                // add all function bindings first
                var methodsClass = namespace_.Members.Where(e => e.Kind() == SyntaxKind.ClassDeclaration && ((ClassDeclarationSyntax)e).Identifier.Text == "Methods").FirstOrDefault() as ClassDeclarationSyntax 
                    ?? throw new KeyNotFoundException("Could not find \"Methods\" class from bindings.");

                foreach (var methodClassMember in methodsClass.Members)
                {
                    if (!methodClassMember.IsKind(SyntaxKind.MethodDeclaration))
                        continue;

                    var method = (MethodDeclarationSyntax)methodClassMember;
                    if (ManuallyImplementedFunctions.Contains(method.Identifier.ValueText))
                        continue;

                    // Strip attributes.
                    var newParamList = new List<ParameterSyntax>();
                    foreach (var param in method.ParameterList.Parameters)
                    {
                        // Skip arglist (it's fine to skip, according to CppSharp. Not like we need it anyway.)
                        if (param.Identifier.IsKind(SyntaxKind.ArgListKeyword))
                            continue;

                        TypeSyntax newType = FixupParameterType(param.Type, param.AttributeLists, ImGuiStructTypeKind.ForInterface, modifiers: out SyntaxTokenList modifiers);
                        newType = FixParameterTypeForFunctionCall(newType, ref modifiers);

                        ParameterSyntax newParam = param.WithType(newType)
                            .WithModifiers(modifiers)
                            .WithAttributeLists([]);

                        // For each pointer type, we're gonna need to wrap them.
                        if (newType is PointerTypeSyntax pointerType && pointerType.ElementType is IdentifierNameSyntax identifierName)
                            _typesToWrap.Add(identifierName.Identifier.ValueText);

                        newParamList.Add(newParam);
                    }

                    var paramListSyntaxList = new SeparatedSyntaxList<ParameterSyntax>();
                    paramListSyntaxList = paramListSyntaxList.AddRange(newParamList);

                    string? nativeTypeName = GetNativeTypeName(method.AttributeLists);
                    TypeSyntax returnType = method.ReturnType;
                    if (nativeTypeName is not null)
                        returnType = FixupParameterType(method.ReturnType, method.AttributeLists, ImGuiStructTypeKind.ForInterface, modifiers: out SyntaxTokenList modifiers);

                    MethodDeclarationSyntax newMethod = SF.MethodDeclaration(
                            attributeLists: [],
                            modifiers: [],
                            returnType,
                            explicitInterfaceSpecifier: null!,
                            method.Identifier,
                            method.TypeParameterList!,
                            SF.ParameterList(paramListSyntaxList),
                            method.ConstraintClauses,
                            body: null!,
                            method.SemicolonToken);

                    string? methodName = GetEntryPoint(method);
                    if (methodName is not null)
                    {
                        FunctionMetadata funcMetadata = _metadata.FunctionsByName![methodName];
                        newMethod = DecorateWithComments(newMethod, funcMetadata.Comments?.Attached, funcMetadata.Comments?.Preceding, numTabs: 1);
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Method name {methodName} not found in dear bindings metadata");
                    }

                    _interfaceMethodList.Add(newMethod);
                }

                // Add everything else next
                foreach (var member in namespace_.Members)
                {
                    if (member.IsKind(SyntaxKind.ClassDeclaration) && ((ClassDeclarationSyntax)member).Identifier.Text == "Methods")
                        continue;

                    // Trim enum names.
                    if (member.IsKind(SyntaxKind.EnumDeclaration))
                    {
                        EnumDeclarationSyntax enumDeclaration = (EnumDeclarationSyntax)member;
                        EnumMetadata enumMetadata = _metadata.EnumsByName![enumDeclaration.Identifier.ValueText];

                        List<EnumMemberDeclarationSyntax> newEnumMembers = [];
                        foreach (EnumMemberDeclarationSyntax enumMember in enumDeclaration.Members)
                        {
                            EnumValueMetadata? enumValueMetadata = enumMetadata.Elements.FirstOrDefault(e => e.Name == enumMember.Identifier.ValueText) ?? 
                                throw new KeyNotFoundException($"Could not find enum value {enumMember.Identifier.ValueText} for enum {enumDeclaration.Identifier.ValueText} in dear bindings metadata");

                            EnumMemberDeclarationSyntax newEnumMember = DecorateWithComments(enumMember, enumValueMetadata.Comments?.Attached, enumValueMetadata.Comments?.Preceding, numTabs: 1);
                            newEnumMembers.Add(newEnumMember);
                        }

                        enumDeclaration = enumDeclaration
                            .WithIdentifier(SF.Identifier(enumDeclaration.Identifier.ValueText.TrimEnd('_')))
                            .WithMembers(SF.SeparatedList(newEnumMembers));
                        enumDeclaration = DecorateWithComments(enumDeclaration, enumMetadata.Comments?.Attached, enumMetadata.Comments?.Preceding, numTabs: 0).NormalizeWhitespace();
                        _interfaceMembers.Add(enumDeclaration);
                    }
                    else if (member.IsKind(SyntaxKind.StructDeclaration))
                    {
                        StructDeclarationSyntax structDeclaration = (StructDeclarationSyntax)member;

                        string structName = structDeclaration.Identifier.ValueText;
                        if (structName.EndsWith("_t"))
                            structName = structName.Substring(0, structName.Length - 2);

                        // Pointless to redeclare these
                        if (TypeInfo.SkipDeclareIdentifiers.Contains(structName) || structName.StartsWith("ImVector"))
                            continue;

                        List<MemberDeclarationSyntax> newStructMembers = [];

                        foreach (MemberDeclarationSyntax structMember in structDeclaration.Members)
                        {
                            if (structMember.IsKind(SyntaxKind.FieldDeclaration))
                            {
                                FieldDeclarationSyntax fieldDeclaration = (FieldDeclarationSyntax)structMember;
                                VariableDeclarationSyntax variableDeclaration = fieldDeclaration.Declaration;

                                if (variableDeclaration.Type.ToString().StartsWith("_Anonymous"))
                                {
                                    // We don't care for defining anonymous unions.
                                    // We will declare the members of that directly instead.
                                    continue;
                                }

                                var fieldName = variableDeclaration.Variables[0].Identifier.ValueText;
                                StructFieldMetadata? fieldMetadata = _metadata.StructsByName![structName].Fields.FirstOrDefault(e => e.Name == fieldName);

                                TypeSyntax newType = FixupParameterType(variableDeclaration.Type, fieldDeclaration.AttributeLists, ImGuiStructTypeKind.ForInterface, modifiers: out SyntaxTokenList modifiers, 
                                    doNotRemapStrings: true, isArrayElement: fieldMetadata?.IsArray == true);

                                bool withSetter = true;
                                if (IsRefableProperty(newType))
                                {
                                    newType = SF.RefType(newType);
                                    withSetter = false;
                                }
                                else if (newType.IsKind(SyntaxKind.IdentifierName) || newType.IsKind(SyntaxKind.GenericName))
                                {
                                    // for safety, we disallow setters on imgui structures
                                    withSetter = false;
                                }

                                PropertyDeclarationSyntax newProperty = SF.PropertyDeclaration(newType, variableDeclaration.Variables[0].Identifier)
                                     .WithAccessorList(SyntaxTreeUtils.GetAccessorList(withSetter: withSetter));

                                newProperty = DecorateWithComments(newProperty, fieldMetadata?.Comments?.Attached, fieldMetadata?.Comments?.Preceding, numTabs: 1);
                                newStructMembers.Add(newProperty);
                            }
                            else if (structMember.IsKind(SyntaxKind.PropertyDeclaration))
                            {
                                PropertyDeclarationSyntax propertyDeclaration = (PropertyDeclarationSyntax)structMember;
                                TypeSyntax newType = FixupParameterType(propertyDeclaration.Type, propertyDeclaration.AttributeLists, ImGuiStructTypeKind.ForInterface, out _);

                                var fieldName = propertyDeclaration.Identifier.ValueText;
                                StructFieldMetadata? fieldMetadata = _metadata.StructsByName![structName].Fields.FirstOrDefault(e => e.Name == fieldName);

                                PropertyDeclarationSyntax newProperty = propertyDeclaration.WithAttributeLists([])
                                    .WithAccessorList(SyntaxTreeUtils.GetAccessorList(withSetter: !propertyDeclaration.Type.IsKind(SyntaxKind.RefType)))
                                    .WithModifiers(default)
                                    .WithType(newType);

                                newProperty = DecorateWithComments(newProperty, fieldMetadata?.Comments?.Attached, fieldMetadata?.Comments?.Preceding, numTabs: 1);
                                newStructMembers.Add(newProperty);
                            }
                            else if (structMember.IsKind(SyntaxKind.StructDeclaration))
                            {
                                // Nested structs are emitted by ClangSharpPInvokeGenerator when a certain field cannot be wrapped easily
                                // We wrap them ourselves, so we can skip them
                                continue; 
                            }
                            else
                                newStructMembers.Add(structMember);
                        }

                        StructMetadata? structMetadata = _metadata.StructsByName![structName];
                        InterfaceDeclarationSyntax newInterface = SF.InterfaceDeclaration($"I{structName}")
                            .WithMembers(SF.List<MemberDeclarationSyntax>(newStructMembers))
                            .WithModifiers(SF.TokenList([
                                SF.Token(SyntaxKind.PublicKeyword),
                                SF.Token(SyntaxKind.UnsafeKeyword),
                                SF.Token(SyntaxKind.PartialKeyword),
                            ]))
                            .WithBaseList(SF.BaseList([SF.SimpleBaseType(SF.ParseTypeName("INativeStruct"))]));

                        newInterface = DecorateWithComments(newInterface, structMetadata.Comments?.Attached, structMetadata.Comments?.Preceding);
                        _interfaceMembers.Add(newInterface);
                    }
                    else
                    {
                        _interfaceMembers.Add(member);
                    }
                }
            }

        }
    }

    private static T DecorateWithComments<T>(T member, string? attached, List<string>? preceding, int numTabs = 0) where T : MemberDeclarationSyntax
    {
        List<string> lines = [];
        if (attached is not null)
        {
            lines.Add(attached);
        }

        if (preceding?.Count > 0)
        {
            if (attached is not null)
                lines.Add(string.Empty);

            foreach (var comment in preceding)
                lines.Add(comment);
        }

        if (lines.Count > 0)
            member = member.AddSummary(lines, numTabs: numTabs);
        return member;
    }

    public string FinishInterface()
    {
        InterfaceDeclarationSyntax interfaceClass = SF.InterfaceDeclaration("IImGui")
            .WithModifiers(SF.TokenList([
                SF.Token(SyntaxKind.PublicKeyword),
                SF.Token(SyntaxKind.UnsafeKeyword),
                SF.Token(SyntaxKind.PartialKeyword),
            ]))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(_interfaceMethodList))
            .AddSummary(["Interface to the ImGui library."]);

        var allMembers = new List<MemberDeclarationSyntax>();
        allMembers.Add(interfaceClass);
        allMembers.AddRange(_interfaceMembers);

        FileScopedNamespaceDeclarationSyntax namespaceDecl = SF.FileScopedNamespaceDeclaration(SF.ParseName(_interfaceNamespace))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(allMembers));

        CompilationUnitSyntax root = SF.CompilationUnit()
            .WithUsings(SF.List(
            [
                SF.UsingDirective(SF.ParseName("System")),
                SF.UsingDirective(SF.ParseName("System.Runtime.InteropServices")),
                SF.UsingDirective(SF.ParseName("System.Numerics"))
            ]))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(namespaceDecl))
            .WithLeadingTrivia(SF.Comment("// This file was generated with ImGuiInterfaceGenerator"))
            .NormalizeWhitespace();

        SyntaxTree interfaceTree = CSharpSyntaxTree.Create(root);
        return interfaceTree.ToString();
    }
    #endregion

    #region Impl Generation
    public void ExtractImplFromSyntaxTree(SyntaxTree syntaxTree)
    {
        var bindingsRoot = syntaxTree.GetCompilationUnitRoot();

        foreach (MemberDeclarationSyntax mem in bindingsRoot.Members)
        {
            if (mem is NamespaceDeclarationSyntax namespace_)
            {
                // add all function bindings first
                var methodsClass = namespace_.Members.Where(e => e.Kind() == SyntaxKind.ClassDeclaration && ((ClassDeclarationSyntax)e).Identifier.Text == "Methods").FirstOrDefault() as ClassDeclarationSyntax;
                foreach (MemberDeclarationSyntax methodClassMember in methodsClass.Members)
                {
                    if (!methodClassMember.IsKind(SyntaxKind.MethodDeclaration))
                        continue;

                    var method = (MethodDeclarationSyntax)methodClassMember;
                    if (ManuallyImplementedFunctions.Contains(method.Identifier.ValueText))
                        continue;

                    // Strip attributes.
                    var newParamList = new List<ParameterSyntax>();
                    foreach (var param in method.ParameterList.Parameters)
                    {
                        // Skip arglist (it's fine to skip, according to CppSharp. Not like we need it anyway.)
                        if (param.Identifier.IsKind(SyntaxKind.ArgListKeyword))
                            continue;

                        TypeSyntax newType = FixupParameterType(param.Type, param.AttributeLists, ImGuiStructTypeKind.ForInterface, modifiers: out SyntaxTokenList modifiers);
                        newType = FixParameterTypeForFunctionCall(newType, ref modifiers);

                        ParameterSyntax newParam = param.WithType(newType)
                            .WithModifiers(modifiers)
                            .WithAttributeLists([]);
                        newParamList.Add(newParam);
                    }

                    var paramListSyntaxList = new SeparatedSyntaxList<ParameterSyntax>();
                    paramListSyntaxList = paramListSyntaxList.AddRange(newParamList);

                    // Create argument list for call
                    var implCallArguments = new List<SyntaxNodeOrToken>();
                    for (int i = 0; i < newParamList.Count; i++)
                    {
                        var param = newParamList[i];
                        ExpressionSyntax newExpression;

                        // Try to resolve impl into native pointer for call. (implStruct.NativePointer)
                        string typeName = param.Type!.GetText().ToString().TrimEnd();
                        if (param.Type.IsKind(SyntaxKind.IdentifierName) && 
                            method.ParameterList.Parameters[i].Type.IsKind(SyntaxKind.PointerType) &&
                            !TypeInfo.CSharpNoPointerIdentifiers.Contains(typeName) && !TypeInfo.KnownEnums.Contains(typeName))
                        {
                            // cast nint pointer to struct pointer - '(struct*)ctx.NativePointer'
                            string structName = $"{typeName.Substring(1)}Struct";
                            newExpression = SyntaxTreeUtils.CreatePointerArgumentWithNullPatternExpression(structName, param.Identifier.Text, "NativePointer");
                        }
                        else if (TypeInfo.KnownEnums.Contains(typeName))
                        {
                            newExpression = SF.ParseExpression($"(int){param.Identifier.Text}");
                        }
                        else if (TypeInfo.ManuallyDeclaredValueTypesAsInterfaces.Contains(typeName.Substring(1))) // ImTextureRef
                        {
                            // Converting an interface for a value type, back to a struct to pass as argument
                            // ToStruct methods are expected on the implementation.
                            newExpression = SF.ParseExpression($"(({typeName.Substring(1)}){param.Identifier.Text}).ToStruct()");
                        }
                        else if (typeName.StartsWith("ImVectorWrapper")) // For weird cases like 'ImGuiTextFilter_ImGuiTextRange_split'.
                        {
                            // Hacky. but works
                            GenericNameSyntax genericType = (GenericNameSyntax)param.Type;
                            string elemTypeName = genericType.TypeArgumentList.Arguments[0].ToString();
                            newExpression = SF.ParseExpression($"ref Unsafe.AsRef<ImVector<{elemTypeName.Substring(1)}Struct>>(&{param.Identifier.Text}.Size)");
                        }
                        else
                        {
                            // Otherwise just pass the argument as is
                            newExpression = SF.IdentifierName(param.Identifier.Text);
                        }

                        // Add ref if needed.
                        bool withRefKeyword = param.Modifiers.Any(e => e.IsKind(SyntaxKind.RefKeyword));
                        ArgumentSyntax newArgument = SF.Argument(newExpression);
                        if (withRefKeyword)
                            newArgument = newArgument.WithRefKindKeyword(SF.Token(SyntaxKind.RefKeyword));
                        implCallArguments.Add(newArgument);

                        if (i != newParamList.Count - 1)
                            implCallArguments.Add(SF.Token(SyntaxKind.CommaToken));
                    }


                    // Fix up return types
                    string? nativeTypeName = GetNativeTypeName(method.AttributeLists);
                    TypeSyntax returnType = method.ReturnType;
                    if (nativeTypeName is not null)
                        returnType = FixupParameterType(method.ReturnType, method.AttributeLists, ImGuiStructTypeKind.ForInterface, modifiers: out SyntaxTokenList modifiers);

                    ExpressionSyntax callExpression = SF.InvocationExpression(
                        expression: SF.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SF.IdentifierName("ImGuiMethods"),
                            SF.IdentifierName(method.Identifier.Text)),
                        argumentList: SF.ArgumentList(SF.SeparatedList<ArgumentSyntax>(implCallArguments)));

                    /*
                    // Put return pointer of native call into wrapper if needed.
                    // IImGuiPayload GetDragDropPayload() => new ImGuiPayload(ImGuiMethods.GetDragDropPayload());
                    if (returnType.IsKind(SyntaxKind.IdentifierName))
                    {
                        IdentifierNameSyntax returnIdentifier = (IdentifierNameSyntax)returnType;
                        string identifierName = returnIdentifier.Identifier.ValueText;
                        if (!TypeInfo.CSharpNoPointerIdentifiers.Contains(identifierName) && !TypeInfo.KnownEnums.Contains(identifierName))
                        {
                            string implName = returnIdentifier.Identifier.ValueText.Substring(1);
                            callExpression = SF.ObjectCreationExpression(
                                SF.IdentifierName(implName),
                                SF.ArgumentList(SF.SeparatedList([SF.Argument(callExpression)])),
                                null);
                        }
                    }
                    */

                    MethodDeclarationSyntax newMethod = SF.MethodDeclaration(
                            attributeLists: [],
                            modifiers: SF.TokenList(SF.Token(SyntaxKind.PublicKeyword)),
                            returnType,
                            explicitInterfaceSpecifier: null,
                            method.Identifier,
                            method.TypeParameterList,
                            SF.ParameterList(paramListSyntaxList),
                            method.ConstraintClauses,
                            body: null,
                            expressionBody: null);

                    // For functions that return strings, we create a body that checks the return value.
                    // We can't just use MarshalAs for returned strings, something to do with memory management.
                    if (returnType.IsKind(SyntaxKind.PredefinedType))
                    {
                        var predefinedType = (PredefinedTypeSyntax)returnType;
                        if (predefinedType.Keyword.ValueText == "string")
                        {
                            var body = SF.Block(SF.List([
                                SF.LocalDeclarationStatement(
                                    SF.VariableDeclaration(method.ReturnType)
                                    .WithVariables(
                                        SF.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SF.VariableDeclarator(
                                                SF.Identifier("retStrPtr"))
                                            .WithInitializer(SF.EqualsValueClause(callExpression))))),
                                SF.ParseStatement("if (retStrPtr is null)"),
                                SF.ParseStatement("    return null!;"),
                                SF.ParseStatement("string retStr = Marshal.PtrToStringUTF8((nint)retStrPtr)!;"),
                                SF.ParseStatement("return retStr;")
                            ]));
                            newMethod = newMethod.WithBody(body);
                        }
                        else
                        {
                            newMethod = newMethod.WithExpressionBody(SF.ArrowExpressionClause(callExpression))
                                .WithSemicolonToken(method.SemicolonToken);
                        }
                    }
                    else if (returnType.IsKind(SyntaxKind.IdentifierName))
                    {
                        // Add a null check for methods that return a structure pointer.
                        IdentifierNameSyntax returnIdentifier = (IdentifierNameSyntax)returnType;
                        string identifierName = returnIdentifier.Identifier.ValueText;
                        if (!TypeInfo.CSharpNoPointerIdentifiers.Contains(identifierName) && !TypeInfo.KnownEnums.Contains(identifierName))
                        {
                            List<StatementSyntax> statements = [];
                            statements.Add(SF.LocalDeclarationStatement(
                                    SF.VariableDeclaration(SF.ParseTypeName("var"))
                                    .WithVariables(
                                        SF.SingletonSeparatedList<VariableDeclaratorSyntax>(
                                            SF.VariableDeclarator(
                                                SF.Identifier("ret"))
                                            .WithInitializer(SF.EqualsValueClause(callExpression))))));

                            // If it's not a value type, include a null check
                            if (!TypeInfo.ManuallyDeclaredValueTypesAsInterfaces.Contains(identifierName.Substring(1)))
                            {
                                statements.Add(SF.ParseStatement("if (ret is null)"));
                                statements.Add(SF.ParseStatement("    return null!;"));
                            }
                            
                            statements.Add(SF.ParseStatement($"return new {returnIdentifier.Identifier.ValueText.Substring(1)}(ret);"));

                            var body = SF.Block(SF.List(statements));
                            newMethod = newMethod.WithBody(body);
                        }
                        else if (TypeInfo.KnownEnums.Contains(identifierName))
                        {
                            // Cast enum result from call (int) to actual enum type.
                            newMethod = newMethod.WithExpressionBody(SF.ArrowExpressionClause(SF.CastExpression(SF.ParseTypeName(identifierName), callExpression)))
                                .WithSemicolonToken(method.SemicolonToken);
                        }
                        else
                        {
                            newMethod = newMethod.WithExpressionBody(SF.ArrowExpressionClause(callExpression))
                                .WithSemicolonToken(method.SemicolonToken);
                        }
                    }
                    else
                    {
                        newMethod = newMethod.WithExpressionBody(SF.ArrowExpressionClause(callExpression))
                            .WithSemicolonToken(method.SemicolonToken);
                    }

                    _implMemberList.Add(newMethod);
                }

                // Add everything else next
                foreach (var member in namespace_.Members)
                {
                    if ((member.IsKind(SyntaxKind.ClassDeclaration) && ((ClassDeclarationSyntax)member).Identifier.Text == "Methods") ||
                        member.IsKind(SyntaxKind.EnumDeclaration))
                        continue;

                    if (member.IsKind(SyntaxKind.StructDeclaration))
                    {
                        StructDeclarationSyntax structDeclaration = (StructDeclarationSyntax)member;

                        string structName = structDeclaration.Identifier.ValueText;
                        if (structName.EndsWith("_t"))
                            structName = structName.Substring(0, structName.Length - 2);

                        // Pointless to redeclare these
                        if (TypeInfo.SkipDeclareIdentifiers.Contains(structName) || structName.StartsWith("ImVector"))
                            continue;

                        List<MemberDeclarationSyntax> newStructMembers = [];

                        // Create important properties first for the impl
                        PropertyDeclarationSyntax nativePtrProperty = SyntaxTreeUtils.CreateNativePointerMember("nint");
                        newStructMembers.Add(nativePtrProperty);

                        // Ctors
                        ConstructorDeclarationSyntax nativePtrCtor = SyntaxTreeUtils.CreatePointerToImplStructConstructorDeclaration(structName, $"{structName}Struct", "NativePointer");
                        newStructMembers.Add(nativePtrCtor);

                        foreach (MemberDeclarationSyntax structMember in structDeclaration.Members)
                        {
                            if (structMember.IsKind(SyntaxKind.FieldDeclaration))
                            {
                                FieldDeclarationSyntax fieldDeclaration = (FieldDeclarationSyntax)structMember;
                                VariableDeclarationSyntax variableDeclaration = fieldDeclaration.Declaration;

                                if (variableDeclaration.Type.ToString().StartsWith("_Anonymous"))
                                {
                                    // We don't care for defining anonymous unions.
                                    // We will declare the members of that directly instead.
                                    continue;
                                }
                                
                                var fieldName = variableDeclaration.Variables[0].Identifier.ValueText;
                                StructFieldMetadata? fieldMetadata = _metadata.StructsByName![structName].Fields.FirstOrDefault(e => e.Name == fieldName);

                                TypeSyntax newType = FixupParameterType(variableDeclaration.Type, fieldDeclaration.AttributeLists, ImGuiStructTypeKind.ForInterface, 
                                    isArrayElement: fieldMetadata?.IsArray == true, doNotRemapStrings: true,
                                    modifiers: out SyntaxTokenList modifiers);

                                PropertyDeclarationSyntax newProperty = CreatePropertyWithImplementation(fieldDeclaration, variableDeclaration, structName + "Struct", fieldName, ref newType);
                                newStructMembers.Add(newProperty!);
                            }
                            else if (structMember.IsKind(SyntaxKind.PropertyDeclaration))
                            {
                                PropertyDeclarationSyntax propertyDeclaration = (PropertyDeclarationSyntax)structMember;
                                PropertyDeclarationSyntax newProperty = ProcessPropertyWithImplementation(propertyDeclaration, structName + "Struct");

                                newStructMembers.Add(newProperty);
                            }
                            else if (structMember.IsKind(SyntaxKind.StructDeclaration))
                            {
                                // Nested structs are emitted by ClangSharpPInvokeGenerator when a certain field cannot be wrapped easily
                                // We wrap them ourselves, so we can skip them
                                continue;
                            }
                            else
                                newStructMembers.Add(structMember);
                        }

                        StructDeclarationSyntax newStruct = SF.StructDeclaration(structName)
                        .WithBaseList(SF.BaseList(SF.SeparatedList<BaseTypeSyntax>(new List<BaseTypeSyntax>()
                        {
                            SF.SimpleBaseType(SF.ParseTypeName($"I{structName}")),
                        })))
                        .WithMembers(SF.List<MemberDeclarationSyntax>(newStructMembers))
                        .WithModifiers(SF.TokenList([
                            SF.Token(SyntaxKind.PublicKeyword),
                            SF.Token(SyntaxKind.UnsafeKeyword),
                            SF.Token(SyntaxKind.PartialKeyword),
                        ]));

                        _implMemberList.Add(newStruct);
                    }
                    else
                    {
                        _implMemberList.Add(member);
                    }
                }
            }
        }
    }

    private PropertyDeclarationSyntax CreatePropertyWithImplementation(FieldDeclarationSyntax fieldDeclaration, VariableDeclarationSyntax variableDeclaration, 
        string nativeStructName, string fieldName, ref TypeSyntax newType)
    {
        bool withGetter = false;
        bool withSetter = false;
        ExpressionSyntax? getterArrowExpression = null;
        ExpressionSyntax? setterArrowExpression = null;
        ExpressionSyntax? bodyExpression = null;

        string newTypeName = newType.GetText().ToString().TrimEnd(); // Trim 'I'
        List<SyntaxToken> newModifiers = [SF.Token(SyntaxKind.PublicKeyword), SF.Token(SyntaxKind.ReadOnlyKeyword)];

        if (IsRefableProperty(newType))
        {
            bodyExpression = SyntaxTreeUtils.CreateAsRefExpression(newType.GetText().ToString().TrimEnd(), nativeStructName, "NativePointer", fieldName);
            newType = SF.RefType(newType);
        }
        else if (newType.IsKind(SyntaxKind.PointerType) || newType.IsKind(SyntaxKind.FunctionPointerType) || newTypeName == "nint" || newTypeName == "nuint")
        {
            withGetter = true;
            withSetter = true;

            getterArrowExpression = SyntaxTreeUtils.CreatePointerAccessExpressionForGetter(nativeStructName, "NativePointer", fieldName);
            setterArrowExpression = SyntaxTreeUtils.CreatePointerAccessExpressionForSetter(nativeStructName, "NativePointer", fieldName);
        }
        else if (newType.IsKind(SyntaxKind.IdentifierName))
        {
            if (variableDeclaration.Type.IsKind(SyntaxKind.PointerType))
            {
                // Create implementation for a member that is a struct pointer
                bodyExpression = SyntaxTreeUtils.CreatePointerToConstructorCallExpression(newTypeName[1..], // Trim 'I' 
                    nativeStructName, "NativePointer", fieldName);
            }
            else
            {
                // Create implementation for a member that is a struct, so simply take its address
                bodyExpression = SyntaxTreeUtils.CreateStructureToConstructorCallExpression(newTypeName[1..], // Trim 'I' 
                    nativeStructName, "NativePointer", fieldName);
            }
        }
        else if (newType.IsKind(SyntaxKind.GenericName))
        {
            GenericNameSyntax genericName = (GenericNameSyntax)newType;
            string genericTypeName = genericName.TypeArgumentList.Arguments[0].ToString();
            if (newTypeName.Contains("RangeAccessor"))
            {
                string? nativeTypeName = GetNativeTypeName(fieldDeclaration.AttributeLists) ??
                    throw new InvalidDataException("Failed to get type name for generic element");

                if (!TryGetNativeTypeArrayLength(nativeTypeName, out _, out int length))
                    throw new InvalidDataException("Failed to parse native type name array");

                bodyExpression = SyntaxTreeUtils.CreateRangeAccessorExpression(genericTypeName, nativeStructName, "NativePointer", fieldName, length);
            }
            else if (newTypeName.Contains("RangeStructAccessor"))
            {
                string? nativeTypeName = GetNativeTypeName(fieldDeclaration.AttributeLists) ??
                    throw new InvalidDataException("Failed to get type name for generic element");

                if (!TryGetNativeTypeArrayLength(nativeTypeName, out _, out int length))
                    throw new InvalidDataException("Failed to parse native type name array");

                bodyExpression = SyntaxTreeUtils.CreateRangeStructAccessorExpression(genericTypeName, genericTypeName.Substring(1), nativeStructName, "NativePointer", fieldName, length);
            }
            else if (newTypeName.Contains("ImStructPtrVectorPtrWrapper"))
            {
                bodyExpression = SyntaxTreeUtils.CreateImStructPtrVectorPtrAccess(genericTypeName, genericTypeName.Substring(1), nativeStructName, "NativePointer", fieldName);
            }
            else if (newTypeName.Contains("ImStructPtrVectorWrapper"))
            {
                bodyExpression = SyntaxTreeUtils.CreateImStructPtrVectorAccess(genericTypeName, genericTypeName.Substring(1), nativeStructName, "NativePointer", fieldName);
            }
            else if (newTypeName.Contains("ImVectorWrapper"))
            {
                if (genericTypeName.StartsWith("II"))
                    bodyExpression = SyntaxTreeUtils.CreateImPtrVectorAccess(genericTypeName, genericTypeName.Substring(1), nativeStructName, "NativePointer", fieldName);
                else
                    bodyExpression = SyntaxTreeUtils.CreateImPtrVectorAccessForPrimitive(genericTypeName, genericTypeName, nativeStructName, "NativePointer", fieldName);
            }
        }

        PropertyDeclarationSyntax newProperty = SF.PropertyDeclaration(newType, variableDeclaration.Variables[0].Identifier)
            .WithModifiers(SF.TokenList(newModifiers))
            .WithExpressionBody(bodyExpression is not null ? SF.ArrowExpressionClause(bodyExpression) : null)
            .WithAccessorList(SyntaxTreeUtils.GetAccessorList(withGetter, getterArrowExpression, withSetter, setterArrowExpression));

        if (newProperty.AccessorList?.Accessors.Count == null || newProperty.AccessorList?.Accessors.Count == 0)
            newProperty = newProperty.WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));

        return newProperty;
    }

    private PropertyDeclarationSyntax ProcessPropertyWithImplementation(PropertyDeclarationSyntax propertyDeclaration, string nativeStructName)
    {
        bool withGetter = false;
        bool withSetter = false;
        ExpressionSyntax? getterArrowExpression = null;
        ExpressionSyntax? setterArrowExpression = null;
        ArrowExpressionClauseSyntax? arrowExpression = null;
        TypeSyntax newType = propertyDeclaration.Type;

        string fieldName = propertyDeclaration.Identifier.ValueText;
        if (IsRefableProperty(propertyDeclaration.Type))
        {
            withGetter = true;
            withSetter = true;
            getterArrowExpression = SyntaxTreeUtils.CreatePointerAccessExpressionForGetter(nativeStructName, "NativePointer", fieldName);
            setterArrowExpression = SyntaxTreeUtils.CreatePointerAccessExpressionForSetter(nativeStructName, "NativePointer", fieldName);
        }
        else
        {
            if (propertyDeclaration.Type.IsKind(SyntaxKind.RefType))
            {
                newType = FixupParameterType(propertyDeclaration.Type, propertyDeclaration.AttributeLists, ImGuiStructTypeKind.ForImpl, out _);

                // Originally ref value type from possibly an union?
                string implTypeName = newType.ToString().Substring(1);
                if (TypeInfo.ManuallyDeclaredValueTypesAsInterfaces.Contains(implTypeName))
                {
                    withGetter = true;
                    withSetter = true;
                    getterArrowExpression = SF.ParseExpression($"new {implTypeName}((({nativeStructName}*)NativePointer)->{fieldName})");
                    setterArrowExpression = SF.ParseExpression($"(({nativeStructName}*)NativePointer)->{fieldName} = (({implTypeName})value).ToStruct()");
                }
                else
                {
                    var body = SyntaxTreeUtils.CreateRefToPointerConstructorCall(newType.ToString(), nativeStructName, "NativePointer", fieldName);
                    arrowExpression = SF.ArrowExpressionClause(body);
                }
            }
        }

        List<SyntaxToken> newModifiers = [SF.Token(SyntaxKind.PublicKeyword), SF.Token(SyntaxKind.ReadOnlyKeyword)];
        var newProp = propertyDeclaration.WithAccessorList(SyntaxTreeUtils.GetAccessorList(withGetter, getterArrowExpression, withSetter, setterArrowExpression))
            .WithAttributeLists([])
            .WithType(newType)
            .WithExpressionBody(arrowExpression)
            .WithModifiers(SF.TokenList(newModifiers));

        if (newProp.AccessorList?.Accessors.Count == null || newProp.AccessorList?.Accessors.Count == 0)
            newProp = newProp.WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));

        return newProp;
    }

    public string FinishImpl()
    {
        ClassDeclarationSyntax implClass = SF.ClassDeclaration("ImGui")
            .WithBaseList(SF.BaseList(SF.SeparatedList<BaseTypeSyntax>(new List<BaseTypeSyntax>()
            {
                SF.SimpleBaseType(SF.ParseTypeName("IImGui")),
            })))
            .WithModifiers(SF.TokenList([
                SF.Token(SyntaxKind.PublicKeyword),
                SF.Token(SyntaxKind.UnsafeKeyword),
                SF.Token(SyntaxKind.PartialKeyword),
             ]))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(_implMemberList));

        FileScopedNamespaceDeclarationSyntax namespaceDecl = SF.FileScopedNamespaceDeclaration(SF.ParseName(_implNamespace))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(implClass));

        CompilationUnitSyntax root = SF.CompilationUnit()
            .WithUsings(SF.List(new List<UsingDirectiveSyntax>()
            {
                SF.UsingDirective(SF.ParseName("System.Numerics")),
                SF.UsingDirective(SF.ParseName("System.Runtime.CompilerServices")),
                SF.UsingDirective(SF.ParseName("System.Runtime.InteropServices")),
                SF.UsingDirective(SF.ParseName(_interfaceNamespace)),
                SF.UsingDirective(SF.ParseName(_methodsNamespace)),
            }))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(new List<MemberDeclarationSyntax>()
            {
                namespaceDecl,
            }))
            .WithLeadingTrivia(SF.Comment("// This file was generated with ImGuiInterfaceGenerator"))
            .NormalizeWhitespace();

        SyntaxTree interfaceTree = CSharpSyntaxTree.Create(root);
        return interfaceTree.ToString();
    }
    #endregion

    #region Bindings Generation
    public void ExtractBindingsFromSyntaxTree(SyntaxTree syntaxTree, bool isBackend)
    {
        var bindingsRoot = syntaxTree.GetCompilationUnitRoot();
        foreach (MemberDeclarationSyntax mem in bindingsRoot.Members)
        {
            if (mem is NamespaceDeclarationSyntax namespace_)
            {
                // add all function bindings first
                var methodsClass = namespace_.Members.Where(e => e.Kind() == SyntaxKind.ClassDeclaration && ((ClassDeclarationSyntax)e).Identifier.Text == "Methods").FirstOrDefault() as ClassDeclarationSyntax 
                    ?? throw new InvalidDataException("Could not find Methods class");

                foreach (var methodClassMember in methodsClass.Members)
                {
                    if (!methodClassMember.IsKind(SyntaxKind.MethodDeclaration))
                        continue;

                    var method = (MethodDeclarationSyntax)methodClassMember;
 
                    var newParamList = new List<ParameterSyntax>();
                    foreach (var param in method.ParameterList.Parameters)
                    {
                        // Skip arglist (it's fine to skip, according to CppSharp. Not like we need it anyway.)
                        if (param.Identifier.IsKind(SyntaxKind.ArgListKeyword))
                            continue;

                        TypeSyntax newType = FixupParameterType(param.Type, param.AttributeLists, ImGuiStructTypeKind.ForNativeStruct, pointersToNint: isBackend, modifiers: out SyntaxTokenList modifiers);
                        newType = FixParameterTypeForFunctionCall(newType, ref modifiers);

                        ParameterSyntax newParam = param.WithType(newType)
                            .WithModifiers(modifiers)
                            .WithAttributeLists([]);

                        if (newParam.Type.IsKind(SyntaxKind.PredefinedType))
                        {
                            PredefinedTypeSyntax predef = (PredefinedTypeSyntax)newParam.Type;
                            if (predef.Keyword.Text == "string")
                                newParam = newParam.WithAttributeLists(SyntaxTreeUtils.GetMarshalAsAttribute(UnmanagedType.LPUTF8Str));
                        }

                        newParamList.Add(newParam);
                    }

                    // Remove NativeTypeName attributes.
                    List<AttributeListSyntax> methodAttributes = method.AttributeLists.ToList();
                    for (int i = methodAttributes.Count - 1; i >= 0; i--)
                    {
                        AttributeListSyntax? methodAttribute = methodAttributes[i];
                        if (methodAttribute.Attributes.Any(e => e.Name.ToString() == "NativeTypeName"))
                            methodAttributes.Remove(methodAttribute);
                    }

                    var paramListSyntaxList = new SeparatedSyntaxList<ParameterSyntax>();
                    paramListSyntaxList = paramListSyntaxList.AddRange(newParamList);

                    // Fixup return types
                    string? nativeTypeName = GetNativeTypeName(method.AttributeLists);
                    TypeSyntax returnType = method.ReturnType;
                    if (nativeTypeName is not null)
                    {
                        returnType = FixupParameterType(method.ReturnType, method.AttributeLists, ImGuiStructTypeKind.ForNativeStruct, modifiers: out SyntaxTokenList modifiers,
                            doNotRemapStrings: true); // <- Important. We need the runtime not to try marshal/free certain returned strings as that causes a crash
                    }

                    _bindingsMethodList.Add(
                            SF.MethodDeclaration(
                                attributeLists: SF.List(methodAttributes),
                                modifiers: method.Modifiers,
                                returnType,
                                explicitInterfaceSpecifier: null!,
                                method.Identifier,
                                method.TypeParameterList!,
                                SF.ParameterList(paramListSyntaxList),
                                method.ConstraintClauses,
                                body: null!,
                                method.SemicolonToken)
                        );
                }

                // Add raw native structures
                foreach (var member in namespace_.Members)
                {
                    if (member.IsKind(SyntaxKind.ClassDeclaration) && ((ClassDeclarationSyntax)member).Identifier.Text == "Methods" ||
                        member.IsKind(SyntaxKind.EnumDeclaration)) // No point declaring enums as we convert them to integers (enums belong to the interface)
                        continue;

                    // Trim enum names.
                    if (member.IsKind(SyntaxKind.StructDeclaration))
                    {
                        // Skip structures from backends. We expect the interface to just pass pointers
                        if (isBackend)
                            continue;

                        StructDeclarationSyntax structDeclaration = (StructDeclarationSyntax)member;

                        string structName = structDeclaration.Identifier.ValueText;
                        if (structName.EndsWith("_t"))
                            structName = structName.Substring(0, structName.Length - 2);

                        // Pointless to redeclare these
                        if (TypeInfo.SkipDeclareIdentifiers.Contains(structName)|| structName.StartsWith("ImVector"))
                            continue;

                        List<MemberDeclarationSyntax> newStructMembers = [];
                        foreach (MemberDeclarationSyntax structMember in structDeclaration.Members)
                        {
                            if (structMember.IsKind(SyntaxKind.FieldDeclaration))
                            {
                                FieldDeclarationSyntax fieldDeclaration = (FieldDeclarationSyntax)structMember;
                                VariableDeclarationSyntax variableDeclaration = fieldDeclaration.Declaration;

                                TypeSyntax newType = FixupParameterType(variableDeclaration.Type, fieldDeclaration.AttributeLists, ImGuiStructTypeKind.ForNativeStruct, out SyntaxTokenList modifiers, doNotRemapStrings: true);
                                SyntaxList<AttributeListSyntax> attributeLists = [];
                                if (newType is PredefinedTypeSyntax predefinedType)
                                {
                                    if (predefinedType.Keyword.ValueText == "bool")
                                        attributeLists = SyntaxTreeUtils.GetMarshalAsAttribute(UnmanagedType.I1);
                                }

                                FieldDeclarationSyntax newDecl = fieldDeclaration.WithDeclaration(variableDeclaration.WithType(newType))
                                    .WithAttributeLists(attributeLists);

                                newStructMembers.Add(newDecl);
                            }
                            else if (structMember.IsKind(SyntaxKind.PropertyDeclaration))
                            {
                                PropertyDeclarationSyntax propertyDecl = (PropertyDeclarationSyntax)structMember;
                                TypeSyntax newType = FixupParameterType(propertyDecl.Type, propertyDecl.AttributeLists, ImGuiStructTypeKind.ForNativeStruct, out _);
                                newStructMembers.Add(propertyDecl.WithAttributeLists([])
                                    .WithType(newType));
                            }
                            else if (structMember.IsKind(SyntaxKind.StructDeclaration)) // Nested struct. Sanitize it/remove useless properties
                            {
                                StructDeclarationSyntax nestedStruct = (StructDeclarationSyntax)structMember;
                                List<MemberDeclarationSyntax> newNestedStructMembers = ProcessNestedNativeStruct(nestedStruct);
                                newStructMembers.Add(nestedStruct.WithMembers(SF.List(newNestedStructMembers)));
                            }
                            else
                                newStructMembers.Add(structMember);
                        }

                        StructDeclarationSyntax newClass = SF.StructDeclaration($"{structName}Struct")
                        .WithMembers(SF.List<MemberDeclarationSyntax>(newStructMembers))
                        .WithModifiers(SF.TokenList([
                            SF.Token(SyntaxKind.PublicKeyword),
                            SF.Token(SyntaxKind.UnsafeKeyword),
                            SF.Token(SyntaxKind.PartialKeyword),
                        ]));

                        _bindingsMemberList.Add(newClass);
                    }
                    else
                    {
                        _bindingsMemberList.Add(member);
                    }
                }
            }

        }
    }

    private static TypeSyntax FixParameterTypeForFunctionCall(TypeSyntax type, ref SyntaxTokenList modifiers)
    {
        // Pass vectors as reference.
        if (type.IsKind(SyntaxKind.GenericName) && type is GenericNameSyntax genericName)
        {
            if (genericName.Identifier.ValueText == "ImVector")
                modifiers = modifiers.Add(SF.Token(SyntaxKind.RefKeyword));
        }
        else if (TypeInfo.CallArgumentPointerTypesThatShouldPassByReference.Contains(type.ToString()))
        {
            PointerTypeSyntax pointerType = (PointerTypeSyntax)type;
            type = SF.ArrayType(pointerType.ElementType);
            modifiers = modifiers.Add(SF.Token(SyntaxKind.RefKeyword));
        }

        return type;
    }

    private List<MemberDeclarationSyntax> ProcessNestedNativeStruct(StructDeclarationSyntax nestedStruct)
    {
        var oldNestedMembers = new List<MemberDeclarationSyntax>();
        foreach (var nestedMember in nestedStruct.Members)
        {
            if (nestedMember.IsKind(SyntaxKind.FieldDeclaration))
            {
                FieldDeclarationSyntax nestedFieldDeclaration = (FieldDeclarationSyntax)nestedMember;
                VariableDeclarationSyntax variableDeclaration = nestedFieldDeclaration.Declaration;

                TypeSyntax newType = FixupParameterType(variableDeclaration.Type, nestedStruct.AttributeLists, ImGuiStructTypeKind.ForNativeStruct, out SyntaxTokenList modifiers, doNotRemapStrings: true);
                FieldDeclarationSyntax newDecl = nestedFieldDeclaration.WithDeclaration(variableDeclaration.WithType(newType))
                    .WithAttributeLists(AttributeListWithoutAttributeName(nestedFieldDeclaration.AttributeLists, "NativeTypeName"));

                oldNestedMembers.Add(newDecl);
            }
        }
        return oldNestedMembers;
    }

    public string FinishBindings()
    {
        ClassDeclarationSyntax bindingsClass = SF.ClassDeclaration("ImGuiMethods")
            .WithModifiers(SF.TokenList(
                SF.Token(
                    SF.TriviaList([
                        SyntaxTreeUtils.CreateWarningDirectiveTrivia("CA1401", "// P/Invokes should not be visible"),
                        SyntaxTreeUtils.CreateWarningDirectiveTrivia("SYSLIB1054", "// Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time"),
                        SyntaxTreeUtils.CreateWarningDirectiveTrivia("CA2101", "// Specify marshaling for P/Invoke string arguments"),
                ]), SyntaxKind.PublicKeyword, SF.TriviaList()),
                SF.Token(SyntaxKind.UnsafeKeyword),
                SF.Token(SyntaxKind.PartialKeyword)
            ))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(_bindingsMethodList));

        var allMembers = new List<MemberDeclarationSyntax>();
        allMembers.Add(bindingsClass);
        allMembers.AddRange(_bindingsMemberList);

        FileScopedNamespaceDeclarationSyntax namespaceDecl = SF.FileScopedNamespaceDeclaration(SF.ParseName(_methodsNamespace))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(allMembers));

        CompilationUnitSyntax root = SF.CompilationUnit()
            .WithUsings(SF.List(new List<UsingDirectiveSyntax>()
            {
                SF.UsingDirective(SF.ParseName("System")),
                SF.UsingDirective(SF.ParseName("System.Runtime.CompilerServices")),
                SF.UsingDirective(SF.ParseName("System.Runtime.InteropServices")),
                SF.UsingDirective(SF.ParseName("System.Numerics")),
            }))
            .WithMembers(new SyntaxList<MemberDeclarationSyntax>(namespaceDecl))
            .NormalizeWhitespace();

        SyntaxTree interfaceTree = CSharpSyntaxTree.Create(root);
        return interfaceTree.ToString();
    }
    #endregion

    /// <summary>
    /// Fixes a type from the original bindings.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="attributeLists"></param>
    /// <param name="modifiers"></param>
    /// <param name="doNotRemapStrings">Whether to not remap string pointers (char*) to string.</param>
    /// <param name="pointersToNint">Whether to convert any pointer structs to <see cref="nint"/> (mainly used for backends)</param>
    /// <param name="typeFixKind">Type context to fix the type to.</param>
    /// <param name="isArrayElement">Whether this is an inline array element. If so, RangeAccessor will be used.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>    
    private TypeSyntax FixupParameterType(TypeSyntax type, SyntaxList<AttributeListSyntax> attributeLists,
        ImGuiStructTypeKind typeFixKind,
        out SyntaxTokenList modifiers, bool doNotRemapStrings = false, bool pointersToNint = false, bool isArrayElement = false)
    {
        modifiers = [];

        // We only use the metadata here for vectors.

        string? nativeTypeName = GetNativeTypeName(attributeLists);

        // We try fixing up vectors
        if (nativeTypeName?.Contains("ImVector") == true)
        {
            if (nativeTypeName.EndsWith("Ptr *"))
            {
                if (type is PointerTypeSyntax pointerType)
                {
                    var elemType = pointerType.ElementType as IdentifierNameSyntax;
                    string typeName = TrimTypeName(elemType.Identifier.ValueText);

                    StructMetadata structInfo = _metadata.StructsByName![typeName];
                    var data = structInfo.Fields.FirstOrDefault(e => e.Name == "Data");
                    var vectorTypeDesc = (PointerDescriptionMetadata)((PointerDescriptionMetadata)data!.Type.TypeDescription!).InnerType!;
                    string vectorElemName = ((UserDescriptionMetadata)vectorTypeDesc!.InnerType!).Name!;

                    if (TypeInfo.TryGetWellknownType(vectorElemName, out _))
                        return SF.ParseTypeName($"Hello<{GetCorrectedType(vectorElemName)}>");
                    else
                    {
                        if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                            return SF.ParseTypeName($"IImStructPtrVectorPtrWrapper<I{GetCorrectedType(vectorElemName)}>");
                        else if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
                            return SF.ParseTypeName($"ImStructPtrVectorPtr<{GetCorrectedType(vectorElemName)}Struct>");
                    }
                }
            }

            TypeSyntax? vectorType = HandleVectorType(type, typeFixKind);
            if (vectorType is not null)
                return vectorType;
        }

        // try to adjust type name based on original type
        if (nativeTypeName is not null)
        {
            if (isArrayElement)
            {
                if (!TryGetNativeTypeArrayLength(nativeTypeName, out string? name, out _))
                    throw new InvalidDataException("Failed to parse native type name array");

                if (TypeInfo.TryGetWellknownType(name, out _) || TypeInfo.BasicRefableTypes.TryGetValue(name, out _))
                {
                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        return SF.ParseTypeName($"IRangeAccessor<{GetCorrectedType(name)}>");
                    else
                        return SF.ParseTypeName($"RangeAccessor<{GetCorrectedType(name)}>");
                }
                else
                {
                    // Structure we don't know about. Put into a range accessor that will wrap the element
                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        name = $"I{name}";

                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        return SF.ParseTypeName($"IRangeStructAccessor<{GetCorrectedType(name)}>");
                    else
                        return SF.ParseTypeName($"RangeStructAccessor<{GetCorrectedType(name)}>");
                }
            }
            else
            {
                string currentParamType = type.GetText().ToString().TrimEnd();
                var translated = TranslateTypeNameIntoCSharpType(currentParamType, nativeTypeName, doNotRemapStrings, castEnumsToInt: typeFixKind == ImGuiStructTypeKind.ForNativeStruct);
                if (translated is not null)
                    return translated;
            }
        }

        // try fixing unmanaged function pointers callbacks. we convert arguments to nints if they're pointers,
        // don't really want to wrap these - kind of lazy.
        if (type.IsKind(SyntaxKind.FunctionPointerType))
            return FixupFunctionPointer(type);

        // try fixing union structs that have been turned into a ref i.e union { ImTextureRef }
        if (type.IsKind(SyntaxKind.RefType))
        {
            RefTypeSyntax refType = (RefTypeSyntax)type;
            string typeName = TrimTypeName(refType.Type.GetText().ToString().TrimEnd());

            // We should have these declared manually
            if (TypeInfo.ManuallyDeclaredValueTypesAsInterfaces.Contains(typeName))
            {
                if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
                    return SF.ParseTypeName($"ref {typeName}Struct");
                else if (typeFixKind == ImGuiStructTypeKind.ForInterface || typeFixKind == ImGuiStructTypeKind.ForImpl)
                    return SF.ParseTypeName($"I{typeName}");
            }
            else
                throw new NotSupportedException($"Ref type with unsupported value type {typeName}");
        }

        if (pointersToNint)
        {
            // We convert types we don't know about in backends to nint.
            if (type.IsKind(SyntaxKind.PointerType) || type.IsKind(SyntaxKind.IdentifierName))
                return SF.ParseTypeName("nint");
        }
        else if (type is not null)
        {
            // Replace sbyte* to string. We know that all sbyte are string references
            string paramTypeName = type.GetText().ToString();
            if (paramTypeName == "bool* ") // Replace bool* to ref bool
            {
                modifiers = SF.TokenList(SF.Token(SyntaxKind.RefKeyword));
                return SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword));
            }
        }

        // try fixing pointers.
        if (type.IsKind(SyntaxKind.PointerType))
        {
            PointerTypeSyntax pointerType = (PointerTypeSyntax)type;
            if (pointerType.ElementType.IsKind(SyntaxKind.IdentifierName))
            {
                var typeName = TrimTypeName(((IdentifierNameSyntax)pointerType.ElementType).Identifier.ValueText);

                if (typeName == "nuint")
                    return SF.ParseTypeName(typeName);
                else if (TypeInfo.TryGetWellknownType(typeName, out string? wellKnownTypeName))
                    return SF.ParseTypeName(wellKnownTypeName);
                else
                {
                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        return SF.ParseTypeName($"I{typeName}");
                    else if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
                        return SF.ParseTypeName($"{typeName}Struct*");
                    else if (typeFixKind == ImGuiStructTypeKind.ForImpl)
                        return SF.ParseTypeName(typeName);
                }
            }
        }
        else if (type.IsKind(SyntaxKind.RefType))
        {
            RefTypeSyntax refType = (RefTypeSyntax)type;
            if (refType.Type.IsKind(SyntaxKind.IdentifierName))
            {
                var typeName = TrimTypeName(((IdentifierNameSyntax)refType.Type).Identifier.ValueText);
                if (TypeInfo.TryGetWellknownType(typeName, out string? wellKnownTypeName))
                    return SF.ParseTypeName($"ref {wellKnownTypeName}");
                else if (TypeInfo.ManuallyDeclaredValueTypesAsInterfaces.Contains(typeName)) // ImTextureRef, etc
                    return SF.ParseTypeName(typeName);

                throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        // Fix remaining stray types.
        // Trim '_t' if needed.
        string currentTypeName = type.GetText().ToString().TrimEnd();
        if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
        {
            // Enums are converted to int
            if (TypeInfo.KnownEnums.Contains(currentTypeName))
                return SF.ParseTypeName("int");

            var newType = GetCorrectedType(currentTypeName);
            if (currentTypeName == newType && currentTypeName.EndsWith("_t"))
                return SF.ParseTypeName(currentTypeName.Replace("_t", "Struct"));

            return SF.ParseTypeName(newType);
        }
        else if (typeFixKind == ImGuiStructTypeKind.ForInterface)
        {
            if (currentTypeName.EndsWith("_t"))
                return SF.ParseTypeName($"I{currentTypeName.Replace("_t", string.Empty)}");
        }
        else if (typeFixKind == ImGuiStructTypeKind.ForImpl)
        {
            if (currentTypeName.EndsWith("_t"))
                return SF.ParseTypeName($"{currentTypeName.Replace("_t", string.Empty)}");
        }

        return type;
    }

    private TypeSyntax? HandleVectorType(TypeSyntax type, ImGuiStructTypeKind typeFixKind)
    {
        if (type is PointerTypeSyntax pointerType)
        {
            var elemType = pointerType.ElementType as IdentifierNameSyntax;
            string typeName = TrimTypeName(elemType.Identifier.ValueText);

            StructMetadata structInfo = _metadata.StructsByName![typeName];
            var data = structInfo.Fields.FirstOrDefault(e => e.Name == "Data");
            var vectorElementType = ((PointerDescriptionMetadata)data!.Type.TypeDescription!).InnerType;
            if (vectorElementType is UserDescriptionMetadata vectorElemUserDesc)
            {
                if (TypeInfo.TryGetWellknownType(vectorElemUserDesc!.Name, out _))
                {
                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        return SF.ParseTypeName($"IImVectorWrapper<{GetCorrectedType(vectorElemUserDesc.Name)}>");
                    else
                        return SF.ParseTypeName($"ImVector<{GetCorrectedType(vectorElemUserDesc.Name)}>");
                }
                else
                {
                    string vectorName = GetStructVectorNameForContext(typeFixKind);
                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        return SF.ParseTypeName($"{vectorName}<I{GetCorrectedType(vectorElemUserDesc.Name)}>");
                    else if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
                        return SF.ParseTypeName($"{vectorName}<{GetCorrectedType(vectorElemUserDesc.Name)}Struct>");
                }
            }
        }
        else
        {
            var identifierName = (IdentifierNameSyntax)type;
            string typeName = TrimTypeName(identifierName.Identifier.ValueText);

            StructMetadata structInfo = _metadata.StructsByName![typeName];
            StructFieldMetadata? data = structInfo.Fields.FirstOrDefault(e => e.Name == "Data");
            var pointerTypeDesc = data.Type.TypeDescription as PointerDescriptionMetadata;

            switch (pointerTypeDesc.InnerType)
            {
                case UserDescriptionMetadata userDesc:
                    {
                        bool hasWellknownTypeName = TypeInfo.TryGetWellknownType(userDesc.Name!, out string? wellKnownTypeName);
                        if (!hasWellknownTypeName)
                        {
                            if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                                return SF.ParseTypeName($"IImVectorWrapper<I{userDesc.Name ?? userDesc.Name}>");
                            else if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
                                return SF.ParseTypeName($"ImVector<{userDesc.Name ?? userDesc.Name}Struct>");
                            else
                                throw new NotSupportedException(); // TODO
                        }
                        else
                        {
                            if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                                return SF.ParseTypeName($"IImVectorWrapper<{wellKnownTypeName ?? userDesc.Name}>");
                            else if (typeFixKind == ImGuiStructTypeKind.ForImpl)
                                return SF.ParseTypeName($"ImVectorWrapper<{wellKnownTypeName ?? userDesc.Name}>");
                            else
                                return SF.ParseTypeName($"ImVector<{wellKnownTypeName ?? userDesc.Name}>");
                        }
                    }

                case BuiltinDescriptionMetadata builtin:
                    {
                        string correctedName = GetCorrectedType(builtin.BuiltinType!);
                        if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                            return SF.ParseTypeName($"IImVectorWrapper<{correctedName}>");
                        else if (typeFixKind == ImGuiStructTypeKind.ForImpl)
                            return SF.ParseTypeName($"ImVectorWrapper<{correctedName}>");
                        else
                            return SF.ParseTypeName($"ImVector<{correctedName}>");
                    }

                case PointerDescriptionMetadata pointerDesc:
                    string? name = ((UserDescriptionMetadata)pointerDesc.InnerType!).Name;
                    if (typeFixKind == ImGuiStructTypeKind.ForInterface)
                        name = $"I{name}";
                    else if (typeFixKind == ImGuiStructTypeKind.ForNativeStruct)
                        name = $"{name}Struct";

                    string vectorName = typeName.EndsWith("Ptr") ? GetStructPtrVectorNameForContext(typeFixKind) : "ImVectorWrapper";
                    return SF.ParseTypeName($"{vectorName}<{name}>");

                default:
                    throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }

        return null;
    }

    public static SyntaxList<AttributeListSyntax> AttributeListWithoutAttributeName(SyntaxList<AttributeListSyntax> list, string attributeName)
    {
        List<AttributeListSyntax> newList = list.ToList();
        for (int i = newList.Count - 1; i >= 0; i--)
        {
            AttributeListSyntax? methodAttribute = newList[i];
            if (methodAttribute.Attributes.Any(e => e.Name.ToString() == attributeName))
                newList.Remove(methodAttribute);
        }

        return SF.List(newList);
    }

    private static bool IsRefableProperty(TypeSyntax type)
    {
        string typeName = type.GetText().ToString().TrimEnd();
        if (TypeInfo.KnownEnums.Contains(typeName) || TypeInfo.BasicRefableTypes.Contains(typeName))
            return true;

        return false;
    }

    private static string GetStructPtrVectorNameForContext(ImGuiStructTypeKind kind)
    {
        if (kind == ImGuiStructTypeKind.ForNativeStruct)
            return "ImStructPtrVector";
        else if (kind == ImGuiStructTypeKind.ForInterface || kind == ImGuiStructTypeKind.ForImpl)
            return "IImStructPtrVectorWrapper";

        throw new InvalidDataException();
    }

    private static string GetStructVectorNameForContext(ImGuiStructTypeKind kind)
    {
        if (kind == ImGuiStructTypeKind.ForNativeStruct)
            return "ImVector";
        else if (kind == ImGuiStructTypeKind.ForInterface || kind == ImGuiStructTypeKind.ForImpl)
            return "IImVectorWrapper";

        throw new InvalidDataException();
    }

    private static FunctionPointerTypeSyntax FixupFunctionPointer(TypeSyntax type)
    {
        FunctionPointerTypeSyntax functionPointerType = (FunctionPointerTypeSyntax)type;
        var funcParams = new List<FunctionPointerParameterSyntax>();
        for (int i = 0; i < functionPointerType.ParameterList.Parameters.Count; i++)
        {
            FunctionPointerParameterSyntax? functionPointerParam = functionPointerType.ParameterList.Parameters[i];
            TypeSyntax? actualType = TranslateTypeNameIntoCSharpType(functionPointerParam.Type.GetText().ToString(), null, doNotRemapStrings: false);

            // If the parameter is a pointer, we convert it to nint.
            if (functionPointerParam.Type.IsKind(SyntaxKind.PointerType))
            {
                funcParams.Add(functionPointerParam.WithType(actualType ?? SF.ParseTypeName("nint")));

            }
            else
                funcParams.Add(functionPointerParam.WithType(actualType ?? functionPointerParam.Type));
        }

        return functionPointerType.WithParameterList(SF.FunctionPointerParameterList(SF.SeparatedList(funcParams)));
    }

    private static bool TryGetNativeTypeArrayLength(string str, [NotNullWhen(true)] out string? name, out int length)
    {
        name = null;
        length = 0;

        string[] spl = str.Split('[');
        if (spl.Length != 2)
            return false;

        if (spl[1].Length <= 1)
            return false;

        if (!int.TryParse(spl[1].Substring(0, spl[1].Length - 1), out length))
            return false;

        name = spl[0];
        return true;
    }

    private static string TrimTypeName(string typeName)
    {
        if (typeName.EndsWith("_t"))
            typeName = typeName[..^2];

        return typeName;
    }

    private static TypeSyntax? TranslateTypeNameIntoCSharpType(string currentParamType, string? nativeTypeName, bool doNotRemapStrings, bool castEnumsToInt = false)
    {
        if (currentParamType == "byte" && nativeTypeName == "bool") // Remap byte to bool
        {
            return SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword));
        }
        else if (TypeInfo.TryGetWellknownType(currentParamType, out string? wellKnownTypeName))
        {
            return SF.ParseTypeName(wellKnownTypeName);
        }
        else if (!doNotRemapStrings && nativeTypeName == "const char *" && currentParamType == "sbyte*") // Remap strings.
        {
            return SF.PredefinedType(SF.Token(SyntaxKind.StringKeyword));
        }
        else
        {
            if (nativeTypeName is not null)
            {
                // Remap enums.
                string? match = TypeInfo.KnownEnums.FirstOrDefault(e => e == nativeTypeName);
                if (match is not null)
                {
                    if (castEnumsToInt)
                        return SF.ParseTypeName("int");
                    else
                        return SF.ParseTypeName(match);
                }
            }
        }

        return null;
    }

    private static string GetCorrectedType(string name)
    {
        return TypeInfo.TryGetWellknownType(name, out string? correctedType) ? correctedType : name;
    }

    private static string? GetNativeTypeName(SyntaxList<AttributeListSyntax> attributeLists)
    {
        foreach (AttributeListSyntax attrList in attributeLists)
        {
            foreach (AttributeSyntax attr in attrList.Attributes)
            {
                var identifier = ((IdentifierNameSyntax)attr.Name).Identifier;
                if (identifier.ValueText == "NativeTypeName")
                {
                    if (attr.ArgumentList!.Arguments.Count < 1)
                        continue;

                    if (attr.ArgumentList.Arguments[0].Expression is not LiteralExpressionSyntax arg)
                        continue;

                    return arg.Token.ValueText.Trim();
                }
            }
        }

        return null;
    }

    private static string? GetEntryPoint(MethodDeclarationSyntax method)
    {
        foreach (AttributeListSyntax attrList in method.AttributeLists)
        {
            foreach (AttributeSyntax attr in attrList.Attributes)
            {
                var identifier = ((IdentifierNameSyntax)attr.Name).Identifier;
                if (identifier.ValueText == "DllImport")
                {
                    foreach (AttributeArgumentSyntax argument in attr.ArgumentList.Arguments)
                    {
                        if (argument.NameEquals?.Name?.Identifier.Text == "EntryPoint")
                        {
                            LiteralExpressionSyntax literalExpr = (LiteralExpressionSyntax)argument.Expression;
                            return literalExpr.Token.ValueText;
                        }
                    }
                }
            }
        }

        return method.Identifier.ValueText;
    }

    private enum ImGuiStructTypeKind
    {
        /// <summary>
        /// Creating implementation struct for interface.
        /// </summary>
        ForImpl,

        /// <summary>
        /// Creating interface struct.
        /// </summary>
        ForInterface,

        /// <summary>
        /// Creating native struct for native bindings.
        /// </summary>
        ForNativeStruct,
    }
}