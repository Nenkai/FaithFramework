using FF16Framework.ImGui.Hooks;
using FF16Framework.ImGui.Hooks.DirectX12;
using FF16Framework.Interfaces.ImGuiManager;

using Reloaded.Mod.Interfaces;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.ImGuiManager;

public class ImGuiTextureManager : IImGuiTextureManager
{
    private IImguiHook _imguiHook;
    private ILogger _logger;

    public ImGuiTextureManager(ILogger logger, IImguiHook hook)
    {
        _imguiHook = hook;
        _logger = logger;
    }

    public IQueuedImGuiImage QueueImageLoad(string filePath, CancellationToken ct = default)
    {
        ImGuiImageState res = new ImGuiImageState();
        _ = Task.Run(async () =>
        {
            Image<Rgba32>? image = null;
            try
            {
                image = await Image.LoadAsync<Rgba32>(filePath, ct);
                ct.ThrowIfCancellationRequested();

                IImGuiImage imGuiImage = LoadImageBytes(image);
                res.Image = imGuiImage;
                res.IsLoaded = true;
            }
            catch (OperationCanceledException)
            {
                // Pass
            }
            catch (Exception ex)
            {
                _logger.WriteLine($"Failed to load queued image - {ex.Message}");
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
        Image<Rgba32>? image = null;
        try
        {
            image = Image.Load<Rgba32>(filePath);
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
