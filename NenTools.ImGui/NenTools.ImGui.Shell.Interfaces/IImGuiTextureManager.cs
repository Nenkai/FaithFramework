using System;
using System.Threading;
using System.Threading.Tasks;

namespace NenTools.ImGui.Shell.Interfaces;

/// <summary>
/// Texture/Image manager for ImGui.
/// </summary>
public interface IImGuiTextureManager
{
    /// <summary>
    /// Loads an image from the specified buffer. Image data is expected to be RGBA32.
    /// </summary>
    /// <param name="imageData">Image data must be RGBA32.</param>
    /// <param name="width">Image width.</param>
    /// <param name="height">Image height.</param>
    /// <returns>New image instance.</returns>
    IImGuiImage LoadImage(Span<byte> imageData, uint width, uint height);

    /// <summary>
    /// Loads an image from the specified file. This is a blocking operation and may hang the game for a few frames.
    /// </summary>
    /// <param name="filePath">File path. The file format/extension can be any format supported by <a href="https://docs.sixlabors.com/articles/imagesharp/imageformats.html"> ImageSharp.</a></param>
    /// <returns>New image instance.</returns>
    IImGuiImage LoadImage(string filePath);

    /// <summary>
    /// Queues an image for loading. This can be used to load images asynchronously.
    /// </summary>
    /// <param name="filePath">File path. The file format/extension can be any format supported by <a href="https://docs.sixlabors.com/articles/imagesharp/imageformats.html"> ImageSharp.</a></param>
    /// <param name="ct">Cancellation token for cancelling load.</param>
    /// <returns>Queued image instance.</returns>
    IQueuedImGuiImage QueueImageLoad(string filePath, CancellationToken ct = default);

    /// <summary>
    /// Frees the specified image and all associated resources. <br/>
    /// This is provided as an alternative to freeing an image. Use <see cref="IDisposable.Dispose"/> otherwise.
    /// </summary>
    /// <param name="image"></param>
    void FreeImage(IImGuiImage image);
}