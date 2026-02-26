using System;
using System.Collections.Generic;

namespace FF16Framework.Interfaces.GameApis.Magic;

/// <summary>
/// Public interface for the MagicWriter service.
/// <para>
/// MagicWriter manages persistent modifications to .magic files.
/// It listens for resource load events and automatically applies registered modifications
/// when the corresponding .magic file is loaded or reloaded by the game.
/// </para>
/// <para>
/// This enables mods to register their modifications once and have them automatically
/// applied whenever the game loads (or reloads) the magic files.
/// </para>
/// <para>Usage:</para>
/// <list type="number">
/// <item><description>Create your spell modifications using IMagicApi.CreateSpell()</description></item>
/// <item><description>Register them with IMagicWriter.Register()</description></item>
/// <item><description>The MagicWriter will automatically apply them when the file loads</description></item>
/// <item><description>Optionally unregister when your mod is unloaded</description></item>
/// </list>
/// </summary>
/// <remarks>
/// Example of code using the MagicWriter API:
/// <code><![CDATA[
/// var builder = magicApi.CreateSpell(214) // Dia's Magic ID
///     .SetProperty(4338, 35, 35, 10.0f) // Modifies the duration of the projectile's linear trajectory to 10 seconds
///
/// // The modifications will be applied automatically whenever the game loads or reloads "chara/c1001/magic/c1001.magic"
/// var handle = magicWriter.Register("my.mod", builder, "c1001"); // c1001 is the character ID for Clive
/// 
/// // Later, to stop applying changes on reload
/// magicWriter.Unregister(handle);
/// // This could be useful if for example your mod changes accesories effects to modify spells and when the item is unequipped 
/// // you want to stop applying those modifications on reload to prevent making the accessory's effects linger after it's unequipped.
/// ]]></code>
/// </remarks>
public interface IMagicWriter
{
    /// <summary>
    /// Event raised after modifications are applied to a magic file.
    /// Parameters: (magicFilePath, numberOfModifications)
    /// </summary>
    event Action<string, int>? OnModificationsApplied;
    
    /// <summary>
    /// Total count of registered modification spells.
    /// </summary>
    int RegisteredCount { get; }
    
    /// <summary>
    /// Registers modifications from an IMagicBuilder to be applied to a magic file.
    /// The modifications will be applied automatically when the file is loaded.
    /// If the file is already loaded, the modifications are applied immediately.
    /// </summary>
    /// <param name="modId">The ID of the mod registering the modifications (for logging/debugging).</param>
    /// <param name="builder">The builder containing the modifications.</param>
    /// <param name="characterId">The character ID (folder name). Default is "c1001" for Clive.</param>
    /// <param name="magicFileName">Optional magic file name. If null, uses characterId as filename.</param>
    /// <returns>A handle that can be used to unregister the modifications.</returns>
    /// <remarks>
    /// Example paths:
    /// - characterId="c1001", magicFileName=null → "chara/c1001/magic/c1001.magic"
    /// - characterId="c1001", magicFileName="c1001_101" → "chara/c1001/magic/c1001_101.magic"
    /// </remarks>
    MagicWriterHandle Register(
        string modId,
        IMagicBuilder builder,
        string characterId = "c1001",
        string? magicFileName = null);
    
    /// <summary>
    /// Unregisters a set of modifications.
    /// Note: This does NOT undo the modifications already applied in memory.
    /// The original file will be restored when the game reloads it.
    /// </summary>
    /// <param name="handle">The handle returned from Register.</param>
    /// <returns>True if the handle was found and removed.</returns>
    bool Unregister(MagicWriterHandle handle);
    
    /// <summary>
    /// Unregisters all modifications from a specific mod.
    /// </summary>
    /// <param name="modId">The mod ID to unregister all modifications for.</param>
    /// <returns>The number of modification sets unregistered.</returns>
    int UnregisterAll(string modId);
    
    /// <summary>
    /// Gets all registered modification sets for a specific mod.
    /// </summary>
    /// <param name="modId">The mod ID to query modifications for.</param>
    /// <returns>List of registered modification info.</returns>
    IReadOnlyList<IRegisteredModificationInfo> GetRegisteredModifications(string modId);
    
    // ========================================
    // NEW MAGIC ID REGISTRATION
    // ========================================
    
    /// <summary>
    /// Registers a brand-new magic ID with an auto-assigned ID in the modded range (>30000).
    /// The builder's MagicId will be set internally to the allocated ID.
    /// The new entry is created in the .magic file when it loads.
    /// </summary>
    /// <param name="modId">The ID of the mod registering the new magic.</param>
    /// <param name="builder">The builder containing the spell definition (its MagicId will be overwritten).</param>
    /// <param name="characterId">The character ID (folder name). Default is "c1001" for Clive.</param>
    /// <param name="magicFileName">Optional magic file name. If null, uses characterId as filename.</param>
    /// <returns>A registration result containing the assigned magic ID and writer handle.</returns>
    MagicRegistration RegisterNewMagicId(
        string modId,
        IMagicBuilder builder,
        string characterId = "c1001",
        string? magicFileName = null);
    
    /// <summary>
    /// Registers a brand-new magic ID with a specific ID chosen by the caller.
    /// The ID must be in the modded range (>30000) and must not already be reserved.
    /// The builder's MagicId will be set internally to the requested ID.
    /// The new entry is created in the .magic file when it loads.
    /// </summary>
    /// <param name="modId">The ID of the mod registering the new magic.</param>
    /// <param name="magicId">The specific magic ID to reserve. Must be >30000.</param>
    /// <param name="builder">The builder containing the spell definition (its MagicId will be overwritten).</param>
    /// <param name="characterId">The character ID (folder name). Default is "c1001" for Clive.</param>
    /// <param name="magicFileName">Optional magic file name. If null, uses characterId as filename.</param>
    /// <returns>A registration result indicating success and the writer handle.</returns>
    MagicRegistration RegisterNewMagicId(
        string modId,
        int magicId,
        IMagicBuilder builder,
        string characterId = "c1001",
        string? magicFileName = null);
}

/// <summary>
/// Unique identifier for a registered modification set.
/// Used to track and unregister modifications from the MagicWriter.
/// </summary>
public readonly struct MagicWriterHandle : IEquatable<MagicWriterHandle>
{
    private readonly Guid _id;
    
    /// <summary>
    /// Creates a new handle with the specified GUID.
    /// </summary>
    public MagicWriterHandle(Guid id) => _id = id;
    
    /// <inheritdoc/>
    public bool Equals(MagicWriterHandle other) => _id == other._id;
    
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MagicWriterHandle other && Equals(other);
    
    /// <inheritdoc/>
    public override int GetHashCode() => _id.GetHashCode();
    
    /// <summary>
    /// Equality operator.
    /// </summary>
    public static bool operator ==(MagicWriterHandle left, MagicWriterHandle right) => left.Equals(right);
    
    /// <summary>
    /// Inequality operator.
    /// </summary>
    public static bool operator !=(MagicWriterHandle left, MagicWriterHandle right) => !left.Equals(right);
    
    /// <summary>
    /// Returns an invalid handle.
    /// </summary>
    public static MagicWriterHandle Invalid => new(Guid.Empty);
    
    /// <summary>
    /// Returns true if this handle is valid.
    /// </summary>
    public bool IsValid => _id != Guid.Empty;
}

/// <summary>
/// Information about a registered modification set.
/// </summary>
public interface IRegisteredModificationInfo
{
    /// <summary>ID of the mod that registered this modification.</summary>
    string ModId { get; }
    
    /// <summary>The magic ID being modified.</summary>
    int MagicId { get; }
    
    /// <summary>Number of modifications in this set.</summary>
    int ModificationCount { get; }
}

/// <summary>
/// Result of registering a new magic ID via <see cref="IMagicWriter.RegisterNewMagicId"/>.
/// Contains the assigned magic ID and a writer handle to manage the registration.
/// </summary>
public readonly struct MagicRegistration : IEquatable<MagicRegistration>
{
    /// <summary>
    /// The magic ID that was assigned or reserved.
    /// </summary>
    public int MagicId { get; }
    
    /// <summary>
    /// The writer handle for managing (unregistering) this registration.
    /// </summary>
    public MagicWriterHandle Handle { get; }
    
    /// <summary>
    /// Whether the registration was successful.
    /// </summary>
    public bool IsValid { get; }
    
    /// <summary>
    /// Creates a valid registration result.
    /// </summary>
    public MagicRegistration(int magicId, MagicWriterHandle handle)
    {
        MagicId = magicId;
        Handle = handle;
        IsValid = handle.IsValid;
    }
    
    /// <summary>
    /// Returns an invalid (failed) registration.
    /// </summary>
    public static MagicRegistration Invalid => new(0, MagicWriterHandle.Invalid);
    
    /// <inheritdoc/>
    public bool Equals(MagicRegistration other) => MagicId == other.MagicId && Handle == other.Handle;
    
    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MagicRegistration other && Equals(other);
    
    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(MagicId, Handle);
    
    /// <summary>Equality operator.</summary>
    public static bool operator ==(MagicRegistration left, MagicRegistration right) => left.Equals(right);
    
    /// <summary>Inequality operator.</summary>
    public static bool operator !=(MagicRegistration left, MagicRegistration right) => !left.Equals(right);
    
    /// <inheritdoc/>
    public override string ToString() => IsValid ? $"MagicRegistration(Id={MagicId})" : "MagicRegistration(Invalid)";
}
