# ff16.utility.framework

Mod Framework for FFXVI using Reloaded-II.

Currently only Nex interfaces.

## Nex Interface

You should grab the [FF16Tools.Files](https://github.com/Nenkai/FF16Tools/) NuGet Package to be able to read rows.

First, grab a `INextExcelDBApiManaged`:
```csharp
_nexApi = _modLoader.GetController<INextExcelDBApiManaged>();
if (!_nexApi.TryGetTarget(out INextExcelDBApiManaged nextExcelDBApi))
{
    _logger.WriteLine($"[{_modConfig.ModId}] Could not get INextExcelDBApi.");
    return;
}

```

Then, subscribe to an event when the game has loaded the nex database:
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

### Basic usage
```csharp
INexTable? photoTable = nextExcelDBApi.GetTable(NexTableIds.photocameraparam);
if (photoTable is null)
{
    // Handle error
}

// Get a specific row
INexRow? row = photoTable.GetRow(7);
if (row is null)
{
    // Handle error
}

// Iterate through rows/get keys
uint numSets = photoTable.GetNumSets();
for (uint i = 0; i < numSets; i++)
{
    uint key1 = photoTable.GetMainKeyByIndex(i);
    // ...
}

// Double keyed (example)
INexTable? gamemap = nextExcelDBApi.GetTable(NexTableIds.gamemap);
uint numSubSets = gamemap.GetSubSetCount(200000);
IReadOnlyList<NexSubSetInfo> allSubSets = photoTable.GetSubSetInfos(200000);
for (uint i = 0; i < numSubSets; i++)
{
    NexSubSetInfo? subSetInfo = gamemap.GetSubSetInfoByIndex(200000, i);
    if (subSetInfo is null)
    {
        // Handle error
    }
}

// Manipulate/Fetch row
float collisionSphereRadius = row.GetSingle((uint)layout.Columns["CollisionSphereRadius"].Offset);
row.SetSingle((uint)layout.Columns["CollisionSphereRadius"].Offset, 133.7f;
```

> [!NOTE]
> It is not yet possible to add/remove rows, access/manipulate structure arrays or edit strings.
> 
> `INextExcelDBApi` is still supported, but you should use the managed api instead to avoid dealing with pointers/unsafe code.
