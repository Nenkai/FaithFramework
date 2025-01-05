﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.Nex.Structures;
using FF16Framework.Interfaces.Nex;

namespace FF16Framework.Nex;

public unsafe class NextExcelDBApi : INextExcelDBApi
{
    private readonly NexHooks _nexHooks;

    public bool Initialized => _nexHooks.Instance is not null;

    public event NexLoadedEvent OnNexLoaded
    {
        add => _nexHooks.OnNexInitialized += value;
        remove => _nexHooks.OnNexInitialized -= value;
    }

    public NextExcelDBApi(NexHooks nexHooks)
    {
        _nexHooks = nexHooks;
    }

    public NexTableInstance* GetTable(NexTableIds tableId)
        => _nexHooks.NexGetTableFunction.Wrapper(_nexHooks.Instance, tableId);

    public int GetSetCount(NexTableIds table)
        => (int)_nexHooks.NexGetSetCountFunction.Wrapper(table);

    public byte* GetRowData(NexRowInstance* row)
        => _nexHooks.NexGetRowDataFunction.Wrapper(row);

    public bool GetRowKeys(NexRowInstance* row, int* result, int numRows)
       => _nexHooks.NexGetRowKeysFunction.Wrapper(row, (uint*)result, (uint)numRows);

    public byte* GetRow(NexTableInstance* table, int key1)
        => _nexHooks.NexGetRow1KByIndexFunction.Wrapper(table, (uint)key1);
    
    public byte* GetRow(NexTableInstance* table, int key1, int key2)
        => _nexHooks.NexGetRow2KByIndexFunction.Wrapper(table, (uint)key1, (uint)key2);
    
    public NexRowInstance* SearchRow(NexTableInstance* table, int key1)
        => _nexHooks.NexSearchRow1KFunction.Wrapper(table, (uint)key1);

    public NexRowInstance* SearchRow(NexTableInstance* table, int key1, int key2)
        => _nexHooks.NexSearchRow2KFunction.Wrapper(table, (uint)key1, (uint)key2);

    public NexRowInstance* SearchRow(NexTableInstance* table, int key1, int key2, int key3)
       => _nexHooks.NexSearchRow3KFunction.Wrapper(table, (uint)key1, (uint)key2, (uint)key3);

    public void GetDoubleKeyedSetCount(NexTableInstance* table, NexSetResult* result, int key1)
        => _nexHooks.NexGetK2SetCountForType2Function.Wrapper(table, result, (uint)key1);

    public void GetTripleKeyedSubSetCount(NexTableInstance* table, NexSetResult* result, int key1, int key2)
        => _nexHooks.NexGetK3SetCountForType3Function.Wrapper(table, result, (uint)key1, (uint)key2);

    public bool IsTableLoaded(NexTableInstance* table)
        => _nexHooks.NexIsTableLoadedFunction.Wrapper(table);

}
