using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.Nex.Structures;

namespace FF16Framework.Interfaces.Nex;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public unsafe static class NexSignatures
{
    /// <summary>
    /// Delegates for a game function that initializes nex.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="a2"></param>
    /// <returns></returns>
    public delegate uint NexInitialize(NexManagerInstance* @this, void* a2);

    /// <summary>
    /// Delegates for a game function that fetches the instance for a table/sheet id.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="tableId"></param>
    /// <returns></returns>
    public delegate NexTableInstance* NexGetTable(NexManagerInstance* @this, uint tableId);

    /// <summary>
    /// Delegate for a game function that fetches the number of sets (main keys) in a table.
    /// </summary>
    /// <param name="tableId"></param>
    /// <returns></returns>
    public delegate uint NexGetSetCount(uint tableId);

    /// <summary>
    /// Delegate for a game function that fetches a row instance by key (for single keyed tables).
    /// </summary>
    /// <param name="this"></param>
    /// <param name="rowId"></param>
    /// <returns></returns>
    public delegate NexRowInstance* NexSearchRow1K(NexTableInstance* @this, uint rowId);

    /// <summary>
    /// Delegate for a game function that fetches a row instance by key (for double keyed tables).
    /// </summary>
    /// <param name="this"></param>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    public delegate NexRowInstance* NexSearchRow2K(NexTableInstance* @this, uint key1, uint key2);

    /// <summary>
    /// Delegate for a game function that fetches a row instance by key (for triple keyed tables).
    /// </summary>
    /// <param name="this"></param>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <param name="key3"></param>
    /// <returns></returns>
    public delegate NexRowInstance* NexSearchRow3K(NexTableInstance* @this, uint key1, uint key2, uint key3);

    /// <summary>
    /// Delegate for a game function that fetches a single-keyed row by index.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="rowId"></param>
    /// <returns></returns>
    public delegate byte* NexGetRow1KByIndex(NexTableInstance* @this, uint rowId);

    /// <summary>
    /// Delegate for a game function that fetches a double-keyed row by key+index.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    public delegate byte* NexGetRow2KByIndex(NexTableInstance* @this, uint key1, uint key2);

    /// <summary>
    /// Delegate for a game function that directly fetches row data for a double keyed row by key+key2.
    /// </summary>
    /// <param name="this"></param>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns></returns>
    public delegate byte* NexGetRowData2K(NexTableInstance* @this, uint key1, uint key2);

    public delegate void NexGetK2SetCountForType2(NexTableInstance* table, NexSetResult* result, uint key1);
    public delegate void NexGetK3SetCountForType3(NexTableInstance* table, NexSetResult* result, uint key1, uint key2);
    public delegate uint NexGetK2SetCount(NexTableInstance* table, uint key1);

    public delegate byte* NexGetRowData(NexRowInstance* @this);
    public delegate bool NexGetRowKeys(NexRowInstance* @this, uint* result, uint numRows);
    public delegate bool NexIsTableLoaded(NexTableInstance* table);

    public delegate NexDataFile2KSetInfo* NexDataFileFindK2SetInfo(byte* nxdfFileBuffer, uint key1);
    public delegate NexDataFile3KSetInfo* NexDataFileFindK3SetInfo(byte* nxdfFileBuffer, uint key1);

}

#pragma warning restore CS1591
