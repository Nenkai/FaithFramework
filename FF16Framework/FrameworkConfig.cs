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
    public SoundManagerConfig SoundManager { get; set; } = new();
    public EntityManagerConfig EntityManager { get; set; } = new();
    public MagicSystemConfig MagicSystem { get; set; } = new();

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

    public class SoundManagerConfig
    {
        public bool IgnoreSystemVolumeChangesField = false;
        public bool IgnoreSystemVolumeChanges { get => IgnoreSystemVolumeChangesField; set => IgnoreSystemVolumeChangesField = value; }
    }

    public class EntityManagerConfig
    {
        public bool PrintEntityLoadsField = false;
        public bool PrintEntityLoads { get => PrintEntityLoadsField; set => PrintEntityLoadsField = value; }
    }

    public class MagicSystemConfig
    {
        public bool PrintMagicCastsField = false;
        public bool PrintMagicCasts { get => PrintMagicCastsField; set => PrintMagicCastsField = value; }
    }
}
