using FF16Framework.Interfaces.Nex.Structures;

namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Low-level nex/nxd runtime interface/api.
/// </summary>
public unsafe interface INextExcelDBApi
{
    /// <summary>
    /// Whether the database is initialized and ready for use.
    /// </summary>
    bool Initialized { get; }

    /// <summary>
    /// Event fired when the game has initialized the nex tables.
    /// </summary>
    event NexLoadedEvent OnNexLoaded;

    /// <summary>
    /// Gets a table instance.
    /// </summary>
    /// <param name="table">Table type.</param>
    /// <returns>Table instance. Returns nullptr if not found.</returns>
    NexTableInstance* GetTable(NexTableIds table);

    /// <summary>
    /// Gets the number of sets (key1's) in a table, can be used to get the number of rows in a single-keyed table.
    /// </summary>
    /// <param name="table">Table type.</param>
    /// <returns>Row count.</returns>
    int GetSetCount(NexTableIds table);

    /// <summary>
    /// Gets row data directly.
    /// </summary>
    /// <returns>Row data pointer. Returns nullptr if not found.</returns>
    byte* GetRow(NexTableInstance* table, int key1);

    /// <summary>
    /// Gets row data directly.
    /// </summary>
    /// <returns>Row data pointer. Returns nullptr if not found.</returns>
    byte* GetRow(NexTableInstance* table, int key1, int key2);

    /// <summary>
    /// Searches for a row. Use <see cref="GetRowData(NexRowInstance*)"/> for then getting the row data.
    /// </summary>
    /// <returns>Row instance pointer. Returns nullptr if not found.</returns>
    NexRowInstance* SearchRow(NexTableInstance* table, int key1);

    /// <summary>
    /// Searches for a row. Use <see cref="GetRowData"/> for then getting the row data.
    /// </summary>
    /// <returns>Row instance pointer. Returns nullptr if not found.</returns>
    NexRowInstance* SearchRow(NexTableInstance* table, int key1, int key2);

    /// <summary>
    /// Searches for a row. Use <see cref="GetRowData"/> for then getting the row data.
    /// </summary>
    /// <returns>Row instance pointer. Returns nullptr if not found.</returns>
    NexRowInstance* SearchRow(NexTableInstance* table, int key1, int key2, int key3);

    /// <summary>
    /// Gets a double-keyed set info for the provided key, intended for double-keyed tables.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="result"></param>
    /// <param name="key1"></param>
    void GetDoubleKeyedSetCount(NexTableInstance* table, NexSetResult* result, int key1);

    /// <summary>
    /// Gets a triple-keyed set info for the provided key1 and key2, intended for triple-keyed tables.
    /// </summary>
    /// <param name="table"></param>
    /// <param name="result"></param>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    void GetTripleKeyedSubSetCount(NexTableInstance* table, NexSetResult* result, int key1, int key2);

    /// <summary>
    /// Gets the row data for a row instance.
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    public byte* GetRowData(NexRowInstance* row);

    /// <summary>
    /// Gets row ids.
    /// </summary>
    /// <param name="row">Row instance.</param>
    /// <param name="keys">Returned ids/keys.</param>
    /// <param name="numIds">Should never be more than 3 (triple keyed).</param>
    /// <returns></returns>
    public bool GetRowKeys(NexRowInstance* row, int* keys, int numIds);

    /// <summary>
    /// Returns whether the provided table is loaded.
    /// </summary>
    /// <param name="table"></param>
    /// <returns></returns>
    public bool IsTableLoaded(NexTableInstance* table);
}

public delegate void NexLoadedEvent();