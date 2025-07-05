using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.ImGuiManager.Windows;
using FF16Framework.Interfaces.ImGui;

namespace FF16Framework.ImGuiManager;

public unsafe class OverlayLogger : IImguiWindow
{
    // Inspired by xenomods
    #region Properties
    public TimeSpan LINE_LIFETIME = TimeSpan.FromSeconds(8.0f);
    public TimeSpan TOAST_LIFETIME = TimeSpan.FromSeconds(2.0f);
    public TimeSpan FADEOUT_START = TimeSpan.FromSeconds(0.5f);
    public const int MAX_LINES = 20;

    private readonly List<LoggerMessage> lines = [];

    public bool IsOverlay => true;

    private bool _open = true;

    private static OverlayLogger _instance = new OverlayLogger();
    public static OverlayLogger Instance => _instance;
    #endregion

    public void AddMessage(string message, Color? messageColor = null)
    {
        if (lines.Count >= MAX_LINES)
            lines.Remove(lines[0]);

        var now = DateTimeOffset.UtcNow;
        lines.Add(new LoggerMessage()
        {
            Text = message,
            Date = now,
            EndsAt = now + LINE_LIFETIME,
            Color = messageColor ?? Color.White,
            // Logger::LINE_LIFETIME
        });
    }

    public void Render(IImguiSupport imguiSupport, IImGui imgui)
    {
        float barHeight = 0;
        if (imguiSupport.IsMainMenuBarOpen)
            barHeight += imgui.GetFrameHeight();

        imgui.SetNextWindowSize(new Vector2(imgui.GetIO().DisplaySize.X, imgui.GetIO().DisplaySize.Y - barHeight), ImGuiCond.ImGuiCond_Always);

        if (imgui.Begin("log_overlay", ref _open, ImGuiWindowFlags.ImGuiWindowFlags_NoDecoration |
            ImGuiWindowFlags.ImGuiWindowFlags_NoDocking |
            ImGuiWindowFlags.ImGuiWindowFlags_AlwaysAutoResize |
            ImGuiWindowFlags.ImGuiWindowFlags_NoSavedSettings |
            ImGuiWindowFlags.ImGuiWindowFlags_NoFocusOnAppearing |
            ImGuiWindowFlags.ImGuiWindowFlags_NoNav |
            ImGuiWindowFlags.ImGuiWindowFlags_NoInputs |
            ImGuiWindowFlags.ImGuiWindowFlags_NoBackground))
        {
            imgui.SetWindowPos(new Vector2(0, barHeight), ImGuiCond.ImGuiCond_Always);

            for (int i = 0; i < lines.Count; ++i)
            {
                var msg = lines[i];

                // check lifetime greater than 0, but also decrement it for next time
                if (msg.Lifetime > TimeSpan.Zero)
                    DrawInternal(imgui, msg, 10, (ushort)(barHeight + 5 + i * 16));
                else if (lines.Count != 0)
                    // erase the current index but decrement i so we try again with the next one
                    lines.Remove(lines[i--]);

            }

            imgui.End();
        }
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
        imgui.TextColored(new Vector4(0.5f, 0.5f, 0.5f, alpha), $"{msg.Date:HH:mm:ss.fff} -"); imgui.SameLine();
        imgui.TextColored(new Vector4(msg.Color.R / 255f, msg.Color.G / 255f, msg.Color.B / 255f, alpha), msg.Text);
	}


    public void BeginMenuComponent(IImGui imgui)
    {

    }
}

public class LoggerMessage
{
    public string Text { get; set; }
    public Color Color { get; set; } = Color.White;
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset EndsAt { get; set; }
    public TimeSpan Lifetime => EndsAt - DateTimeOffset.UtcNow;
};
