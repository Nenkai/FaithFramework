using FF16Framework.Interfaces.Nex;
using FF16Framework.Interfaces.Nex.Structures;

using Reloaded.Mod.Interfaces.Structs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FF16Framework.Nex;

public class NexTable : INexTableV2
{
    private readonly unsafe NexTableInstance* _tableInstance;
    private readonly unsafe NexHooks _hooks;

    public unsafe uint TableIdRaw => _tableInstance->TableId;
    public NexTableIds TableId
    {
        get { unsafe { return (NexTableIds)TableIdRaw; } }
    }

    public NexTableType Type
    {
        get { unsafe { return (NexTableType)_tableInstance->Type; } }
    }

    public uint BaseRowId
    {
        get { unsafe { return _tableInstance->BaseRowId; } }
    }

    public unsafe NexTable(NexHooks nexHooks, NexTableInstance* instance)
    {
        _hooks = nexHooks;
        _tableInstance = instance;
    }

    private uint? _cachedMainRowCount;
    public uint GetNumRows()
    { 
        _cachedMainRowCount ??= _hooks.NexGetSetCountFunction(TableIdRaw);
        return _cachedMainRowCount.Value;
    }

    public uint GetSubSetCount(uint key1)
    {
        if (Type != NexTableType.DoubleKeyed && Type != NexTableType.TripleKeyed)
            throw new InvalidOperationException("GetTripleKeyedSubSetCount is invalid for non double/triple-keyed tables.");

        uint count = 0;
        unsafe
        {
            count = _hooks.NexGetK2CountFunction(_tableInstance, key1);
        }
        return count;
    }

    public uint GetNumSets()
    {
        unsafe
        {
            byte* subTableHeader = _tableInstance->FileHandle->Buffer + 0x20;
            return *(uint*)(subTableHeader + 0x04);
        }
    }

    public uint GetMainKeyByIndex(uint index)
    {
        unsafe
        {
            byte* typeHeader = _tableInstance->FileHandle->Buffer + 0x20;
            uint numSets = *(uint*)(typeHeader + 0x04);

            if (Type == NexTableType.SingleKeyed)
            {
                if (index > numSets)
                    throw new IndexOutOfRangeException($"GetMainKeyByIndex: index out of range. num sets: {numSets}, index: {index}");

                NexDataFile1KRowInfo* setInfo = (NexDataFile1KRowInfo*)(typeHeader + (*(int*)typeHeader) + (index * 0x08));
                return setInfo->Key1;
            }
            else if (Type == NexTableType.DoubleKeyed)
            {
                if (index > numSets)
                    throw new IndexOutOfRangeException($"GetMainKeyByIndex: index out of range. num sets: {numSets}, index: {index}");

                NexDataFile2KSetInfo* setInfo = (NexDataFile2KSetInfo*)(typeHeader + (*(int*)typeHeader) + (index * 0x0C));
                return setInfo->Key1;
            }
            else if (Type == NexTableType.TripleKeyed)
            {
                if (index > numSets)
                    throw new IndexOutOfRangeException($"GetMainKeyByIndex: index out of range. num sets: {numSets}, index: {index}");

                NexDataFile3KSetInfo* setInfo = (NexDataFile3KSetInfo*)(typeHeader + (*(int*)typeHeader) + (index * 0x14));
                return setInfo->Key1;
            }
        }

        return 0;
    }

    public IReadOnlyList<uint> GetMainKeys()
    {
        unsafe
        {
            List<uint> keys = [];
            byte* typeHeader = _tableInstance->FileHandle->Buffer + 0x20;
            uint numSets = *(uint*)(typeHeader + 0x04);

            if (Type == NexTableType.SingleKeyed)
            {
                for (int i = 0; i < numSets; i++)
                {
                    NexDataFile1KRowInfo* setInfo = (NexDataFile1KRowInfo*)(typeHeader + (*(int*)typeHeader) + (i * 0x08));
                    keys.Add(setInfo->Key1);
                }
            }
            else if (Type == NexTableType.DoubleKeyed)
            {
                for (int i = 0; i < numSets; i++)
                {
                    NexDataFile2KSetInfo* setInfo = (NexDataFile2KSetInfo*)(typeHeader + (*(int*)typeHeader) + (i * 0x0C));
                    keys.Add(setInfo->Key1);
                }
            }
            else if (Type == NexTableType.TripleKeyed)
            {
                for (int i = 0; i < numSets; i++)
                {
                    NexDataFile3KSetInfo* setInfo = (NexDataFile3KSetInfo*)(typeHeader + (*(int*)typeHeader) + (i * 0x14));
                    keys.Add(setInfo->Key1);
                }
            }

            return keys;
        }
    }

    public IReadOnlyList<NexSubSetInfo> GetSubSetInfos(uint key1)
    {
        if (Type != NexTableType.DoubleKeyed && Type != NexTableType.TripleKeyed)
            throw new InvalidOperationException("GetSubSetInfos is only valid for double or triple-keyed tables.");

        List<NexSubSetInfo> infos = [];
        unsafe
        {
            if (Type == NexTableType.DoubleKeyed)
            {
                NexDataFile2KSetInfo* setInfo = _hooks.NexDataFileFindK2SetInfoFunction(_tableInstance->FileHandle->Buffer + 0x20, key1);
                if (setInfo is null)
                    return infos;

                for (uint i = 0; i < setInfo->ArrayLength; i++)
                {
                    NexDataFile2KSetRowInfo* subsetInfo = (NexDataFile2KSetRowInfo*)((nint)setInfo + setInfo->RowArrayOffset + (i * 0x0C));
                    infos.Add(new NexSubSetInfo(key1, subsetInfo->Key2));
                }
            }
            else if (Type == NexTableType.TripleKeyed)
            {
                NexDataFile3KSetInfo* setInfo = _hooks.NexDataFileFindK3SetInfoFunction(_tableInstance->FileHandle->Buffer + 0x20, key1);
                if (setInfo is null)
                    return infos;

                for (uint i = 0; i < setInfo->NumSubRows; i++)
                {
                    NexDataFile3KSubsetInfo* subsetInfo = (NexDataFile3KSubsetInfo*)((nint)setInfo + setInfo->RowSubSetOffset + (i * 0x14));
                    infos.Add(new NexSubSetInfo(key1, subsetInfo->Key2));
                }
            }
        }

        return infos;
    }

    public NexSubSetInfo? GetSubSetInfoByIndex(uint key1, uint index)
    {
        if (Type != NexTableType.DoubleKeyed && Type != NexTableType.TripleKeyed)
            throw new InvalidOperationException("GetSubSetInfoByIndex is only valid for double or triple-keyed tables.");

        unsafe
        {
            if (Type == NexTableType.DoubleKeyed)
            {
                NexDataFile2KSetInfo* setInfo = _hooks.NexDataFileFindK2SetInfoFunction(_tableInstance->FileHandle->Buffer + 0x20, key1);
                if (setInfo is null)
                    return null;

                if (index > setInfo->ArrayLength - 1)
                    throw new IndexOutOfRangeException($"GetSubSetInfoByIndex: index out of range. num rows for key1: {setInfo->ArrayLength}, index: {index}");

                NexDataFile2KSetRowInfo* subsetInfo = (NexDataFile2KSetRowInfo*)((nint)setInfo + setInfo->RowArrayOffset + (index * 0x0C));
                return new NexSubSetInfo(key1, subsetInfo->Key2);
            }
            else if (Type == NexTableType.TripleKeyed)
            {
                NexDataFile3KSetInfo* setInfo = _hooks.NexDataFileFindK3SetInfoFunction(_tableInstance->FileHandle->Buffer + 0x20, key1);
                if (setInfo is null)
                    return null;

                if (index > setInfo->NumSubRows - 1)
                    throw new IndexOutOfRangeException($"GetSubSetInfoByIndex: index out of range. num rows for key1: {setInfo->NumSubRows}, index: {index}");

                NexDataFile3KSubsetInfo* subsetInfo = (NexDataFile3KSubsetInfo*)((nint)setInfo + setInfo->RowSubSetOffset + (index * 0x14));
                return new NexSubSetInfo(key1, subsetInfo->Key2);
            }
        }

        return null;
    }


    public NexTripleKeyedSubSetRowInfo GetTripleKeyedSubSetRowInfoByIndex(uint key1, uint key2, uint index)
    {
        if (Type != NexTableType.TripleKeyed)
            throw new InvalidOperationException("GetTripleKeyedSubSetInfos only valid for triple-keyed tables.");

        NexSetResult setResult = new NexSetResult();
        unsafe
        {
            _hooks.NexGetK3SetCountForType3Function(_tableInstance, &setResult, key1, key2);
            if (index > setResult.Count - 1)
                throw new IndexOutOfRangeException($"GetTripleKeyedSubSetRowInfo: index out of range. num rows: {setResult.Count}, index: {index}");

            Nex3KRowInfo* rowInfo = (Nex3KRowInfo*)(&setResult.Rows[index]);
            return new NexTripleKeyedSubSetRowInfo(rowInfo->Key1, rowInfo->Key2, rowInfo->Key3);
        }
    }

    public IReadOnlyList<NexTripleKeyedSubSetRowInfo> GetTripleKeyedSubSetRowInfos(uint key1, uint key2)
    {
        if (Type != NexTableType.TripleKeyed)
            throw new InvalidOperationException("GetTripleKeyedSubSetInfos is only valid for triple-keyed tables.");

        List<NexTripleKeyedSubSetRowInfo> infos = [];
        NexSetResult setResult = new NexSetResult();
        unsafe
        {
            _hooks.NexGetK3SetCountForType3Function(_tableInstance, &setResult, key1, key2);
            for (int i = 0; i < setResult.Count; i++)
            {
                Nex3KRowInfo* rowInfo = (Nex3KRowInfo*)(&setResult.Rows[i])->RowInfo;
                infos.Add(new NexTripleKeyedSubSetRowInfo(rowInfo->Key1, rowInfo->Key2, rowInfo->Key3));
            }
        }

        return infos;
    }


    public INexRow? GetRow(uint key1, uint key2 = 0, uint key3 = 0)
    {
        unsafe
        {
            switch (Type)
            {
                case NexTableType.SingleKeyed:
                    {
                        NexRowInstance* rowInstance = _hooks.NexSearchRow1KFunction(_tableInstance, key1);
                        if (rowInstance is null)
                            return null;

                        return GetNexRow1K(rowInstance);
                    }

                case NexTableType.DoubleKeyed:
                    {
                        NexRowInstance* rowInstance = _hooks.NexSearchRow2KFunction(_tableInstance, key1, key2);
                        if (rowInstance is null)
                            return null;

                        return GetNexRow2K(rowInstance);
                    }

                case NexTableType.TripleKeyed:
                    {
                        NexRowInstance* rowInstance = _hooks.NexSearchRow3KFunction(_tableInstance, key1, key2, key3);
                        if (rowInstance is null)
                            return null;

                        return GetNexRow3K(rowInstance);
                    }

            }
        }

        return null;
    }

    public INexRow? GetRowByIndex(uint index)
    {
        if (index > GetNumRows())
            throw new IndexOutOfRangeException($"GetRowByIndex: index out of range. num rows: {GetNumRows()}, index: {index}");

        unsafe
        {
            NexRowInstance* rowInstance = &_tableInstance->RowInfos[index];
            switch (Type)
            {
                case NexTableType.SingleKeyed:
                    return GetNexRow1K(rowInstance);
                case NexTableType.DoubleKeyed:
                    return GetNexRow2K(rowInstance);
                case NexTableType.TripleKeyed:
                    return GetNexRow3K(rowInstance);
            }
        }

        return null;
    }

    public INexRow? GetRowByIndex(uint key1, uint index)
    {
        if (Type != NexTableType.DoubleKeyed)
            throw new InvalidOperationException("GetRowByIndex with 1 key + index is only valid for double-keyed tables.");

        unsafe
        {
            NexSetResult setResult = new NexSetResult();

            _hooks.NexGetK2SetCountForType2Function(_tableInstance, &setResult, key1);
            if (index > setResult.Count - 1)
                throw new IndexOutOfRangeException($"GetRowByIndex: index out of range. num rows: {setResult.Count}, index: {index}");

            NexRowInstance* rowInstance = &setResult.Rows[index];
            return GetNexRow2K(rowInstance);
        }
    }

    public INexRow? GetRowByIndex(uint key1, uint key2, uint index)
    {
        if (Type != NexTableType.TripleKeyed)
            throw new InvalidOperationException("GetRowByIndex with 2 keys + index is only valid for triple-keyed tables.");

        unsafe
        {
            if (_hooks.NexGetK3SetCountForType3Function is null)
                throw new NotSupportedException("GetRowByIndex(uint key1, uint key2, uint index) is not supported as no hook for NexGetK3SetCountForType3Function was found.");

            NexSetResult setResult = new NexSetResult();
            _hooks.NexGetK3SetCountForType3Function(_tableInstance, &setResult, key1, key2);

            if (index > setResult.Count - 1)
                throw new IndexOutOfRangeException($"GetRowByIndex: index out of range. num rows: {setResult.Count}, index: {index}");

            NexRowInstance* rowInstance = &setResult.Rows[index];
            return GetNexRow3K(rowInstance);
        }
    }

    private unsafe NexRow? GetNexRow1K(NexRowInstance* rowInstance)
    {
        var rowInfo1k = (Nex1KRowInfo*)rowInstance->RowInfo;
        byte* rowData = GetRowData(rowInfo1k->Key1, 0, 0);
        if (rowData is null)
            return null;

        return new NexRow(rowInfo1k->Key1, 0, 0, rowData);
    }

    private unsafe NexRow? GetNexRow2K(NexRowInstance* rowInstance)
    {
        var rowInfo2k = (Nex2KRowInfo*)rowInstance->RowInfo;
        byte* rowData = GetRowData(rowInfo2k->Key1, rowInfo2k->Key2, 0);
        if (rowData is null)
            return null;

        return new NexRow(rowInfo2k->Key1, rowInfo2k->Key2, 0, rowData);
    }

    private unsafe NexRow? GetNexRow3K(NexRowInstance* rowInstance)
    {
        var rowInfo3k = (Nex3KRowInfo*)rowInstance->RowInfo;
        byte* rowData = GetRowData(rowInfo3k->Key1, rowInfo3k->Key2, rowInfo3k->Key3);
        if (rowData is null)
            return null;

        return new NexRow(rowInfo3k->Key1, rowInfo3k->Key2, rowInfo3k->Key3, rowData);
    }

    private unsafe byte* GetRowData(uint key1, uint key2, uint key3)
    {
        unsafe
        {
            switch (Type)
            {
                case NexTableType.SingleKeyed:
                    {
                        NexRowInstance* row = _hooks.NexSearchRow1KFunction(_tableInstance, key1);
                        if (row is null)
                            return null;

                        return _hooks.NexGetRowDataFunction(row);
                    }

                case NexTableType.DoubleKeyed:
                    {
                        NexRowInstance* row = _hooks.NexSearchRow2KFunction(_tableInstance, key1, key2);
                        if (row is null)
                            return null;

                        return _hooks.NexGetRowDataFunction(row);
                    }

                case NexTableType.TripleKeyed:
                    {
                        NexRowInstance* row = _hooks.NexSearchRow3KFunction(_tableInstance, key1, key2, key3);
                        if (row is null)
                            return null;

                        return _hooks.NexGetRowDataFunction(row);
                    }

            }
        }

        return null;
    }
}
