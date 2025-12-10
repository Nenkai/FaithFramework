namespace FF16Framework.Interfaces.Nex;

/// <summary>
/// Managed interface for accessing nex data. This time with methods to get tables by raw id.
/// </summary>
public interface INextExcelDBApiManagedV2 : INextExcelDBApiManaged
{
    /// <summary>
    /// Gets the physical number of rows for a table.
    /// </summary>
    /// <param name="tableId">Table id.</param>
    /// <returns></returns>
    uint GetMainRowCount(uint tableId);

    /// <summary>
    /// Gets a table.
    /// </summary>
    /// <param name="tableId">Table id.</param>
    /// <returns>Table instance. Returns null if not found.</returns>
    INexTable? GetTable(uint tableId);

    /// <summary>
    /// Returns whether the provided table is loaded.
    /// </summary>
    /// <param name="tableId">Table id.</param>
    /// <returns></returns>
    bool IsTableLoaded(uint tableId);
}