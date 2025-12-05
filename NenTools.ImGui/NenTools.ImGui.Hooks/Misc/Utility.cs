using NenTools.ImGui.Hooks;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using DebugLog = NenTools.ImGui.Hooks.DirectX.DebugLog;

namespace NenTools.ImGui.Hooks.Misc;

internal static class Utility
{
    /// <summary>
    /// Gets the DirectX version loaded into the current process.
    /// </summary>
    /// <param name="candidates">Candidate implementations to check for support.</param>
    /// <param name="retryTime">Time between retries in milliseconds.</param>
    /// <param name="timeout">Timeout in milliseconds to determine DX version.</param>
    public static async Task<List<IImguiHook>> GetSupportedImplementations(List<IImguiHook> candidates, int retryTime = 64, int timeout = 20000)
    {
        // Store the amount of attempts taken at hooking DirectX for a process.
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        // Loop until DirectX module found.
        var result = new List<IImguiHook>();
        while (true)
        {
            foreach (var candidate in candidates)
            {
                if (candidate.IsApiSupported()) 
                    result.Add(candidate);
            }
            
            // Check timeout.
            if (stopWatch.ElapsedMilliseconds > timeout)
                throw new Exception("No working implementation found. The application is either not a DirectX/OpenGL/??? application or uses an unsupported version of the Graphics API.");

            // Check every X milliseconds.
            if (result.Count > 0)
            {
                var impls = "";
                foreach (var res in result)
                    impls += $"{res.GetType().Name} |";

                DebugLog.WriteLine($"| Supported Implementations Detected: {impls}");
                return result;
            }

            await Task.Delay(retryTime).ConfigureAwait(false);
        }
    }
}
