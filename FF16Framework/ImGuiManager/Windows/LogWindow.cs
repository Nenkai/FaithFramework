using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

using FF16Framework.Interfaces.ImGui;

using Reloaded.Mod.Interfaces;
using FF16Framework.ImGuiManager;
using FF16Framework.Interfaces.ImGuiManager;

namespace FF16Framework.ImGuiManager.Windows;

public unsafe class LogWindow : IImguiWindow, IImguiMenuComponent
{
    public bool IsOverlay => false;

    public bool IsOpen = false;
    public bool _autoScroll = true;

    private ILogger _logger;

    private StreamWriter _sw = new StreamWriter("modtools_log.txt");

    public List<LogMessage> LastLines = new(2000);
    private static object _lock = new object();

    public LogWindow(ILogger logger)
    {
        _logger = logger;
        _logger.OnWriteLine += _logger_OnWriteLine;
    }

    private void _logger_OnWriteLine(object sender, (string text, System.Drawing.Color color) e)
    {
        lock (_lock)
        {
            if (LastLines.Count >= 2000)
                LastLines.Remove(LastLines[0]);

            var logMsg = new LogMessage(DateTime.UtcNow, sender.ToString(), e.text);
            LastLines.Add(logMsg);
            _sw.WriteLine(e.text);
        }
    }

    public void BeginMenuComponent(IImGui imgui)
    {
        if (imgui.MenuItemEx("Logs", "", false, true))
        {
            IsOpen = true;
        }
    }

    public void Render(IImguiSupport imguiSupport, IImGui imgui)
    {
        if (!IsOpen)
            return;

        if (imgui.Begin("Log Window", ref IsOpen, 0))
        {
            if (imgui.SmallButton("Clear"))
                LastLines.Clear();

            imgui.SameLineEx(0, 2);
            if (imgui.SmallButton("Copy"))
                imgui.SetClipboardText(string.Join("\n", LastLines.Select(e => e.Message)));

            imgui.SameLineEx(0, 2);
            imgui.Checkbox("Auto-scroll", ref _autoScroll);

            imgui.Checkbox("Enable File Logging", ref ImGuiConfig.LogFiles);
            imgui.BeginChild("##log", new Vector2(), 0, ImGuiWindowFlags.ImGuiWindowFlags_AlwaysVerticalScrollbar | ImGuiWindowFlags.ImGuiWindowFlags_AlwaysHorizontalScrollbar);

            var greyColor = new Vector4(0.4f, 0.4f, 0.4f, 0.4f);
            var whiteColor = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            lock (_lock)
            {
                for (int i = 0; i < LastLines.Count; i++)
                {
                    imgui.TextColored(greyColor, $"[{LastLines[i].Time:HH:mm:ss.fff}]"); imgui.SameLineEx(0, 4);
                    //ImGui.TextColored(greyColor, $"[{LastLines[i].Handler}]"); ImGui.SameLine(0, 4);
                    imgui.TextColored(whiteColor, LastLines[i].Message);
                }
            }


            if (_autoScroll && imgui.GetScrollY() >= imgui.GetScrollMaxY())
                imgui.SetScrollHereY(1.0f);

            imgui.EndChild();

            imgui.End();
        }
    }
}

public record LogMessage(DateTime Time, string Handler, string Message);
