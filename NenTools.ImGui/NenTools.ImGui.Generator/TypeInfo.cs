using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NenTools.ImGui.Generator;

public class TypeInfo
{
    public static readonly Dictionary<string, string> WellKnownTypes = new()
    {
        { "unsigned char", "byte" },
        { "signed char", "sbyte" },
        { "char", "byte" },
        { "ImWchar", "uint" },
        { "ImFileHandle", "nint" },
        { "ImU8", "byte" },
        { "ImS8", "sbyte" },
        { "ImU16", "ushort" },
        { "ImS16", "short" },
        { "ImU32", "uint" },
        { "ImS32", "int" },
        { "ImU64", "ulong" },
        { "ImS64", "long" },
        { "unsigned short", "ushort" },
        { "unsigned int", "uint" },
        { "ImVec2", "Vector2" },
        { "ImVec2_Simple", "Vector2" },
        { "ImVec3", "Vector3" },
        { "ImVec4", "Vector4" },
        { "ImColor", "Vector4" }, // Marked as may obsolete, and should not be stored/used for convenience of converting colors between rgb and hsv. "(*OBSOLETE* please avoid using)"
        { "ImWchar16", "ushort" }, //char is not blittable
        { "ImVec4_Simple", "Vector4" },
        { "ImColor_Simple", "ImColor" },
        { "ImTextureID", "ulong" },
        { "ImGuiID", "uint" },
        { "ImDrawIdx", "ushort" },
        { "ImDrawListSharedData", "nint" },
        { "ImDrawListSharedData*", "nint" },
        { "ImDrawCallback", "nint" },
        { "size_t", "uint" },
        { "ImGuiContext*", "nint" },
        { "ImPlotContext*", "nint" },
        { "EditorContext*", "nint" },
        { "ImGuiMemAllocFunc", "nint" },
        { "ImGuiMemFreeFunc", "nint" },
        { "ImFontBuilderIO", "nint" },
        { "float[2]", "Vector2*" },
        { "float[3]", "Vector3*" },
        { "float[4]", "Vector4*" },
        { "int[2]", "int*" },
        { "int[3]", "int*" },
        { "int[4]", "int*" },
        { "float&", "float*" },
        { "ImVec2[2]", "Vector2*" },
        { "char* []", "byte**" },
        { "unsigned char[256]", "byte*"},
        { "ImPlotFormatter", "nint" },
        { "ImPlotGetter", "nint" },
        { "ImPlotTransform", "nint" },
        { "ImGuiKeyChord", "ImGuiKey" },
        { "ImGuiSelectionUserData", "long" },
    };

    /// <summary>
    /// List of pointer types that should be passed by reference between function calls. This allows ref usage instead of pointers.
    /// </summary>
    public static readonly HashSet<string> CallArgumentPointerTypesThatShouldPassByReference =
    [
        "int*",
        "uint*",
        "float*",
        "double*",
        "ushort*",
        "short*",
        "long*",
        "ulong*",
        "Vector2*",
        "Vector4*",
        "ImTextureRef*",
    ];

    /// <summary>
    /// Identifiers known to be enums for remapping integers to enums.
    /// </summary>
    public static readonly HashSet<string> KnownEnums =
    [
        "ImGuiWindowFlags",
        "ImGuiChildFlags",
        "ImGuiItemFlags",
        "ImGuiInputTextFlags",
        "ImGuiTreeNodeFlags",
        "ImGuiPopupFlags",
        "ImGuiSelectableFlags",
        "ImGuiComboFlags",
        "ImGuiTabBarFlags",
        "ImGuiTabItemFlags",
        "ImGuiFocusedFlags",
        "ImGuiHoveredFlags",
        "ImGuiDockNodeFlags",
        "ImGuiDragDropFlags",
        "ImGuiDataType",
        "ImGuiInputFlags",
        "ImGuiConfigFlags",
        "ImGuiBackendFlags",
        "ImGuiCol",
        "ImGuiStyleVar",
        "ImGuiButtonFlags",
        "ImGuiColorEditFlags",
        "ImGuiSliderFlags",
        "ImGuiMouseButton",
        "ImGuiMouseCursor",
        "ImGuiCond",
        "ImGuiTableFlags",
        "ImGuiTableColumnFlags",
        "ImGuiTableRowFlags",
        "ImGuiTableBgTarget",
        "ImGuiMultiSelectFlags",
        "ImGuiSelectionRequestType",
        "ImDrawFlags",
        "ImDrawListFlags",
        "ImFontAtlasFlags",
        "ImGuiViewportFlags",
        "ImGuiFreeTypeBuilderFlags",
        "ImTextureFormat",
        "ImTextureStatus",
    ];

    /// <summary>
    /// Basic types that can be ref'ed (for assigning the ref keyword for implementation/interface properties)
    /// </summary>
    public static readonly HashSet<string> BasicRefableTypes =
    [
        "bool",
        "byte",
        "sbyte",
        "int",
        "uint",
        "float",
        "double",
        "ushort",
        "short",
        "long",
        "ulong",
        nameof(Vector2),
        nameof(Vector4),
    ];

    public static readonly HashSet<string> CSharpNoPointerIdentifiers =
    [
        "nint",
        "nuint",
        nameof(Vector2),
        nameof(Vector4),
    ];

    /// <summary>
    /// Type identifiers to skip declaring as we declare ourselves manually
    /// </summary>
    public static readonly HashSet<string> SkipDeclareIdentifiers =
    [
        "ImVec2", // We map to Vector2
        "ImVec4", // We map to Vector4
        "ImGuiStoragePair", // We map manually, as this is an union
        "ImColor",
        "ImTextureRef",
    ];

    /// <summary>
    /// Types that are passed by values that we declare manually
    /// </summary>
    public static readonly HashSet<string> ManuallyDeclaredValueTypesAsInterfaces =
    [
        "ImTextureRef"
    ];

    public static bool TryGetWellknownType(string name, [NotNullWhen(true)] out string? wellknownTypeName)
    {
        if (name.EndsWith("_t"))
            name = name[..^2]; // Remove the _t

        return WellKnownTypes.TryGetValue(name, out wellknownTypeName);
    }
}
