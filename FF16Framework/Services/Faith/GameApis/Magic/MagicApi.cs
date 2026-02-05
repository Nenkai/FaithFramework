using System.Text.Json;
using Reloaded.Mod.Interfaces;
using FF16Framework;
using FF16Framework.Faith.Hooks;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Interfaces.GameApis.Structs;
using FF16Framework.Services.Faith.GameApis.Actor;

namespace FF16Framework.Services.Faith.GameApis.Magic;

/// <summary>
/// Implementation of the public Magic API.
/// Provides a clean interface for casting magic spells and applying modifications.
/// </summary>
public class MagicApi : IMagicApi, IDisposable
{
    private readonly ILogger _logger;
    private readonly string _modId;
    private readonly MagicCastingEngine _engine;

    internal MagicApi(ILogger logger, string modId, FrameworkConfig frameworkConfig, MagicHooks magicHooks)
    {
        _logger = logger;
        _modId = modId;
        _engine = new MagicCastingEngine(logger, modId, frameworkConfig, magicHooks);
        
        _logger.WriteLine($"[{_modId}] [MagicApi] Initialized", _logger.ColorGreen);
    }

    // ========================================
    // INITIALIZATION (Internal)
    // ========================================
    
    /// <summary>
    /// Sets the ActorApi for consolidated actor/player management.
    /// </summary>
    internal void SetActorApi(ActorApi actorApi)
    {
        _engine.SetActorApi(actorApi);
    }
    
    // ========================================
    // IMagicApi Implementation
    // ========================================
    
    /// <inheritdoc/>
    public bool IsReady => _engine.IsReady;
    
    /// <inheritdoc/>
    public IMagicBuilder CreateSpell(int magicId)
    {
        return new MagicBuilder(magicId, _engine, _logger, _modId);
    }
    
    /// <inheritdoc/>
    public bool Cast(int magicId, ActorSelection source = ActorSelection.Player, ActorSelection target = ActorSelection.LockedTarget)
    {
        return CreateSpell(magicId).Cast(source, target);
    }    
    /// <inheritdoc/>
    public IMagicBuilder? ImportFromJson(string json)
    {
        try
        {
            var config = JsonSerializer.Deserialize<MagicSpellConfig>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            if (config == null)
            {
                _logger.WriteLine($"[{_modId}] [MagicApi] Failed to parse JSON", _logger.ColorRed);
                return null;
            }
            
            var builder = CreateSpell(config.MagicId);
            builder.ImportFromJson(json);
            return builder;
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modId}] [MagicApi] Failed to import JSON: {ex.Message}", _logger.ColorRed);
            return null;
        }
    }
    
    /// <inheritdoc/>
    public IMagicBuilder? ImportFromFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                _logger.WriteLine($"[{_modId}] [MagicApi] File not found: {filePath}", _logger.ColorRed);
                return null;
            }
            
            string json = File.ReadAllText(filePath);
            return ImportFromJson(json);
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modId}] [MagicApi] Failed to load file: {ex.Message}", _logger.ColorRed);
            return null;
        }
    }
    
    // ========================================
    // State Management
    // ========================================
    
    /// <summary>
    /// Resets the magic system state.
    /// Called when changing levels or reloading.
    /// </summary>
    public void Reset()
    {
        _engine.Reset();
    }
    
    public void Dispose()
    {
        _engine.Dispose();
    }
}
