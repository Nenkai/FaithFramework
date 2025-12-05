using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using NenTools.ImGui.Hooks;
using NenTools.ImGui.Shell.Interfaces;

namespace NenTools.ImGui.Shell;

public class ImGuiTextureManager : IImGuiTextureManager
{
    private readonly IImguiHook _imguiHook;
    private readonly ILogger? _logger;

    public ImGuiTextureManager(IImguiHook hook, ILoggerFactory? loggerFactory = null)
    {
        _imguiHook = hook;
        _logger = loggerFactory?.CreateLogger<ImGuiTextureManager>();
    }

    public IQueuedImGuiImage QueueImageLoad(string filePath, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        _logger?.LogTrace("Loading {filePath}", filePath);

        ImGuiImageState res = new ImGuiImageState();
        _ = Task.Run(async () =>
        {
            Image<Rgba32>? image = null;
            try
            {
                image = await Image.LoadAsync<Rgba32>(filePath, ct);
                ct.ThrowIfCancellationRequested();

                ImGuiImage imGuiImage = LoadImageBytes(image);
                _logger?.LogTrace("Loaded {filePath} ({width}x{height})", filePath, imGuiImage.Width, imGuiImage.Height);

                res.Image = imGuiImage;
                res.IsLoaded = true;
            }
            catch (OperationCanceledException)
            {
                // Pass
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to load queued image");
            }
            finally
            {
                image?.Dispose();
            }
        }, ct);

        return res;
    }

    public IImGuiImage LoadImage(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath, nameof(filePath));

        Image<Rgba32>? image = null;
        try
        {
            image = Image.Load<Rgba32>(filePath);
            _logger?.LogTrace("Loaded {filePath} ({width}x{height})", filePath, image.Width, image.Height);

            return LoadImageBytes(image);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            image?.Dispose();
        }
    }

    private ImGuiImage LoadImageBytes(Image<Rgba32> image)
    {
        ArgumentNullException.ThrowIfNull(image, nameof(image));

        int size = image.Width * image.Height * 4;
        byte[] data = ArrayPool<byte>.Shared.Rent(size);
        image.CopyPixelDataTo(data);

        ulong texId = _imguiHook.LoadTexture(data.AsSpan(0, size), (uint)image.Width, (uint)image.Height);
        if (data is not null)
            ArrayPool<byte>.Shared.Return(data);

        return new ImGuiImage(this, texId, (uint)image.Width, (uint)image.Height);
    }

    public IImGuiImage LoadImage(Span<byte> rgba32Bytes, uint width, uint height)
    {
        if (rgba32Bytes.Length != width * height * 4)
            throw new ArgumentException("The provided bytes does not match the specified dimensions.");

        ulong texId = _imguiHook.LoadTexture(rgba32Bytes, width, height);
        return new ImGuiImage(this, texId, width, height);
    }

    public void FreeImage(IImGuiImage image)
    {
        ImGuiImage imGuiImage = (ImGuiImage)image;
        if (_imguiHook.IsTextureLoaded(image.TexId))
            _imguiHook.FreeTexture(image.TexId);

        imGuiImage.Disposed = true;
        imGuiImage.TexId = 0;
    }
}
