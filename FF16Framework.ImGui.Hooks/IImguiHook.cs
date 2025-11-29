using System;

namespace FF16Framework.ImGui.Hooks;

public interface IImguiHook : IDisposable
{
    /// <summary>
    /// True if the API is supported for the current process.
    /// </summary>
    bool IsApiSupported();

    /// <summary>
    /// Initializes the hooks specific to this graphics API.
    /// </summary>
    void InitAndEnableHooks();

    /// <summary>
    /// Disables the hooks used by this implementation.
    /// </summary>
    void DisableHooks();

    /// <summary>
    /// Re-enables the hooks used by this implementation.
    /// </summary>
    void EnableHooks();

    /// <summary>
    /// Loads an image.
    /// </summary>
    /// <param name="textureData"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    ulong LoadTexture(Span<byte> textureData, uint width, uint height);

    /// <summary>
    /// Returns whether a texture is loaded.
    /// </summary>
    bool IsTextureLoaded(ulong texId);

    /// <summary>
    /// Frees an image.
    /// </summary>
    void FreeTexture(ulong texId);
}
