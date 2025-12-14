using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Abstractions;

using Reloaded.Mod.Interfaces;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "File", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public unsafe class LogWindow : IImGuiComponent
{
    public bool IsOverlay => false;

    public bool IsOpen = false;
    public bool _autoScroll = true;

    private ILogger _logger;

    private StreamWriter? _sw;

    private const int MAX_LINES = 5000;
    public List<LogMessage> LastLines = new(MAX_LINES);
    private static object _lock = new object();

    private readonly IImGui _imGui;
    public LogWindow(IImGui imgui, ILogger logger)
    {
        _imGui = imgui;
        _logger = logger;
        _logger.OnWriteLine += _logger_OnWriteLine;
    }

    ~LogWindow()
    {
        _sw?.Flush();
    }

    public void SetupLogPath(string path)
    {
        _sw?.Dispose();
        _sw = new StreamWriter(path);
    }

    private void _logger_OnWriteLine(object sender, (string text, System.Drawing.Color color) e)
    {
        lock (_lock)
        {
            if (LastLines.Count >= MAX_LINES)
                LastLines.Remove(LastLines[0]);

            var logMsg = new LogMessage(DateTime.UtcNow, sender.ToString(), e.text);
            LastLines.Add(logMsg);
            _sw?.WriteLine(e.text);
        }
    }

    public void RenderMenu(IImGuiShell imGuiShell)
    {
        if (_imGui.MenuItemEx("Log Window"u8, ""u8, false, true))
        {
            IsOpen = true;
        }

#if DEBUG
        if (_imGui.MenuItemEx("Shutdown D3D12/ImGui", "", false, true))
        {
            imGuiShell.Shutdown();
        }
#endif
    }

    public void Render(IImGuiShell imguiSupport)
    {
        if (!IsOpen)
            return;

        if (_imGui.Begin("Log Window"u8, ref IsOpen, 0))
        {
            if (_imGui.SmallButton("Copy"u8))
                _imGui.SetClipboardText(string.Join("\n", LastLines.Select(e => e.Message)));
            _imGui.SameLineEx(0, 2);

            if (_imGui.SmallButton("Clear"u8))
                LastLines.Clear();
            _imGui.SameLineEx(0, 2);
            _imGui.Checkbox("Auto-scroll"u8, ref _autoScroll);

            _imGui.BeginChild("##log_window_container"u8, new Vector2(), 0, ImGuiWindowFlags.ImGuiWindowFlags_AlwaysVerticalScrollbar | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysHorizontalScrollbar);

            var greyColor = new Vector4(0.4f, 0.4f, 0.4f, 0.4f);
            var whiteColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            lock (_lock)
            {
                for (int i = 0; i < LastLines.Count; i++)
                {
                    _imGui.TextColored(greyColor, $"[{LastLines[i].Time:HH:mm:ss.fff}]"); _imGui.SameLineEx(0, 4);
                    //ImGui.TextColored(greyColor, $"[{LastLines[i].Handler}]"); ImGui.SameLine(0, 4);
                    _imGui.TextColored(whiteColor, LastLines[i].Message);
                }
            }

            if (_autoScroll && _imGui.GetScrollY() >= _imGui.GetScrollMaxY())
                _imGui.SetScrollHereY(1.0f);

            _imGui.EndChild();
        }

        _imGui.End();
    }
}

public record LogMessage(DateTime Time, string Handler, string Message);
