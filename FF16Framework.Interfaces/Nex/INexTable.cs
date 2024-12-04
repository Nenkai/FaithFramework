namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Nex table.
/// </summary>
public interface INexTable
{
    /// <summary>
    /// Base row id for this table.
    /// </summary>
    uint BaseRowId { get; }

    /// <summary>
    /// Table id of this table.
    /// </summary>
    NexTableIds TableId { get; }

    /// <summary>
    /// Type of table.
    /// </summary>
    NexTableType Type { get; }

    /// <summary>
    /// Gets the number of physical rows in the table (not the count of key1's)
    /// </summary>
    /// <returns></returns>
    uint GetNumRows();

    /// <summary>
    /// Gets the number of main sets in the table (not the count of actual physical rows).
    /// </summary>
    /// <returns></returns>
    uint GetNumSets();

    /// <summary>
    /// Gets the key for the specified main row.
    /// </summary>
    /// <param name="key1"></param>
    /// <returns>key.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    uint GetMainKeyByIndex(uint index);

    /// <summary>
    /// Gets the main key/key1s. <br></br>
    /// Note that this may not match the number of physical rows, it could be more or less due to empty sets.
    /// </summary>
    /// <param name="key1"></param>
    /// <returns>Key1's.</returns>
    IReadOnlyList<uint> GetMainKeys();

    /// <summary>
    /// Gets the number of sub-sets/entries in a set, for double/triple keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <returns></returns>
    uint GetSubSetCount(uint key1);

    /// <summary>
    /// Gets the key information about a sub-set in a set by index, for double/triple-keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <returns>null if not found.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    NexSubSetInfo? GetSubSetInfoByIndex(uint key1, uint index);

    /// <summary>
    /// Gets the key information about sub-sets in a set, for double/triple-keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <returns>Empty list if not found/invalid.</returns>
    IReadOnlyList<NexSubSetInfo> GetSubSetInfos(uint key1);

    /// <summary>
    /// Gets the key information about a row in a sub-set by index, for triple keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <returns>null if not found.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    NexTripleKeyedSubSetRowInfo GetTripleKeyedSubSetRowInfoByIndex(uint key1, uint key2, uint index);

    /// <summary>
    /// Gets the key information for all rows in a sub-set, for triple keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    /// <returns>null if not found.</returns>
    IReadOnlyList<NexTripleKeyedSubSetRowInfo> GetTripleKeyedSubSetRowInfos(uint key1, uint key2);

    /// <summary>
    /// Gets a row.
    /// </summary>
    /// <param name="key1">Key1.</param>
    /// <param name="key2">Key2. Leave 0 for single-keyed tables.</param>
    /// <param name="key3">Key3. Leave 0 for single or double keyed tables.</param>
    /// <returns>null if not found/invalid.</returns>
    INexRow? GetRow(uint key1, uint key2 = 0, uint key3 = 0);

    /// <summary>
    /// Gets a row by absolute index/row number, regardless of sets.
    /// </summary>
    /// <param name="index"></param>
    /// <returns>null if not found/invalid.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    INexRow? GetRowByIndex(uint index);

    /// <summary>
    /// Gets a row from a set by index, for double keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="index"></param>
    /// <returns>null if not found/invalid.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    INexRow? GetRowByIndex(uint key1, uint index);

    /// <summary>
    /// Gets a row from a set's sub-set by index, for triple keyed tables.
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="index"></param>
    /// <returns>null if not found/invalid.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    INexRow? GetRowByIndex(uint key1, uint key2, uint index);
}

public enum NexTableType
{
    SingleKeyed = 1,
    DoubleKeyed = 2,
    TripleKeyed = 3,
}