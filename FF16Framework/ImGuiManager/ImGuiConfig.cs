using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Tomlyn;

namespace FF16Framework.ImGuiManager;

public class ImGuiConfig
{
    private string _configPath;

    public OverlayLoggerConfig OverlayLogger { get; set; } = new();

    public ImGuiConfig()
    {

    }

    public ImGuiConfig(string configPath)
    {
        _configPath = configPath;
    }

    public void SetPath(string path)
    {
        _configPath = path;
    }

    public void Save()
    {
        string text = Toml.FromModel(this, new TomlModelOptions());
        File.WriteAllText(_configPath, text);
    }

    public class OverlayLoggerConfig
    {
        public bool EnabledField = true;
        public bool Enabled { get => EnabledField; set => EnabledField = value; }
        public int MaxLinesField = 20;
        public int MaxLines { get => MaxLinesField; set => MaxLinesField = value; }
        public float FadeTimeSecondsField = 8.0f;
        public float FadeTimeSeconds { get => FadeTimeSecondsField; set => FadeTimeSecondsField = value; }
    }
}
