using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using static NenTools.ImGui.Generator.Program;

namespace NenTools.ImGui.Generator;

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

        string methodsNamespace = "NenTools.ImGui.Native";
        string interfacesNamespace = "NenTools.ImGui.Interfaces";
        string implNamespace = "NenTools.ImGui.Implementation";
        var gen = new ImGuiInterfaceGenerator(implNamespace, methodsNamespace, interfacesNamespace);

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
        string interfaceSource = gen.FinishInterface();

        Console.WriteLine("Finalizing impl..");
        string implSource = gen.FinishImpl();

        Console.WriteLine("Finalizing bindings..");
        string bindings = gen.FinishBindings();

        Directory.CreateDirectory($"generated/{interfacesNamespace}");
        File.WriteAllText($"generated/{interfacesNamespace}/IImGui.cs", interfaceSource);

        Directory.CreateDirectory($"generated/{implNamespace}");
        File.WriteAllText($"generated/{implNamespace}/ImGui.cs", implSource);

        Directory.CreateDirectory($"generated/{methodsNamespace}");
        File.WriteAllText($"generated/{methodsNamespace}/ImGuiMethods.cs", bindings);

        Console.WriteLine($"Bindings saved to 'generated' folder.");

    }
}

