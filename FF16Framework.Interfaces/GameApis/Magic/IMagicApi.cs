using System;
using FF16Framework.Interfaces.GameApis.Structs;

namespace FF16Framework.Interfaces.GameApis.Magic;

/// <summary>
/// Public interface for the Magic API, this API allows modder to cast magic spells whenever they want.
/// <para>
/// If you want to apply modifications to the spell you have to familiarize with the MagicBuilder class,
/// with which you can apply modifications to the spell's magic file structure and then cast the spell 
/// with those modifications applied on the fly.
/// </para>
/// <para>
/// And if you want to make those modifications permanent you have to use the MagicWriter class.
/// </para>
/// </summary>
/// <remarks>
/// Example of code using the Magic API:
/// <code><![CDATA[
/// // Simple dia cast from the player to the locked target.
/// bool success = magicApi.Cast(214);
/// 
/// // Tornado cast from an enemy (elemental) to the player.
/// bool success = magicApi.Cast(84, ActorSelection.LockedTarget, ActorSelection.Player);
/// 
/// // Create a builder for dia, modify duration to 10 seconds and cast it.
/// // The modification only applies when the spell is cast from this builder.
/// var builder = magicApi.CreateSpell(214).SetProperty(4338, 35, 35, 10.0);
/// builder.Cast();
/// 
/// // To make changes permanent, register the builder with the MagicWriter.
/// // Modifications will be applied whenever the game loads the file.
/// var handle = magicWriter.Register("my.mod.name", builder, "c1001");
/// 
/// // Unregister when you no longer want them applied on reload.
/// magicWriter.Unregister(handle);
/// 
/// // Or unregister all your mod's modifications at once:
/// magicWriter.UnregisterAll("my.mod.name");
/// ]]></code>
/// </remarks>
public interface IMagicApi
{
    /// Returns true if the magic system has captured the necessary game context to cast spells.
    bool IsReady { get; }
    
    /// <summary>
    /// Casts a spell using actor selection presets for source and target.
    /// </summary>
    /// <param name="magicId">The ID of the magic spell to cast.</param>
    /// <param name="source">The actor casting the spell. Defaults to Player.</param>
    /// <param name="target">The target actor. Defaults to LockedTarget (camera's soft/hard locked target).</param>
    /// <returns>True if the spell was cast successfully.</returns>
    bool Cast(int magicId, ActorSelection source = ActorSelection.Player, ActorSelection target = ActorSelection.LockedTarget);

    /// <summary>
    /// Creates a MagicBuilder object referencing the magic ID.
    /// </summary>
    /// <param name="magicId">The ID of the magic spell to cast.</param>
    /// <returns>A builder in charge of modifying the spell's magic file structure and also casting the spell.</returns>
    IMagicBuilder CreateSpell(int magicId);
    
    /// <summary>
    /// Loads a JSON string with all the spell modifications and instantiates a builder with those modifications ready to cast or register with the MagicWriter.
    /// </summary>
    /// <param name="json">JSON string containing spell modifications.</param>
    /// <returns>A builder configured with all the imported modifications, or null if parsing failed.</returns>
    IMagicBuilder? ImportFromJson(string json);
    
    /// <summary>
    /// Loads a JSON file with all the spell modifications and instantiates a builder with those modifications ready to cast or register with the MagicWriter.
    /// </summary>
    /// <param name="filePath">Path to the JSON file.</param>
    /// <returns>A builder configured with all the imported modifications, or null if loading failed.</returns>
    IMagicBuilder? ImportFromFile(string filePath);
}