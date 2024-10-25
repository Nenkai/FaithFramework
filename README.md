# ff16.utility.framework

Mod Framework for FFXVI using Reloaded-II.

Currently only Nex interfaces.

## Nex Interface

You should grab the [FF16Tools.Files](https://github.com/Nenkai/FF16Tools/) NuGet Package to be able to read rows.

First, grab a `INextExcelDBApi`:
```csharp
_nexApi = _modLoader.GetController<INextExcelDBApi>();
if (!_nexApi.TryGetTarget(out INextExcelDBApi nextExcelDBApi))
{
    _logger.WriteLine($"[{_modConfig.ModId}] Could not get INextExcelDBApi.");
    return;
}

```

Optionally, subscribe to an event when the game has loaded the nex database:
```csharp
nextExcelDBApi.OnNexLoaded += NextExcelDBApi_OnNexLoaded;
```

You can use this to apply nex changes immediately once the event fires.

> [!NOTE]
> Always ensure that the database is initialized before attempting to do any changes (especially if you are applying changes again from a configuration change through `ConfigurationUpdated`).

```csharp
if (!nextExcelDBApi.Initialized)
    return;
```

### Single-Keyed table fetching

```csharp
NexTableInstance* photoTable = nextExcelDBApi.GetTable(NexTableIds.photocameraparam);
if (photoTable is null)
    return;

// Grab a mapped layout from FF16Tools.Files.
NexTableLayout layout = TableMappingReader.ReadTableLayout("photocameraparam", new Version(1, 0, 0));

for (int i = 0; i < photoTable->NumRows; i++)
{
    NexRowInstance* rowInstance = nextExcelDBApi.SearchRow(photoTable, i);
    if (rowInstance is null)
    {
        _logger.WriteLine($"[{_modConfig.ModId}] Could not get photocameraparam row {i}, skipping", _logger.ColorRed);
        continue;
    }

    Nex1KRowInfo* rowInfo = (Nex1KRowInfo*)rowInstance->RowInfo;
    byte* rowData = nextExcelDBApi.GetRowData(rowInstance);

    // You have access to the row's data here. Make use of FF16Tools.Files's defined layout to read and edit it.
    *(float*)&rowData[layout.Columns["CollisionSphereRadius"].Offset] = 69.420f;
}
```

---

### Double-Keyed tables

```csharp
NexTableInstance* questSequence = nextExcelDBApi.GetTable(NexTableIds.questsequence);

// Get the number of main sets (k1's) in the table
int sets = nextExcelDBApi.GetSetCount(NexTableIds.questtodo);

NexSetResult setResult = new NexSetResult();
nextExcelDBApi.GetDoubleKeyedSetCount(questSequence, &setResult, 100);
for (int i = 0; i < setResult.Count; i++)
{
    NexRowInstance* rowInstance = &setResult.Rows[i];
    Nex2KRowInfo* rowInfo = (Nex2KRowInfo*)rowInstance->RowInfo;

    var rowData = nextExcelDBApi.GetRowData(rowInstance);
    // ...
}
```

---

### Triple-Keyed tables

```csharp
NexTableInstance* questtodo = nextExcelDBApi.GetTable(NexTableIds.questtodo);

// Get the number of main sets (k1's) in the table
int sets = nextExcelDBApi.GetSetCount(NexTableIds.questtodo);

if (questtodo is not null && questtodo->Type == (int)NexTableType.TripleKeyed)
{
    NexSetResult res = new NexSetResult();
    nextExcelDBApi.GetTripleKeyedSubSetCount(questtodo, &res, 100, 510001);

    for (int i = 0; i < res.Count; i++)
    {
        // Get row
        NexRowInstance* rowInstance = &res.Rows[i];
        Nex3KRowInfo* rowInfo = (Nex3KRowInfo*)rowInstance->RowInfo;
    }

    // Get keys
    NexRowInstance* row = nextExcelDBApi.SearchRow(questtodo, 31070, 100, 0);
    int* keys = (int*)Marshal.AllocHGlobal(4 * (nint)3);
    if (nextExcelDBApi.GetRowKeys(row, keys, 3))
    {
        // ...
    }
}
```
