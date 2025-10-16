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

namespace FF16Framework.Save;

public unsafe class SaveHooks : HookGroupBase
{
    public NexManagerInstance* Instance;

    private HookContainer<SerializeSave>? SerializeSaveHook;
    public delegate nint SerializeSave(nint a1, nint a2, nint saveEntity, string fileName, bool saveAsXml);

    private HookContainer<SerializeSystemSave>? SerializeSystemSaveHook;
    public delegate nint SerializeSystemSave(nint a1, nint saveEntity,  bool saveAsXml);

    private HookContainer<DeserializeSave>? DeserializeSaveHook;
    public delegate nint DeserializeSave(nint a1, nint a2, string fileName, bool saveAsXml, nint a5);

    private HookContainer<CreateXmlSerializer>? CreateXmlSerializerHook;
    public delegate nint CreateXmlSerializer(nint a1, string fileName, uint version);

    public Dictionary<string, string> Patterns = new()
    {
        [nameof(SerializeSave)] = "48 89 5C 24 ?? 55 56 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 89 91",
        [nameof(SerializeSystemSave)] = "48 89 5C 24 ?? 55 56 57 41 56 41 57 48 8D AC 24 ?? ?? ?? ?? 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 85 ?? ?? ?? ?? 48 8B B9",
        [nameof(DeserializeSave)] = "48 89 5C 24 ?? 55 56 57 48 81 EC ?? ?? ?? ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 84 24 ?? ?? ?? ?? 48 8B AC 24",
        [nameof(CreateXmlSerializer)] = "4C 8B DC 49 89 5B ?? 49 89 73 ?? 57 48 83 EC ?? 48 83 79",
    };

    public SaveHooks(Config config, IModConfig modConfig, IModLoader loader, ISharedScans scans, ILogger logger)
        : base(config, modConfig, loader, scans, logger)
    {

    }

    public override void Setup()
    {
        string appExePath = _modLoader.GetAppConfig().AppLocation;
        if (!appExePath.Contains("ffxvi")) // Check for FFXVI. Most of the functionality was removed in FFT
            return;

        foreach (var pattern in Patterns)
            _scans.AddScan(pattern.Key, pattern.Value);

        // Initial strategy was to hook the serializers/deserializer to pass saveAsXml as true.
        // https://nenkai.github.io/ffxvi-modding/resources/other/save_files/?h=save
        //
        // When that happens, the save png has an extra entry with the .xml extension.
        // Simple idea was to hook the deserializer, try to load as xml, and then check on the error
        // and load raw instead.
        //
        // If... an error was actually returned in 'DeserializeSave'. Turns out it's not, the internal reader's error code is never returned
        // so 0 (success) is always returned to the callee. fun. (48 89 5C 24 ? 48 89 6C 24 ? 48 89 74 24 ? 57 41 56 41 57 48 81 EC ? ? ? ? 48 8B 3D)
        //
        // New strategy is to always pass saveAsXml as true still, BUT, change the path passed to the xml serializer ctor
        // To NOT have the xml extension. Serializer looks like:
        //
        // ---------------------------------------------
        // char name[0x100];
        // if (saveAsXml)
        // {
        //   
        //     sprintf(name, 0xFF, "%s.xml", file_name); 
        //     serializer* xmlSerializer = CreateSerializerXml(a1, name, 2019111101);
        // }
        // else
        //    [...binary serializer]...
        //----------------------------------------------
        // 
        // So, why? When deserializing, the game checks that the file is longer than 0x15 bytes, and explicitly checks for '<?xml version=\"1.0\"?>' for whether
        // The current file being dealt with is actually a xml. (40 53 55 56 57 41 56 41 57 48 83 EC ? 48 8B 05 ? ? ? ? 48 33 C4 48 89 44 24 ? 48 8B B4 24)
        // So. Just strip the extension! Game will load the save anyway since it proper checks on that.

        
        SerializeSaveHook = _scans.CreateHook<SerializeSave>(SerializeSaveDetour, _modConfig.ModId);
        SerializeSystemSaveHook = _scans.CreateHook<SerializeSystemSave>(SerializeSystemSaveDetour, _modConfig.ModId);
        //DeserializeSaveHook = _scans.CreateHook<DeserializeSave>(DeserializeSaveDetour, _modConfig.ModId);
        CreateXmlSerializerHook = _scans.CreateHook<CreateXmlSerializer>(CreateXmlSerializerDetour, _modConfig.ModId);
    }

    
    public nint SerializeSaveDetour(nint a1, nint a2, nint saveEntity, string fileName, bool saveAsXml)
    {
        if (_configuration.SerializeSavesAsXml)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Saving '{fileName}' as XML...");
            return SerializeSaveHook!.Hook?.OriginalFunction(a1, a2, saveEntity, fileName, true) ?? 0;
        }
        else
            return SerializeSaveHook!.Hook?.OriginalFunction(a1, a2, saveEntity, fileName, false) ?? 0;
    }

    public nint SerializeSystemSaveDetour(nint a1, nint saveEntity,  bool saveAsXml)
    {
        if (_configuration.SerializeSavesAsXml)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Saving system file (system-save-data) as XML...");
            return SerializeSystemSaveHook!.Hook?.OriginalFunction(a1, saveEntity, true) ?? 0;
        }
        else
            return SerializeSystemSaveHook!.Hook?.OriginalFunction(a1, saveEntity, false) ?? 0;
    }

    /*
    public nint DeserializeSaveDetour(nint a1, nint a2, string fileName, bool saveAsXml, nint a5)
    {
        _logger.WriteLine($"[{_modConfig.ModId}] DeserializeSaveDetour: {a1:X8}, {a2:X8}, {fileName}, {saveAsXml}, {a5:X8}");
        if (_configuration.LoadSavesFromXml)
        {
            _logger.WriteLine($"[{_modConfig.ModId}] Loading '{fileName}' from XML...");
            nint res = DeserializeSaveHook!.Hook?.OriginalFunction(a1, a2, fileName, true, a5) ?? 0;
            
            // TODO: Check how to load raw file if XML failed.
            // result is unreliable. It's not returned by the call performing the reading. Fun.

            return res;
        }
        else
            return DeserializeSaveHook!.Hook?.OriginalFunction(a1, a2, fileName, false, a5) ?? 0;
    }
    */

    public nint CreateXmlSerializerDetour(nint a1, string fileName, uint version)
    {
        if (fileName == "config.xml")
            return CreateXmlSerializerHook!.Hook?.OriginalFunction(a1, fileName, version) ?? 0;

        return CreateXmlSerializerHook!.Hook?.OriginalFunction(a1, Path.GetFileNameWithoutExtension(fileName), version) ?? 0;
    }

    public override void UpdateConfig(Config configuration)
    {
        base.UpdateConfig(configuration);
    }
}
