using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace ImGuiInterfaceGenerator;

public class DearBindingsMetadata
{
    [JsonPropertyName("defines")]
    public List<DefineMetadata> Defines { get; set; } = [];

    [JsonPropertyName("enums")]
    public List<EnumMetadata> Enums { get; set; } = [];

    [JsonPropertyName("typedefs")]
    public List<TypedefMetadata> Typedefs { get; set; } = [];

    [JsonPropertyName("structs")]
    public List<StructMetadata> Structs { get; set; } = [];

    [JsonPropertyName("functions")]
    public List<FunctionMetadata> Functions { get; set; } = [];

    [JsonIgnore]
    public FrozenDictionary<string, DefineMetadata>? DefinesByName { get; private set; }

    [JsonIgnore]
    public FrozenDictionary<string, EnumMetadata>? EnumsByName { get; private set; }

    [JsonIgnore]
    public FrozenDictionary<string, TypedefMetadata>? TypedefsByName { get; private set; }

    [JsonIgnore]
    public FrozenDictionary<string, StructMetadata>? StructsByName { get; private set; }

    [JsonIgnore]
    public FrozenDictionary<string, FunctionMetadata>? FunctionsByName { get; private set; }

    public static DearBindingsMetadata? Parse(string file)
    {
        var deserialized = JsonSerializer.Deserialize<DearBindingsMetadata>(
            File.ReadAllText(file),
            new JsonSerializerOptions()
            {
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            });

        deserialized?.CreateLUTs();
        return deserialized;
    }

    private void CreateLUTs()
    {
        DefinesByName = Defines.DistinctBy(e => e.Name).ToFrozenDictionary(d => d.Name, d => d);
        EnumsByName = Enums.DistinctBy(e => e.Name).ToFrozenDictionary(e => e.Name, e => e);
        TypedefsByName = Typedefs.DistinctBy(e => e.Name).ToFrozenDictionary(t => t.Name, t => t);
        StructsByName = Structs.DistinctBy(e => e.Name).ToFrozenDictionary(s => s.Name, s => s);
        FunctionsByName = Functions.DistinctBy(e => e.Name).ToFrozenDictionary(f => f.Name, f => f);
    }
}

/////////////////////////////////////////////////////
/// Defines
/////////////////////////////////////////////////////
[DebuggerDisplay("Define {Name}")]
public class DefineMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("content")]
    public string? Content { get; set; }
}

/////////////////////////////////////////////////////
/// Enums
/////////////////////////////////////////////////////
[DebuggerDisplay("Enum {Name}")]
public class EnumMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("original_fully_qualified_name")]
    public required string OriginalFullyQualifiedName { get; set; }

    [JsonPropertyName("storage_type")]
    public TypedefTypeMetadata? StorageType { get; set; }

    [JsonPropertyName("is_flags_enum")]
    public bool IsFlagsEnum { get; set; }

    [JsonPropertyName("elements")]
    public List<EnumValueMetadata> Elements { get; set; } = [];
}

public class EnumValueMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("value_expression")]
    public string? ValueExpression { get; set; }

    [JsonPropertyName("value")]
    public long Value { get; set; }

    [JsonPropertyName("is_count")]
    public bool? IsCount { get; set; }
}

/////////////////////////////////////////////////////
/// Typedefs
/////////////////////////////////////////////////////
public class TypedefMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required TypedefTypeMetadata Type { get; set; }
}

/////////////////////////////////////////////////////
/// Structs
/////////////////////////////////////////////////////
[DebuggerDisplay("Struct {Name}")]
public class StructMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("original_fully_qualified_name")]
    public required string OriginalFullyQualifiedName { get; set; }

    [JsonPropertyName("kind")]
    public required string Kind { get; set; }

    [JsonPropertyName("by_value")]
    public bool ByValue { get; set; }

    [JsonPropertyName("forward_declaration")]
    public bool ForwardDeclaration { get; set; }

    [JsonPropertyName("is_anonymous")]
    public bool IsAnonymous { get; set; }

    [JsonPropertyName("fields")]
    public List<StructFieldMetadata> Fields { get; set; } = [];
}

[DebuggerDisplay("Struct Field {Name}")]
public class StructFieldMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("is_array")]
    public bool IsArray { get; set; }

    [JsonPropertyName("array_bounds")]
    public string? ArrayBounds { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("is_anonymous")]
    public bool IsAnonymous { get; set; }

    [JsonPropertyName("type")]
    public required TypedefTypeMetadata Type { get; set; }

    [JsonPropertyName("default_value")]
    public string? DefaultValue { get; set; }
}

/////////////////////////////////////////////////////
/// Functions
/////////////////////////////////////////////////////
[DebuggerDisplay("Function {Name}")]
public class FunctionMetadata : GenericMetadataBase
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("original_fully_qualified_name")]
    public required string OriginalFullyQualifiedName { get; set; }

    [JsonPropertyName("return_type")]
    public required TypedefTypeMetadata ReturnType { get; set; }

    [JsonPropertyName("arguments")]
    public required List<FunctionArgumentMetadata> Arguments { get; set; } = [];

    [JsonPropertyName("is_default_argument_helper")]
    public bool IsDefaultArgumentHelper { get; set; }

    [JsonPropertyName("is_manual_helper")]
    public bool IsManualHelper { get; set; }

    [JsonPropertyName("is_imstr_helper")]
    public bool IsImStrHelper { get; set; }

    [JsonPropertyName("has_imstr_helper")]
    public bool HasImStrHelper { get; set; }

    [JsonPropertyName("is_unformatted_helper")]
    public bool IsUnformattedHelper { get; set; }

    [JsonPropertyName("is_static")]
    public bool IsStatic { get; set; }

    [JsonPropertyName("original_class")]
    public string? OriginalClass { get; set; }
}

/////////////////////////////////////////////////////
/// Type union
/////////////////////////////////////////////////////
[DebuggerDisplay("Typedef {Declaration}")]
public class TypedefTypeMetadata
{
    [JsonPropertyName("declaration")]
    public required string Declaration { get; set; }

    [JsonPropertyName("type_details")]
    public TypedefTypeDetailsMetadata? TypeDetails { get; set; }

    [JsonPropertyName("description")]
    public InnerTypeMetadataBase? TypeDescription { get; set; }
}

public class TypedefTypeDetailsMetadata
{
    [JsonPropertyName("flavour")]
    public required string Flavor { get; set; }

    [JsonPropertyName("return_type")]
    public required TypedefTypeMetadata ReturnType { get; set; }

    [JsonPropertyName("arguments")]
    public List<FunctionArgumentMetadata> Arguments { get; set; } = [];
}

public class FunctionArgumentMetadata
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public TypedefTypeMetadata? Type { get; set; }

    [JsonPropertyName("is_array")]
    public bool IsArray { get; set; }

    [JsonPropertyName("array_bounds")]
    public string? ArrayBounds { get; set; }

    [JsonPropertyName("is_varargs")]
    public bool IsVaArgs { get; set; }

    [JsonPropertyName("is_instance_pointer")]
    public bool IsInstancePointer { get; set; }

    [JsonPropertyName("default_value")]
    public string? DefaultValue { get; set; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(TypeDescriptionMetadata), "Type")]
[JsonDerivedType(typeof(PointerDescriptionMetadata), "Pointer")]
[JsonDerivedType(typeof(UserDescriptionMetadata), "User")]
[JsonDerivedType(typeof(BuiltinDescriptionMetadata), "Builtin")]
[JsonDerivedType(typeof(FunctionDescriptionMetadata), "Function")]
[JsonDerivedType(typeof(ArrayDescriptionMetadata), "Array")]
public abstract class InnerTypeMetadataBase
{
    [JsonPropertyName("storage_classes")]
    public List<string>? StorageClasses { get; set; } = [];
}

public class TypeDescriptionMetadata : InnerTypeMetadataBase
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("inner_type")]
    public InnerTypeMetadataBase? InnerType { get; set; }

    public TypeDescriptionMetadata() { }
}

public class PointerDescriptionMetadata : InnerTypeMetadataBase
{
    [JsonPropertyName("is_nullable")]
    public bool? Kind { get; set; }

    [JsonPropertyName("is_reference")]
    public bool? IsReference { get; set; }

    [JsonPropertyName("inner_type")]
    public InnerTypeMetadataBase? InnerType { get; set; }

    public PointerDescriptionMetadata() { }
}

public class UserDescriptionMetadata : InnerTypeMetadataBase
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public UserDescriptionMetadata() { }
}

public class BuiltinDescriptionMetadata : InnerTypeMetadataBase
{
    [JsonPropertyName("builtin_type")]
    public string? BuiltinType { get; set; }

    public BuiltinDescriptionMetadata() { }
}

public class FunctionDescriptionMetadata : InnerTypeMetadataBase
{
    [JsonPropertyName("return_type")]
    public required InnerTypeMetadataBase ReturnType { get; set; }

    [JsonPropertyName("parameters")]
    public required List<InnerTypeMetadataBase> Parameters { get; set; } = [];

    public FunctionDescriptionMetadata() { }
}

public class ArrayDescriptionMetadata : InnerTypeMetadataBase
{
    [JsonPropertyName("bounds")]
    public string? Bounds { get; set; }

    [JsonPropertyName("inner_type")]
    public required InnerTypeMetadataBase InnerType { get; set; }

    public ArrayDescriptionMetadata() { }
}

/////////////////////////////////////////////////////
/// Generic Data
/////////////////////////////////////////////////////
public class ConditionalMetadata
{
    [JsonPropertyName("condition")]
    public required string Condition { get; set; }

    [JsonPropertyName("expression")]
    public required string Expression { get; set; }
}

public class CommentsMetadata
{
    [JsonPropertyName("preceding")]
    public List<string>? Preceding { get; set; } = [];

    [JsonPropertyName("attached")]
    public string? Attached { get; set; }
}

public class SourceLocationMetadata
{
    [JsonPropertyName("filename")]
    public required string FileName { get; set; }

    [JsonPropertyName("line")]
    public int? Line { get; set; }
}

public class GenericMetadataBase
{
    [JsonPropertyName("comments")]
    public CommentsMetadata? Comments { get; set; }

    [JsonPropertyName("is_internal")]
    public bool IsInternal { get; set; }

    [JsonPropertyName("source_location")]
    public SourceLocationMetadata? SourceLocation { get; set; }

    [JsonPropertyName("conditionals")]
    public List<ConditionalMetadata> Conditionals { get; set; } = [];
}