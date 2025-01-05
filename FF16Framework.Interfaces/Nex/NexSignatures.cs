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
    public delegate uint NexGetSetCount(NexTableIds tableId);

    public delegate NexRowInstance* NexSearchRow1K(NexTableInstance* @this, uint rowId);
    public delegate NexRowInstance* NexSearchRow2K(NexTableInstance* @this, uint key1, uint key2);
    public delegate NexRowInstance* NexSearchRow3K(NexTableInstance* @this, uint key1, uint key2, uint key3);

    public delegate byte* NexGetRow1KByIndex(NexTableInstance* @this, uint rowId);
    public delegate byte* NexGetRow2KByIndex(NexTableInstance* @this, uint key1, uint key2);

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
