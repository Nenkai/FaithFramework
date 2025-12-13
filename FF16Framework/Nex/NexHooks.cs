using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.Nex;
using FF16Framework.Interfaces.Nex.Structures;

using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using static FF16Framework.Interfaces.Nex.NexSignatures;
using static FF16Framework.Save.SaveHooks;

namespace FF16Framework.Nex;

public unsafe class NexHooks : HookGroupBase
{
    public NexManagerInstance* Instance;

    private IHook<NexInitialize> NexInitializeHook;

    public NexGetTable NexGetTableFunction { get; private set; }
    public NexGetSetCount NexGetSetCountFunction { get; private set; }

    public NexGetRow1KByIndex NexGetRow1KByIndexFunction { get; private set; }
    public NexGetRow2KByIndex NexGetRow2KByIndexFunction { get; private set; }

    /// <summary>
    /// Gets data directly for a 2K row
    /// </summary>
    public NexGetRowData2K NexGetRowData2KFunction { get; private set; }

    public NexSearchRow1K NexSearchRow1KFunction { get; private set; }
    public NexSearchRow2K NexSearchRow2KFunction { get; private set; }
    public NexSearchRow3K NexSearchRow3KFunction { get; private set; }

    public NexGetK2SetCountForType2 NexGetK2SetCountForType2Function { get; private set; }
    public NexGetK3SetCountForType3 NexGetK3SetCountForType3Function { get; private set; }
    public NexGetK2SetCount NexGetK2CountFunction { get; private set; }

    public NexGetRowData NexGetRowDataFunction { get; private set; }
    public NexGetRowKeys NexGetRowKeysFunction { get; private set; }

    public NexIsTableLoaded NexIsTableLoadedFunction { get; private set; }

    public NexDataFileFindK2SetInfo NexDataFileFindK2SetInfoFunction { get; private set; }
    public NexDataFileFindK3SetInfo NexDataFileFindK3SetInfoFunction { get; private set; }

    public event NexLoadedEvent OnNexInitialized;

    public NexHooks(Config config, IModConfig modConfig, ILogger logger)
        : base(config, modConfig, logger)
    {
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(NexInitialize), (result, hooks) => NexInitializeHook = hooks.CreateHook<NexInitialize>(OnNxlLoadDetour, result).Activate());
        Project.Scans.AddScanHook(nameof(NexGetTable), (result, hooks) => NexGetTableFunction = hooks.CreateWrapper<NexGetTable>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetSetCount), (result, hooks) => NexGetSetCountFunction = hooks.CreateWrapper<NexGetSetCount>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetRow1KByIndex), (result, hooks) => NexGetRow1KByIndexFunction = hooks.CreateWrapper<NexGetRow1KByIndex>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetRow2KByIndex), (result, hooks) => NexGetRow2KByIndexFunction = hooks.CreateWrapper<NexGetRow2KByIndex>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetRowData2K), (result, hooks) => NexGetRowData2KFunction = hooks.CreateWrapper<NexGetRowData2K>(result, out _));
        Project.Scans.AddScanHook(nameof(NexSearchRow1K), (result, hooks) => NexSearchRow1KFunction = hooks.CreateWrapper<NexSearchRow1K>(result, out _));
        Project.Scans.AddScanHook(nameof(NexSearchRow2K), (result, hooks) => NexSearchRow2KFunction = hooks.CreateWrapper<NexSearchRow2K>(result, out _));
        Project.Scans.AddScanHook(nameof(NexSearchRow3K), (result, hooks) => NexSearchRow3KFunction = hooks.CreateWrapper<NexSearchRow3K>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetK2SetCountForType2), (result, hooks) => NexGetK2SetCountForType2Function = hooks.CreateWrapper<NexGetK2SetCountForType2>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetK3SetCountForType3), (result, hooks) => NexGetK3SetCountForType3Function = hooks.CreateWrapper<NexGetK3SetCountForType3>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetK2SetCount), (result, hooks) => NexGetK2CountFunction = hooks.CreateWrapper<NexGetK2SetCount>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetRowData), (result, hooks) => NexGetRowDataFunction = hooks.CreateWrapper<NexGetRowData>(result, out _));
        Project.Scans.AddScanHook(nameof(NexGetRowKeys), (result, hooks) => NexGetRowKeysFunction = hooks.CreateWrapper<NexGetRowKeys>(result, out _));
        Project.Scans.AddScanHook(nameof(NexIsTableLoaded), (result, hooks) => NexIsTableLoadedFunction = hooks.CreateWrapper<NexIsTableLoaded>(result, out _));
        Project.Scans.AddScanHook(nameof(NexDataFileFindK2SetInfo), (result, hooks) => NexDataFileFindK2SetInfoFunction = hooks.CreateWrapper<NexDataFileFindK2SetInfo>(result, out _));
        Project.Scans.AddScanHook(nameof(NexDataFileFindK3SetInfo), (result, hooks) => NexDataFileFindK3SetInfoFunction = hooks.CreateWrapper<NexDataFileFindK3SetInfo>(result, out _));
    }

    public uint OnNxlLoadDetour(NexManagerInstance* @this, void* a2)
    {
        uint errorCode = NexInitializeHook.OriginalFunction(@this, a2);
        if (errorCode == 0)
        {
            // The nxl/nxds were loaded and parsed/initialized. Now we Wait for a bit.

            // The hook is installed in the .nxl load handler, which itself loads .nxds.
            // When the function that opens and inits/parses the format for a registered game file format handler is done, the game still does some processing like
            // setting the file open state to 1 and registering or caching the file in some map.

            // anything that happens after the load but before the finalization is subject to a race. so we sleep for now.
            // format loader ref: 89 54 24 ? 53 55 56 57 41 54 41 55 41 56 41 57 48 83 EC ? 48 8B 81
            Thread.Sleep(100);

            _logger.WriteLine($"[{_modConfig.ModId}] Game successfully loaded nex tables.", _logger.ColorGreen);

            Instance = @this;
            OnNexInitialized?.Invoke();
        }
        else
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Game failed to load nex tables. Returned error code {errorCode:X8}", _logger.ColorRed);
        }

        return errorCode;
    }

    public override void UpdateConfig(Config configuration)
    {
        base.UpdateConfig(configuration);
    }
}
