using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using Reloaded.Hooks.Definitions;
using FF16Framework.Interfaces.Nex.Structures;
using FF16Framework.Interfaces.Nex;

using SharedScans.Interfaces;

using static FF16Framework.Interfaces.Nex.NexSignatures;

namespace FF16Framework.Nex;

public unsafe class NexHooks : HookGroupBase
{
    public NexManagerInstance* Instance;

    private HookContainer<NexInitialize> NexInitializeHook;

    public WrapperContainer<NexGetTable> NexGetTableFunction { get; private set; }
    public WrapperContainer<NexGetSetCount> NexGetSetCountFunction { get; private set; }

    public WrapperContainer<NexGetRow1KByIndex> NexGetRow1KByIndexFunction { get; private set; }
    public WrapperContainer<NexGetRow2KByIndex> NexGetRow2KByIndexFunction { get; private set; }

    /// <summary>
    /// Gets data directly for a 2K row
    /// </summary>
    public WrapperContainer<NexGetRowData2K> NexGetRowData2KFunction { get; private set; }

    public WrapperContainer<NexSearchRow1K> NexSearchRow1KFunction { get; private set; }
    public WrapperContainer<NexSearchRow2K> NexSearchRow2KFunction { get; private set; }
    public WrapperContainer<NexSearchRow3K> NexSearchRow3KFunction { get; private set; }

    public WrapperContainer<NexGetK2SetCountForType2> NexGetK2SetCountForType2Function { get; private set; }
    public WrapperContainer<NexGetK3SetCountForType3> NexGetK3SetCountForType3Function { get; private set; }
    public WrapperContainer<NexGetK2SetCount> NexGetK2CountFunction { get; private set; }

    public WrapperContainer<NexGetRowData> NexGetRowDataFunction { get; private set; }
    public WrapperContainer<NexGetRowKeys> NexGetRowKeysFunction { get; private set; }

    public WrapperContainer<NexIsTableLoaded> NexIsTableLoadedFunction { get; private set; }

    public WrapperContainer<NexDataFileFindK2SetInfo> NexDataFileFindK2SetInfoFunction { get; private set; }
    public WrapperContainer<NexDataFileFindK3SetInfo> NexDataFileFindK3SetInfoFunction { get; private set; }

    public event NexLoadedEvent OnNexInitialized;

    public Dictionary<string, string> Patterns = new()
    {
        [nameof(NexInitialize)] = "48 89 5C 24 ?? 55 56 57 41 54 41 55 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 85 ?? ?? ?? ?? 4C 8B 42",
        [nameof(NexGetTable)] = "45 33 C0 89 54 24 ?? 45 8B D0 4C 8B D9 49 B9 25 23 22 84 E4 9C F2 CB 42 0F B6 44 14 ?? 48 B9 B3 01 00 00 00 01 00 00 4C 33 C8 49 FF C2 4C 0F AF C9 49 83 FA 04 72 ?? 49 8B 4B ?? 49 23 C9 4D 8B 4B ?? 48 03 C9 49 8B 44 C9 ?? 49 3B 43 ?? 74 ?? 4D 8B 0C C9 EB ?? 49 3B C1 74 ?? 48 8B 40 ?? 3B 50 ?? 75 ?? EB ?? 49 8B C0 48 85 C0 49 0F 44 43 ?? 49 3B 43 ?? 74 ?? 4C 8B 40",
        [nameof(NexGetSetCount)] = "40 53 48 83 EC ?? 8B D1 33 DB",
        [nameof(NexGetRow1KByIndex)] = "40 53 48 83 EC ?? 8B DA 8B D1 48 8B 0D ?? ?? ?? ?? 48 85 C9 74 ?? E8 ?? ?? ?? ?? 4C 8B C0 48 85 C0 74 ?? 8B 48 ?? 83 E9 ?? 83 F9 ?? 77 ?? 48 8B C8 E8 ?? ?? ?? ?? 84 C0 75 ?? 49 8B 40",
        [nameof(NexGetRow2KByIndex)] = "48 89 5C 24 ?? 57 48 83 EC ?? 48 8B 41 ?? 49 8B D8 48 8B F9",
        [nameof(NexGetRowData2K)] = "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 8B F2 41 8B F8 8B D1",
        [nameof(NexSearchRow1K)] = "48 8B 41 ?? 48 85 C0 74 ?? 48 83 E8 ?? 74 ?? 48 83 F8 ?? 74 ?? 45 33 C9 45 33 C0",
        [nameof(NexGetK2SetCount)] = "40 53 48 83 EC ?? 48 8B 41 ?? 33 DB 48 85 C0 74 ?? 48 83 E8", // Works with double or triple
        [nameof(NexGetRowData)] = "48 8B 01 48 BA",
        [nameof(NexGetRowKeys)] = "48 8B 01 4C 8B D2",
        [nameof(NexIsTableLoaded)] = "48 8B 51 ?? 48 85 D2 74 ?? 33 C0 F0 0F C1 42 ?? 83 C0",

        // Data file related
        [nameof(NexDataFileFindK2SetInfo)] = "48 89 5C 24 ?? 57 48 83 EC ?? 48 63 79 ?? 45 33 C9 8B DA 4C 8B D1 44 39 49 ?? 76 ?? 48 63 09 4C 8B DF 49 D1 EB 49 03 CA 49 63 C3 48 8D 14 40", // bsearches
        [nameof(NexDataFileFindK3SetInfo)] = "48 89 5C 24 ?? 57 48 83 EC ?? 48 63 79 ?? 45 33 C9 8B DA 4C 8B D1 44 39 49 ?? 76 ?? 48 63 09 4C 8B DF 49 D1 EB 49 03 CA 49 63 C3 48 8D 14 80", // bsearches

    };

    public NexHooks(Config config, IModConfig modConfig, IModLoader loader, ISharedScans scans, ILogger logger)
        : base(config, modConfig, loader, scans, logger)
    {

    }

    public override void Setup()
    {
        foreach (var pattern in Patterns)
            _scans.AddScan(pattern.Key, pattern.Value);

        string appExePath = _modLoader.GetAppConfig().AppLocation;
        if (appExePath.Contains("ffxvi"))
        {
            _scans.AddScan(nameof(NexSearchRow2K), "48 8B 41 ?? 48 85 C0 74 ?? 48 83 E8 ?? 74 ?? 48 83 F8 ?? 74 ?? 45 33 C9 E9"); 
            _scans.AddScan(nameof(NexSearchRow3K), "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC 30 48 83 79 ?? ?? 49 8B F1");
            _scans.AddScan(nameof(NexGetK2SetCountForType2), "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 48 83 79 ?? ?? 48 8B DA"); // Only double keyed
            _scans.AddScan(nameof(NexGetK3SetCountForType3), "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 48 83 79 ?? ?? 41 8B F1");
        }
        else
        {
            _scans.AddScan(nameof(NexSearchRow2K), "48 83 EC ?? 48 8B 41 ?? 48 85 C0 74 ?? 48 83 E8");
            _scans.AddScan(nameof(NexSearchRow3K), "48 83 EC ?? 48 83 79 ?? ?? 73");
            _scans.AddScan(nameof(NexGetK2SetCountForType2), "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? B8 ?? ?? ?? ?? 33 05 ?? ?? ?? ?? 48 39 41"); // Only double keyed
            //_scans.AddScan(nameof(NexGetK3SetCountForType3), "48 89 5C 24 ?? 48 89 74 24 ?? 57 48 83 EC ?? 48 83 79 ?? ?? 41 8B F1");
        }

        NexInitializeHook = _scans.CreateHook<NexInitialize>(OnNxlLoadDetour, _modConfig.ModId);
        NexGetTableFunction = _scans.CreateWrapper<NexGetTable>(_modConfig.ModId);
        NexGetSetCountFunction = _scans.CreateWrapper<NexGetSetCount>(_modConfig.ModId);
        NexGetRow1KByIndexFunction = _scans.CreateWrapper<NexGetRow1KByIndex>(_modConfig.ModId);
        NexGetRow2KByIndexFunction = _scans.CreateWrapper<NexGetRow2KByIndex>(_modConfig.ModId);
        NexGetRowData2KFunction = _scans.CreateWrapper<NexGetRowData2K>(_modConfig.ModId);
        NexSearchRow1KFunction = _scans.CreateWrapper<NexSearchRow1K>(_modConfig.ModId);
        NexSearchRow2KFunction = _scans.CreateWrapper<NexSearchRow2K>(_modConfig.ModId);
        NexSearchRow3KFunction = _scans.CreateWrapper<NexSearchRow3K>(_modConfig.ModId);
        NexGetK2SetCountForType2Function = _scans.CreateWrapper<NexGetK2SetCountForType2>(_modConfig.ModId);
        NexGetK3SetCountForType3Function = _scans.CreateWrapper<NexGetK3SetCountForType3>(_modConfig.ModId);
        NexGetK2CountFunction = _scans.CreateWrapper<NexGetK2SetCount>(_modConfig.ModId);
        NexGetRowDataFunction = _scans.CreateWrapper<NexGetRowData>(_modConfig.ModId);
        NexGetRowKeysFunction = _scans.CreateWrapper<NexGetRowKeys>(_modConfig.ModId);
        NexIsTableLoadedFunction = _scans.CreateWrapper<NexIsTableLoaded>(_modConfig.ModId);
        NexDataFileFindK2SetInfoFunction = _scans.CreateWrapper<NexDataFileFindK2SetInfo>(_modConfig.ModId);
        NexDataFileFindK3SetInfoFunction = _scans.CreateWrapper<NexDataFileFindK3SetInfo>(_modConfig.ModId);
    }

    public uint OnNxlLoadDetour(NexManagerInstance* @this, void* a2)
    {
        uint errorCode = NexInitializeHook.Hook!.OriginalFunction(@this, a2);
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
