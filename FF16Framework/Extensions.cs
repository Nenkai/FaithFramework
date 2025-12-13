using Microsoft.Extensions.DependencyInjection;

using Reloaded.Memory.SigScan.ReloadedII.Interfaces;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework;

public static class Extensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string BytesToString(this Span<byte> buffer)
    {
        int end;

        for (end = 0; end < buffer.Length && buffer[end] != 0; end++) ;

        unsafe
        {
            fixed (byte* pinnedBuffer = buffer)
            {
                return new((sbyte*)pinnedBuffer, 0, end);
            }
        }
    }

    public static void SigScan(this IStartupScanner startupScanner, string pattern, string name, Action<nint> action)
    {
        var baseAddress = Process.GetCurrentProcess().MainModule!.BaseAddress;
        startupScanner?.AddMainModuleScan(pattern, result =>
        {
            if (!result.Found)
            {
                return;
            }
            action(result.Offset + baseAddress);
        });
    }
    
    /// <summary>
    /// Registers a service with implementation but also as an interface
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TImpl"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddSingletonAs<TService, TImpl>(this IServiceCollection services) where TImpl : class, TService 
                                                                                                       where TService : class
    {
        services.AddSingleton<TImpl>();
        services.AddSingleton<TService>(sp => sp.GetRequiredService<TImpl>());
        return services;
    }
}
