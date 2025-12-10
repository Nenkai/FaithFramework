using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;

namespace FF16Framework;

public abstract class HookGroupBase
{
    protected Config _configuration;
    protected IModConfig _modConfig;
    protected ILogger _logger;
    protected IModLoader _modLoader;

    public HookGroupBase(Config config, IModConfig modConfig, ILogger logger)
    {
        _configuration = config;
        _modConfig = modConfig;
        _logger = logger;
    }

    public abstract void Setup();

    public virtual void UpdateConfig(Config configuration)
    {
        _configuration = configuration;
    }
}
