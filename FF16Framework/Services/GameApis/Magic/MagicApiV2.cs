using System.Text.Json;
using Reloaded.Hooks.Definitions;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using FF16Framework;
using FF16Framework.Interfaces.GameApis.Magic;
using FF16Framework.Interfaces.GameApis.Actor;
using FF16Framework.Services.GameApis.Actor;

namespace FF16Framework.Services.GameApis.Magic;

/// <summary>
/// Implementation of the public Magic API.
/// Provides a clean interface for casting magic spells and applying modifications.
/// </summary>
public class MagicApiV2 : IMagicApi, IDisposable
{
    private readonly ILogger _logger;
    private readonly string _modId;
    private readonly MagicCastingEngine _engine;

    internal MagicApiV2(ILogger logger, string modId, Config configuration, IStartupScanner scanner)
    {
        _logger = logger;
        _modId = modId;
        _engine = new MagicCastingEngine(logger, modId, configuration, scanner);
        
        _logger.WriteLine($"[{_modId}] [MagicApiV2] Initialized", _logger.ColorGreen);
    }

    // ========================================
    // INITIALIZATION (Internal)
    // ========================================
    
    internal void SetupScans(IStartupScanner scans, IReloadedHooks hooks)
    {
        _engine.SetupScans(scans, hooks);
    }
    
    internal void InitializeProcessor(IReloadedHooks hooks)
    {
        _engine.InitializeProcessor(hooks);
    }
    
    internal void SetCallbacks(Func<int>? getActiveEikon)
    {
        _engine.GetActiveEikon = getActiveEikon;
    }
    
    /// <summary>
    /// Sets the ActorApi for consolidated actor/player management.
    /// </summary>
    internal void SetActorApi(IActorApi actorApi)
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
    public bool Cast(int magicId, nint? sourceActor = null, nint? targetActor = null)
    {
        return CreateSpell(magicId).Cast(sourceActor, targetActor);
    }
    
    /// <inheritdoc/>
    public bool CastWithGameTarget(int magicId, nint? sourceActor = null)
    {
        var request = new MagicCastRequest
        {
            MagicId = magicId,
            SourceActor = sourceActor,
            // Don't set TargetActor - UseGameTarget will copy from game's targeting system
            UseGameTarget = true
        };
        return _engine.CastSpell(request);
    }
    
    /// <inheritdoc/>
    public nint GetLockedTarget()
    {
        return _engine.GetLockedTarget();
    }
    
    /// <inheritdoc/>
    public nint GetPlayerActor()
    {
        return _engine.GetPlayerActor();
    }
    
    /// <inheritdoc/>
    public void RegisterChargedShotHandler(Func<int, bool> handler)
    {
        _engine.OnChargedShotDetected = handler;
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
                _logger.WriteLine($"[{_modId}] [MagicApiV2] Failed to parse JSON", _logger.ColorRed);
                return null;
            }
            
            var builder = CreateSpell(config.MagicId);
            builder.ImportFromJson(json);
            return builder;
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modId}] [MagicApiV2] Failed to import JSON: {ex.Message}", _logger.ColorRed);
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
                _logger.WriteLine($"[{_modId}] [MagicApiV2] File not found: {filePath}", _logger.ColorRed);
                return null;
            }
            
            string json = File.ReadAllText(filePath);
            return ImportFromJson(json);
        }
        catch (Exception ex)
        {
            _logger.WriteLine($"[{_modId}] [MagicApiV2] Failed to load file: {ex.Message}", _logger.ColorRed);
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
    
    /// <summary>
    /// Updates the configuration.
    /// </summary>
    public void UpdateConfiguration(Config configuration)
    {
        _engine.UpdateConfiguration(configuration);
    }
    
    public void Dispose()
    {
        _engine.Dispose();
    }
}
