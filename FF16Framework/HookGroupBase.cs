using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Memory.SigScan.ReloadedII.Interfaces;

using FF16Framework.Configuration;
using Reloaded.Hooks.Definitions;
using Reloaded.Mod.Interfaces;
using SharedScans.Interfaces;

namespace FF16Framework;

public abstract class HookGroupBase
{
    protected Config _configuration;
    protected IModConfig _modConfig;
    protected ISharedScans _scans;
    protected ILogger _logger;

    public HookGroupBase(Config config, IModConfig modConfig, ISharedScans scans, ILogger logger)
    {
        _configuration = config;
        _modConfig = modConfig;
        _scans = scans;
        _logger = logger;
    }

    public abstract void Setup();

    public virtual void UpdateConfig(Config configuration)
    {
        _configuration = configuration;
    }
}
