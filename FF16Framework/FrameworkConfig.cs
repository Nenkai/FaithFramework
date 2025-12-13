using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tomlyn;

namespace FF16Framework;

public class FrameworkConfig
{
    private string _configPath;

    public GameInfoOverlayConfig GameInfoOverlay { get; set; } = new();

    public FrameworkConfig()
    {

    }

    public FrameworkConfig(string configPath)
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

    public class GameInfoOverlayConfig
    {
        public bool ShowActorInfoField = false;
        public bool ShowCurrentActorInfo { get => ShowActorInfoField; set => ShowActorInfoField = value; }
    }
}
