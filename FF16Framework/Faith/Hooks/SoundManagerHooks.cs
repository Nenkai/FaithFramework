using FF16Framework.Interfaces.Nex.Structures;

using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.Definitions.Structs;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;

using RyoTune.Reloaded;

using SixLabors.ImageSharp.PixelFormats;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Faith.Hooks;

public unsafe class SoundManagerHooks : HookGroupBase
{
    private readonly FrameworkConfig _frameworkConfig;

    public delegate void faith_Sound_SoundManager_SetVolume(nint @this, float volume, float a3);
    public IHook<faith_Sound_SoundManager_SetVolume> SoundManager_SetVolume { get; private set; }

    public SoundManagerHooks(Config config, IModConfig modConfig, ILogger logger,
        FrameworkConfig frameworkConfig)
    : base(config, modConfig, logger)
    {
        _frameworkConfig = frameworkConfig;
    }

    public override void SetupHooks()
    {
        Project.Scans.AddScanHook(nameof(faith_Sound_SoundManager_SetVolume), (result, hooks)
            => SoundManager_SetVolume = hooks.CreateHook<faith_Sound_SoundManager_SetVolume>(SetVolumeImpl, result).Activate());
    }

    public void SetVolumeImpl(nint @this, float volume, float a3)
    {
        // SetVolume is only called in FFXVI/FFT when:
        // - The sound manager is initialized
        // - Each swapchain/window WNDPROC handler handles WM_ACTIVATE
        //   The code looks more or less like this:

        /*   if ( g_Application->IsInitialized )
             {
               if ( g_SoundManager->field_2081 )
               {
                 if ( g_SoundManager->field_2080 )
                 {
                   if ( (_WORD)wParam )
                   {
                     if ( g_SoundManager->IsMuted )
                     {
                       OldVolume = g_SoundManager->OldVolume;
                       g_SoundManager->OldVolume = 0.0;
                       g_SoundManager->IsMuted = false;
                       faith::Sound::SoundManager::SetVolume(g_SoundManager, OldVolume, 0.0);
                     }
                   }
                   else if ( !g_SoundManager->IsMuted )
                   {
                     g_SoundManager->OldVolume = g_SoundManager->CurrentVolume;
                     faith::Sound::SoundManager::SetVolume(g_SoundManager, 0.0, 0.0);
                     soundManager->IsMuted = true;
                   }
                 }
               }
             }
        */

        if (!_frameworkConfig.SoundManager.IgnoreSystemVolumeChanges)
            SoundManager_SetVolume.OriginalFunction(@this, volume, a3);
    }
}
