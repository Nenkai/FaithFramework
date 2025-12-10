namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Managed interface for accessing nex data.
/// </summary>
public interface INextExcelDBApiManaged
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
    /// Gets the physical number of rows for a table.
    /// </summary>
    /// <param name="table">Table id.</param>
    /// <returns></returns>
    uint GetMainRowCount(NexTableIds table);

    /// <summary>
    /// Gets a table.
    /// </summary>
    /// <param name="tableId">Table type.</param>
    /// <returns>Table instance. Returns null if not found.</returns>
    INexTable? GetTable(NexTableIds tableId);

    /// <summary>
    /// Returns whether the provided table is loaded.
    /// </summary>
    /// <param name="tableId">Table id.</param>
    /// <returns></returns>
    bool IsTableLoaded(NexTableIds tableId);
}