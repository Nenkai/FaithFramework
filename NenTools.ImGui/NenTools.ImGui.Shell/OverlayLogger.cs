using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Shell.Interfaces;
using NenTools.ImGui.Interfaces;

namespace NenTools.ImGui.Shell;

public unsafe class OverlayLogger : IImGuiComponent
{
    // Inspired by xenomods
    public TimeSpan LINE_LIFETIME => TimeSpan.FromSeconds(_config.OverlayLogger.FadeTimeSeconds);
    public TimeSpan TOAST_LIFETIME = TimeSpan.FromSeconds(2.0f);
    public TimeSpan FADEOUT_START = TimeSpan.FromSeconds(0.5f);

    private readonly List<LoggerMessage> lines = [];

    public bool IsOverlay => true;

    private bool _open = true;

    private ImGuiConfig _config;
    private readonly IImGui _imGui;
    public OverlayLogger(IImGui imGui, ImGuiConfig config)
    {
        _imGui = imGui;
        _config = config;
    }

    public void AddMessage(string source, string message, Color? messageColor = null)
    {
        if (lines.Count >= _config.OverlayLogger.MaxLinesField)
            lines.Remove(lines[0]);

        var now = DateTimeOffset.UtcNow;
        lines.Add(new LoggerMessage()
        {
            Source = source,
            Text = message,
            Date = now,
            EndsAt = now + LINE_LIFETIME,
            Color = messageColor ?? Color.White,
            // Logger::LINE_LIFETIME
        });
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {

    }

    public void Render(IImGuiShell imGuiShell)
    {
        if (!_config.OverlayLogger.Enabled)
            return;

        float barHeight = 0;
        if (imGuiShell.IsMainMenuOpen)
            barHeight += _imGui.GetFrameHeight();

        _imGui.SetNextWindowSize(new Vector2(_imGui.GetIO().DisplaySize.X, _imGui.GetIO().DisplaySize.Y - barHeight), ImGuiCond.ImGuiCond_Always);

        if (_imGui.Begin("log_overlay", ref _open, ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
            ImGuiWindowFlags.ImGuiWindowFlags_NoDocking |
            ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |
            ImGuiWindowFlags.ImGuiWindowFlags_NoSavedSettings |
            ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing |
            ImGuiWindowFlags.ImGuiWindowFlags_NoNav |
            ImGuiWindowFlags.ImGuiWindowFlags_NoInputs |
            ImGuiWindowFlags.ImGuiWindowFlags_NoBackground))
        {
            _imGui.SetWindowPos(new Vector2(0, barHeight), ImGuiCond.ImGuiCond_Always);

            for (int i = 0; i < lines.Count; ++i)
            {
                var msg = lines[i];

                // check lifetime greater than 0, but also decrement it for next time
                if (msg.Lifetime > TimeSpan.Zero)
                    DrawInternal(_imGui, msg, 10, (ushort)(barHeight + 5 + i * 16));
                else if (lines.Count != 0)
                    // erase the current index but decrement i so we try again with the next one
                    lines.Remove(lines[i--]);

            }
        }

        _imGui.End();
    }

    void DrawInternal(IImGui imgui, LoggerMessage msg, ushort x, ushort y)
    {
        float alpha = 1.0f;
        if (msg.Lifetime <= TimeSpan.Zero)
        {
            alpha = 0.0f;
        }
        else if (msg.Lifetime <= FADEOUT_START)
        {
            // make the text fade out before it gets removed
            alpha = (float)(msg.Lifetime / FADEOUT_START);
        }

        imgui.SetCursorScreenPos(new Vector2(x, y));
        imgui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, alpha), $"[{msg.Source}]"); imgui.SameLineEx(0, 2);
        imgui.TextColored(new Vector4(1.0f, 1.0f, 1.0f, alpha), $"{msg.Date:HH:mm:ss.fff} - "); imgui.SameLineEx(0, 2);
        imgui.TextColored(new Vector4(msg.Color.R / 255f, msg.Color.G / 255f, msg.Color.B / 255f, alpha), msg.Text);
	}
}

public class LoggerMessage
{
    public string Source { get; set; }
    public string Text { get; set; }
    public Color Color { get; set; } = Color.White;
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset EndsAt { get; set; }
    public TimeSpan Lifetime => EndsAt - DateTimeOffset.UtcNow;
};
