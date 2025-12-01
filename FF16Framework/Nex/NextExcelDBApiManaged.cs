
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.Nex.Structures;
using FF16Framework.Interfaces.Nex;

namespace FF16Framework.Nex;

public class NextExcelDBApiManaged : INextExcelDBApiManaged
{
    private readonly NexHooks _nexHooks;

    public bool Initialized
    {
        get { unsafe { return _nexHooks.Instance is not null; } }
    }

    public event NexLoadedEvent OnNexLoaded
    {
        add => _nexHooks.OnNexInitialized += value;
        remove => _nexHooks.OnNexInitialized -= value;
    }

    public NextExcelDBApiManaged(NexHooks nexHooks)
    {
        _nexHooks = nexHooks;
    }

    public INexTable? GetTable(NexTableIds tableId)
    {
        unsafe
        {
            NexTableInstance* tableInstance = _nexHooks.NexGetTableFunction(_nexHooks.Instance, tableId);
            if (tableInstance is null)
                return null;

            return new NexTable(_nexHooks, tableInstance);
        }
    }

    public uint GetMainRowCount(NexTableIds table)
        => _nexHooks.NexGetSetCountFunction(table);

    public bool IsTableLoaded(NexTableIds tableId)
    {
        unsafe
        {
            NexTableInstance* tableInstance = _nexHooks.NexGetTableFunction(_nexHooks.Instance, tableId);
            if (tableInstance is null)
                return false;

            return _nexHooks.NexIsTableLoadedFunction(tableInstance);
        }
    }
}
