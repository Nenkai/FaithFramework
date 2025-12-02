using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static ImGuiInterfaceGenerator.Program;

namespace ImGuiInterfaceGenerator;

public class Program
{
    private static readonly List<FileDefinition> _files =
    [
        new ("ImGuiBindings.cs", "dcimgui.json", false),
        new ("ImGuiBindingsFreeType.cs", "misc/freetype/dcimgui_freetype.json", false),
        new ("ImGuiBindingsDx9.cs", "backends/dcimgui_impl_dx9.json", true),
        new ("ImGuiBindingsDx11.cs", "backends/dcimgui_impl_dx11.json", true),
        new ("ImGuiBindingsDx12.cs", "backends/dcimgui_impl_dx12.json", true),
        new ("ImGuiBindingsWin32.cs", "backends/dcimgui_impl_win32.json", true)
    ];

    public record FileDefinition(string FileName, string MetadataFileName, bool ShouldNotGenerateInterface);

    public static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: <path to 'generated' folder>");
            return;
        }

        var gen = new ImGuiInterfaceGenerator();

        // Massages bindings for all files into three files:
        // - Interface (for reloaded)
        // - Implementation (for framework)
        // - Native bindings (for framework)

        foreach (var file in _files)
        {
            Console.WriteLine($"Processing {file}...");
            bool shouldNotGenerateInterface = file.ShouldNotGenerateInterface;

            var bindingsSyntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(args[0], file.FileName)), new CSharpParseOptions(kind: SourceCodeKind.Script));
            DearBindingsMetadata? metadata = DearBindingsMetadata.Parse(Path.Combine(args[0], file.MetadataFileName))
                ?? throw new InvalidOperationException($"Failed to parse dear bindings metadata file: {file.MetadataFileName}");

            gen.SetMetadata(metadata);
            if (!shouldNotGenerateInterface) // We don't care to create an interface for backends.
            {
                gen.ExtractInterfaceFromSyntaxTree(bindingsSyntaxTree);
                gen.ExtractImplFromSyntaxTree(bindingsSyntaxTree);
            }
            gen.ExtractBindingsFromSyntaxTree(bindingsSyntaxTree, shouldNotGenerateInterface);
        }

        Console.WriteLine("Finalizing interface..");
        string interfaceSource = gen.FinishInterface("FF16Framework.Interfaces.ImGui");

        Console.WriteLine("Finalizing impl..");
        string implSource = gen.FinishImpl("FF16Framework.ImGuiManager", "FF16Framework.Interfaces.ImGui");

        Console.WriteLine("Finalizing bindings..");
        string bindings = gen.FinishBindings("FF16Framework.Native.ImGui");

        Directory.CreateDirectory("generated/FF16Framework.Interfaces/ImGui");
        File.WriteAllText("generated/FF16Framework.Interfaces/ImGui/IImGui.cs", interfaceSource);

        Directory.CreateDirectory("generated/FF16Framework/ImGuiManager");
        File.WriteAllText("generated/FF16Framework/ImGuiManager/ImGui.cs", implSource);

        Directory.CreateDirectory("generated/FF16Framework.Native/ImGui");
        File.WriteAllText("generated/FF16Framework.Native/ImGui/ImGuiMethods.cs", bindings);

        Console.WriteLine($"Bindings saved to 'generated' folder.");

    }
}

