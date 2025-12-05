using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using Reloaded.Mod.Interfaces;

using NenTools.ImGui.Interfaces;
using NenTools.ImGui.Shell;
using NenTools.ImGui.Shell.Interfaces;

namespace FF16Framework.ImGuiManager.Windows;

[ImGuiMenu(Category = "File", Priority = ImGuiShell.SystemPriority, Owner = nameof(FF16Framework))]
public unsafe class LogWindow : IImGuiComponent
{
    public bool IsOverlay => false;

    public bool IsOpen = false;
    public bool _autoScroll = true;

    private ILogger _logger;

    private StreamWriter _sw;

    private const int MAX_LINES = 5000;
    public List<LogMessage> LastLines = new(MAX_LINES);
    private static object _lock = new object();

    private readonly IImGui _imgui;
    public LogWindow(IImGui imgui, ILogger logger, string logPath)
    {
        _imgui = imgui;
        _logger = logger;
        _logger.OnWriteLine += _logger_OnWriteLine;
        _sw = new StreamWriter(logPath);
    }

    ~LogWindow()
    {
        _sw.Flush();
    }

    private void _logger_OnWriteLine(object sender, (string text, System.Drawing.Color color) e)
    {
        lock (_lock)
        {
            if (LastLines.Count >= MAX_LINES)
                LastLines.Remove(LastLines[0]);

            var logMsg = new LogMessage(DateTime.UtcNow, sender.ToString(), e.text);
            LastLines.Add(logMsg);
            _sw.WriteLine(e.text);
        }
    }

    public void RenderMenu(IImGuiShell imguiSupport)
    {
        if (_imgui.MenuItemEx("Log Window", "", false, true))
        {
            IsOpen = true;
        }
    }

    public void Render(IImGuiShell imguiSupport)
    {
        if (!IsOpen)
            return;

        if (_imgui.Begin("Log Window", ref IsOpen, 0))
        {
            if (_imgui.SmallButton("Copy"))
                _imgui.SetClipboardText(string.Join("\n", LastLines.Select(e => e.Message)));
            _imgui.SameLineEx(0, 2);

            if (_imgui.SmallButton("Clear"))
                LastLines.Clear();
            _imgui.SameLineEx(0, 2);
            _imgui.Checkbox("Auto-scroll", ref _autoScroll);

            _imgui.BeginChild("##log_window_container", new Vector2(), 0, ImGuiWindowFlags.ImGuiWindowFlags_AlwaysVerticalScrollbar | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysHorizontalScrollbar);

            var greyColor = new Vector4(0.4f, 0.4f, 0.4f, 0.4f);
            var whiteColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            lock (_lock)
            {
                for (int i = 0; i < LastLines.Count; i++)
                {
                    _imgui.TextColored(greyColor, $"[{LastLines[i].Time:HH:mm:ss.fff}]"); _imgui.SameLineEx(0, 4);
                    //ImGui.TextColored(greyColor, $"[{LastLines[i].Handler}]"); ImGui.SameLine(0, 4);
                    _imgui.TextColored(whiteColor, LastLines[i].Message);
                }
            }

            if (_autoScroll && _imgui.GetScrollY() >= _imgui.GetScrollMaxY())
                _imgui.SetScrollHereY(1.0f);

            _imgui.EndChild();
        }

        _imgui.End();
    }
}

public record LogMessage(DateTime Time, string Handler, string Message);
