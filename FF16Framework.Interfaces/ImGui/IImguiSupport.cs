using FF16Framework.Interfaces.ImGuiManager;

using System;

namespace FF16Framework.Interfaces.ImGui;

public interface IImguiSupport
{
    /// <summary>
    /// Whether ImGui can render or not. Use this in render loops to skip any calls to ImGui.
    /// </summary>
    bool CanRender { get; set; }

    /// <summary>
    /// Whether the main menu bar is open. Can be used to account for any potential positional padding.
    /// </summary>
    bool IsMainMenuBarOpen { get; }

    void AddComponent(string category, IImguiMenuComponent component);
    void AddMenuSeparator(string category);

    /// <summary>
    /// Loads an image from the specified buffer. Image data is expected to be RGBA32.
    /// </summary>
    /// <param name="imageData">Image data is expected to be RGBA32.</param>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    /// <returns></returns>
    ImGuiImage LoadImage(Span<byte> imageData, uint width, uint height);

    /// <summary>
    /// Loads an image from the specified path.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    ImGuiImage LoadImage(string filePath);

    /// <summary>
    /// Frees the specified image and all associated resources.
    /// </summary>
    /// <param name="image"></param>
    void FreeImage(ImGuiImage image);
}