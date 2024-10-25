using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FF16Framework.Interfaces.Nex;
using FF16Framework.Interfaces.Nex.Structures;

namespace FF16Framework.Nex;

public unsafe static class NexSignatures
{
    public delegate uint NexInitialize(NexManagerInstance* @this, void* a2);
    public delegate NexTableInstance* NexGetTable(NexManagerInstance* @this, NexTableIds tableId);
    public delegate int NexGetSetCount(NexTableIds tableId);

    public delegate NexRowInstance* NexSearchRow1K(NexTableInstance* @this, int rowId);
    public delegate NexRowInstance* NexSearchRow2K(NexTableInstance* @this, int key1, int key2);
    public delegate NexRowInstance* NexSearchRow3K(NexTableInstance* @this, int key1, int key2, int key3);
    public delegate byte* NexGetRow1K(NexTableInstance* @this, int rowId);
    public delegate byte* NexGetRow2K(NexTableInstance* @this, int key1, int key2);

    public delegate void NexGetK2SetCount(NexTableInstance* table, NexSetResult* result, int key1);
    public delegate void NexGetK3SetCount(NexTableInstance* table, NexSetResult* result, int key1, int key2);

    public delegate byte* NexGetRowData(NexRowInstance* @this);
    public delegate bool NexGetRowKeys(NexRowInstance* @this, int* result, int numRows);
    public delegate bool NexIsTableLoaded(NexTableInstance* table);

}
