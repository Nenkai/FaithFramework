using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ImGuiInterfaceGenerator;

public class SyntaxTreeUtils
{
    /// <summary>
    /// Creates <code>[MarshalAs(UnmanagedType.{type})]</code>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static SyntaxList<AttributeListSyntax> GetMarshalAsAttribute(UnmanagedType type)
    {
        // Oh god. All that just to write [MarshalAs(UnmanagedType.LPUTF8Str)]
        // Thank you https://roslynquoter.azurewebsites.net/
        var attributeArguments = SF.ParseAttributeArgumentList($"({nameof(UnmanagedType)}.{type})");
        return SF.SingletonList<AttributeListSyntax>(
                       SF.AttributeList(
                           SF.SingletonSeparatedList<AttributeSyntax>(
                               SF.Attribute(
                                   SF.IdentifierName("MarshalAs"))
                                   .WithArgumentList(attributeArguments))));
    }

    public static AttributeListSyntax GetReturnMarshalAsAttribute(UnmanagedType type)
    {
        return SF.AttributeList(
                     SF.SingletonSeparatedList<AttributeSyntax>(
                         SF.Attribute(
                             SF.IdentifierName("MarshalAs"))
                         .WithArgumentList(
                             SF.AttributeArgumentList(
                                 SF.SingletonSeparatedList<AttributeArgumentSyntax>(
                                     SF.AttributeArgument(
                                         SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SF.IdentifierName("UnmanagedType"), SF.IdentifierName($"{type}"))))))))
                 .WithTarget(SF.AttributeTargetSpecifier(SF.Token(SyntaxKind.ReturnKeyword)));
    }

    /// <summary>
    /// Creates { get; set; }, with arrow bodies if provided
    /// </summary>
    /// <param name="withGetter"></param>
    /// <param name="getterExpression"></param>
    /// <param name="withSetter"></param>
    /// <param name="setterExpression"></param>
    /// <returns></returns>
    public static AccessorListSyntax? GetAccessorList(
        bool withGetter = true, ExpressionSyntax? getterExpression = null, 
        bool withSetter = true, ExpressionSyntax? setterExpression = null)
    {
        List<AccessorDeclarationSyntax> list = [];

        if (withGetter)
        {
            list.Add(SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration, [], [], expressionBody: getterExpression is not null ? SF.ArrowExpressionClause(getterExpression) : null!)
                .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)));
        }

        if (withSetter)
        {
            list.Add(SF.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration, [], [], expressionBody: setterExpression is not null ? SF.ArrowExpressionClause(setterExpression) : null!)
                .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)));
        }

        if (list.Count == 0)
            return null;

        return SF.AccessorList(SF.List<AccessorDeclarationSyntax>(list));
    }

    /// <summary>
    /// Creates <code>ref Unsafe.AsRef&lt;typeName&gt;(&amp;((nativeStructName*)sourceProperty)-&gt;targetProperty)</code>
    /// </summary>
    /// <param name="typeName"></param>
    /// <param name="sourceProperty"></param>
    /// <param name="targetProperty"></param>
    /// <returns></returns>
    public static ExpressionSyntax CreateAsRefExpression(string typeName, string nativeStructTypeName, string sourceProperty, string targetProperty)
    {
        return SF.ParseExpression($"ref Unsafe.AsRef<{typeName}>(&(({nativeStructTypeName}*){sourceProperty})->{targetProperty})");
    }

    /// <summary>
    /// Creates  ((nativeStructTypeName*)SourceProperty)-&gt;TargetProperty</code> 
    /// </summary>
    /// <param name="sourcePropertyName"></param>
    /// <param name="targetPropertyName"></param>
    /// <returns></returns>
    public static ExpressionSyntax CreatePointerAccessExpressionForGetter(string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}");
    }

    public static ExpressionSyntax CreateRefToPointerConstructorCall(string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"ref (({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}");
    }

    /// <summary>
    /// Creates <code>=&gt; ((nativeStructTypeName*)SourceProperty)-&gt;TargetProperty = value</code> 
    /// </summary>
    /// <param name="sourcePropertyName"></param>
    /// <param name="targetPropertyName"></param>
    /// <returns></returns>
    public static ExpressionSyntax CreatePointerAccessExpressionForSetter(string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName} = value");
    }

    /// <summary>
    /// Creates <code>public implTypeName(nativeStructTypeName* arg) => sourcePropertyName = (nint)arg;</code>
    /// </summary>
    /// <param name="implTypeName"></param>
    /// <param name="nativeStructTypeName"></param>
    /// <param name="sourcePropertyName"></param>
    /// <returns></returns>
    public static ConstructorDeclarationSyntax CreatePointerToImplStructConstructorDeclaration(string implTypeName, string nativeStructTypeName, string sourcePropertyName)
    {
        return SF.ConstructorDeclaration(
                    SF.Identifier(implTypeName))
                .WithModifiers(
                    SF.TokenList(
                        SF.Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    SF.ParameterList(
                        SF.SingletonSeparatedList<ParameterSyntax>(
                            SF.Parameter(
                                SF.Identifier("nativePtr"))
                            .WithType(SF.PointerType(SF.IdentifierName(nativeStructTypeName))))))
                .WithExpressionBody(
                    SF.ArrowExpressionClause(
                        SF.AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            SF.IdentifierName(sourcePropertyName),
                            SF.CastExpression(
                                SF.IdentifierName("nint"),
                                SF.IdentifierName("nativePtr")))))
                .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken));
    }

    /// <summary>
    /// Creates <code>new implTypeName(((nativeStructTypeName*)sourcePropertyName)-&gt;targetPropertyName);</code>
    /// </summary>
    /// <param name="implTypeName"></param>
    /// <param name="sourcePropertyName"></param>
    /// <param name="targetPropertyName"></param>
    /// <returns></returns>
    public static ExpressionSyntax CreatePointerToConstructorCallExpression(string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"new {implTypeName}((({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName})");
    }

    /// <summary>
    /// Creates <code>new implTypeName(&amp;((nativeStructTypeName*)sourcePropertyName)-&gt;targetPropertyName);</code>
    /// </summary>
    /// <param name="implTypeName"></param>
    /// <param name="sourcePropertyName"></param>
    /// <param name="targetPropertyName"></param>
    /// <returns></returns>
    public static ExpressionSyntax CreateStructureToConstructorCallExpression(string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"new {implTypeName}(&(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName})");
    }


    public static PropertyDeclarationSyntax CreateNativePointerMember(string name)
    {
        PropertyDeclarationSyntax nativePtr = SF.PropertyDeclaration(
                            SF.IdentifierName($"{name}"),
                            SF.Identifier("NativePointer"))
                            .WithModifiers(
                                SF.TokenList(SF.Token(SyntaxKind.PublicKeyword)))
                            .WithAccessorList(
                                SF.AccessorList(
                                SF.SingletonList<AccessorDeclarationSyntax>(
                                    SF.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                                .WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)))));

        return nativePtr;
    }

    /// <summary>
    /// Creates <code>argName is not null ? (nativeStructTypeName*)argName.targetPropertyName : null;</code>
    /// </summary>
    /// <param name="nativeStructTypeName"></param>
    /// <param name="argName"></param>
    /// <param name="targetPropertyName"></param>
    /// <returns></returns>
    public static ExpressionSyntax CreatePointerArgumentWithNullPatternExpression(string nativeStructTypeName, string argName, string targetPropertyName)
    {
        return SF.ParseExpression($"{argName} is not null ? ({nativeStructTypeName}*){argName}.{targetPropertyName} : null");
    }

    /// <summary>
    /// Creates <code>=&gt; new ImStructVectorWrapper&lt;interfaceTypeName&gt;(Unsafe.Read&lt;ImVector&gt;(&amp;sourcePropertyName->targetPropertyName), Unsafe.SizeOf&lt;implTypeNameStruct&gt;(), (addr) =&gt; new implTypeName((implTypeNameStruct*)addr))));</code>
    /// </summary>
    /// <param name="interfaceTypeName"></param>
    /// <param name="implTypeName"></param>
    /// <param name="sourcePropertyName"></param>
    /// <param name="targetPropertyName"></param>
    public static ExpressionSyntax CreateImPtrVectorAccess(string interfaceTypeName, string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"new ImStructVectorWrapper<{interfaceTypeName}>(Unsafe.Read<ImVector>(&(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}), " +
            $"Unsafe.SizeOf<{implTypeName}Struct>(), " +
            $"(addr) => new {implTypeName}(({implTypeName}Struct*)addr))");
    }

    /// <summary>
    /// Creates <code>=&gt; new ImStructPtrVectorWrapper&lt;interfaceTypeName&gt;(Unsafe.Read&lt;ImVector&gt;(&amp;sourcePropertyName->targetPropertyName), (addr) =&gt; new implTypeName((implTypeNameStruct*)addr)));</code>
    /// </summary>
    /// <param name="interfaceTypeName"></param>
    /// <param name="implTypeName"></param>
    /// <param name="sourcePropertyName"></param>
    /// <param name="targetPropertyName"></param>
    public static ExpressionSyntax CreateImStructPtrVectorAccess(string interfaceTypeName, string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"new ImStructPtrVectorWrapper<{interfaceTypeName}>(" +
            $"Unsafe.Read<ImVector>(&(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}), " +
            $"(addr) => new {implTypeName}(({implTypeName}Struct*)addr)" +
        ")");
    }

    public static ExpressionSyntax CreateRangeAccessorExpression(string typeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName, int length)
    {
        return SF.ParseExpression($"new RangeAccessor<{typeName}>(&(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}, {length})");
    }


    public static ExpressionSyntax CreateRangeStructAccessorExpression(string interfaceTypeName, string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName, int length)
    {
        return SF.ParseExpression($"new RangeStructAccessor<{interfaceTypeName}>(&(({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}, " +
            $"{length}, " +
            $"Unsafe.SizeOf<{implTypeName}Struct>(), " +
            $"(addr) => new {implTypeName}(({implTypeName}Struct*)addr))");
    }

    public static ExpressionSyntax CreateImStructPtrVectorPtrAccess(string interfaceTypeName, string implTypeName, string nativeStructTypeName, string sourcePropertyName, string targetPropertyName)
    {
        return SF.ParseExpression($"new ImStructPtrVectorPtrWrapper<{interfaceTypeName}>(" +
            $"(nint)(&((({nativeStructTypeName}*){sourcePropertyName})->{targetPropertyName}))," +
            $"(addr) => new {implTypeName}(({implTypeName}Struct*)addr))");
    }


    /// <summary>
    /// Creates <code>#pragma warning disable code // comment</code>
    /// </summary>
    /// <param name="code"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    public static SyntaxTrivia CreateWarningDirectiveTrivia(string code, string comment)
    {
        return SF.Trivia(
                   SF.PragmaWarningDirectiveTrivia(
                        SF.Token(SyntaxKind.DisableKeyword),
                        isActive: true)
                    .WithErrorCodes(
                        SF.SingletonSeparatedList<ExpressionSyntax>(
                            SF.IdentifierName(
                                SF.Identifier(
                                    SF.TriviaList(),
                                    code,
                                    SF.TriviaList(
                                        SF.Comment(comment)))))));
    }
}
