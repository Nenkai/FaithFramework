using DoomNetFrameworkEngine;
using DoomNetFrameworkEngine.DoomEntity;
using DoomNetFrameworkEngine.DoomEntity.Game;
using DoomNetFrameworkEngine.DoomEntity.MathUtils;
using DoomNetFrameworkEngine.Video;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Interfaces.Shell;
using NenTools.ImGui.Interfaces.Shell.Textures;

using System;
using System.Buffers;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FaithFramework.Sample.Doom;

public static class ExtensionMethods
{
    public static Vector4 ToV4(this Color color) => new(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
}

[ImGuiMenu(Category = "Mods", Priority = 0, Owner = "FaithFramework.Sample.Doom")]
public class DoomGameComponent : IImGuiComponent
{
    private readonly IImGui imGui;
    private readonly FaithFramework.Sample.Doom.Configuration.Config config;

    public bool IsOverlay => false;
    public bool WindowOpen = true;

    // Our output image
    public IImGuiImage _viewportImage { get; set; }
    public bool BlockInput;

    // Doom state
    private UserInput _input;
    private DoomNetFrameworkEngine.DoomEntity.Doom _doom;
    private Renderer _renderer;
    private DoomState _lastState = DoomState.None;
    private byte[] _screenBuffer;
    private byte[] _normalizedScreenBuffer;
    public string WadPath { get; set; }
    private float _zoomScale = 1.0f;
    private Stopwatch _frameTimer;

    public const int FrameRate = 35;

    public DoomGameComponent(IImGui imGui, FaithFramework.Sample.Doom.Configuration.Config config)
    {
        this.imGui = imGui;
        this.config = config;
        WadPath = config.WadPath;
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (imGui.MenuItem("Doom"u8))
        {
            WindowOpen = true;
        }
    }

    public void Render(IImGuiShell imGuiShell)
    {
        if (WindowOpen)
        {
            var result = imGui.Begin("Doom"u8, ref WindowOpen, ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize);
            if (!result)
            {
                imGui.End();
                return;
            }

            imGui.Checkbox("Block Input"u8, ref BlockInput);

            if (_doom is null && string.IsNullOrWhiteSpace(WadPath))
            {
                imGui.Text("Set WADPath in mod config first..."u8);
                imGui.End();
                return;
            }

            if (_doom is null && !string.IsNullOrWhiteSpace(WadPath))
            {
                if (imGui.Button("Start"u8))
                {
                    if (!File.Exists(WadPath))
                    {
                        imGuiShell.LogWriteLine("FaithFramework.Sample.Doom", $"WAD file not found in {WadPath}! " +
                            $"(Change path in 'Configure' after right-clicking on the mod in Reloaded-II)", outputTargetFlags: LoggerOutputTargetFlags.OverlayLogger);
                    }
                    else
                    {
                        SetupDoom();
                        _doom!.NewGame(GameSkill.Medium, 0, 0);
                    }
                }
            }

            if (_doom is not null)
            {
                if (imGui.Button("New Game"u8))
                    _doom.NewGame(GameSkill.Medium, 0, 0);
                imGui.SameLine();
                if (imGui.Button("Close"u8))
                    WindowOpen = false;
                imGui.SliderFloat("Scale"u8, ref _zoomScale, 1.0f, 3.0f);

                imGui.Text($"State: {_doom.State} ({_doom.Game.GameTic}) {(_doom.Game.Paused ? "Paused" : "")}");
                if (_doom.State != _lastState)
                {
                    imGuiShell.LogWriteLine("Doom", $"State changed from: {_lastState} to {_doom.State}");
                    _lastState = _doom.State;
                }

                RenderDoomViewport(imGuiShell);
            }

            imGui.End();
        }
    }

    private void SetupDoom()
    {
        var config = new DoomNetFrameworkEngine.Config();
        config.video_highresolution = true;
        var argsList = new[] { "-iwad", WadPath };
        var cmdArgs = new CommandLineArgs(argsList);
        var content = new GameContent(cmdArgs);
        _input = new UserInput(config, e => _doom?.PostEvent(e));
        _doom = new DoomNetFrameworkEngine.DoomEntity.Doom(cmdArgs, config, content, null, null, null, _input);
        _renderer = new Renderer(config, content);
        //config.video_fpsscale = 1;
        int width = _renderer.Width;
        int height = _renderer.Height;
        _screenBuffer = new byte[4 * width * height];
        _normalizedScreenBuffer = new byte[4 * width * height];

        _frameTimer = Stopwatch.StartNew();
    }

    private void RenderDoomViewport(IImGuiShell imGuiShell)
    {
        if (_frameTimer.ElapsedMilliseconds >= (1000 / FrameRate))
        {
            _doom.Update();
            _renderer.Render(_doom, _screenBuffer, Fixed.Zero);
            _frameTimer.Restart();
        }

        if (_doom.Game.World != null)
        {
            imGui.Text($"Health: {_doom.Game.World.ConsolePlayer.Health}, Ammo: {string.Join(", ", _doom.Game.World.ConsolePlayer.Ammo.Select(v => v.ToString()))}");

            int targetWidth = _renderer.Width;
            int targetHeight = _renderer.Height;

            Span<uint> inPixels = MemoryMarshal.Cast<byte, uint>(_screenBuffer);
            Span<uint> outPixels = MemoryMarshal.Cast<byte, uint>(_normalizedScreenBuffer);

            for (int y = 0; y < targetHeight; y++)
            {
                int outputRowStart = y * targetWidth;
                for (int x = 0; x < targetWidth; x++)
                {
                    int inputIndex = (x * targetHeight + y);
                    int outputIndex = outputRowStart + x;

                    outPixels[outputIndex] = inPixels[inputIndex];
                }
            }

            if (_viewportImage is null)
                _viewportImage = imGuiShell.TextureManager.LoadImage(_normalizedScreenBuffer, (uint)_renderer.Width, (uint)_renderer.Height);
            else
                imGuiShell.TextureManager.UpdateImage(_viewportImage, _normalizedScreenBuffer);

            imGui.Image(imGui.CreateTextureRef(_viewportImage.TexId), new Vector2((uint)_renderer.Width, (uint)_renderer.Height) * _zoomScale);
        }
    }
}
