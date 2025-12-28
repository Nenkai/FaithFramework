using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Logging;

public class R2LoggerToMSLoggerAdapterProvider : ILoggerProvider
{
    private Reloaded.Mod.Interfaces.ILogger _reloadedLogger;

    public R2LoggerToMSLoggerAdapterProvider(Reloaded.Mod.Interfaces.ILogger reloadedLogger)
    {
        _reloadedLogger = reloadedLogger;
    }

    public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
    {
        return new R2LoggerToMSLoggerAdapter(_reloadedLogger);
    }

    public void Dispose()
    {

    }
}
