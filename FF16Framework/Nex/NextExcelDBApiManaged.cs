
using FF16Framework.Interfaces.Nex;
using FF16Framework.Interfaces.Nex.Structures;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Nex;

public class NextExcelDBApiManaged : INextExcelDBApiManagedV2
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
        => GetTable((uint)tableId);

    public uint GetMainRowCount(NexTableIds table)
        => _nexHooks.NexGetSetCountFunction.Wrapper((uint)table);

    public uint GetMainRowCount(uint tableId)
        => _nexHooks.NexGetSetCountFunction.Wrapper(tableId);

    public bool IsTableLoaded(NexTableIds tableId)
        => IsTableLoaded((uint)tableId);

    public INexTable? GetTable(uint tableId)
    {
        unsafe
        {
            NexTableInstance* tableInstance = _nexHooks.NexGetTableFunction(_nexHooks.Instance, tableId);
            if (tableInstance is null)
                return null;

            return new NexTable(_nexHooks, tableInstance);
        }
    }

    public bool IsTableLoaded(uint tableId)
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
