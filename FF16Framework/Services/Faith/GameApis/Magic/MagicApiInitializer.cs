using Microsoft.Extensions.Hosting;
using FF16Framework.Services.Faith.GameApis.Actor;

namespace FF16Framework.Services.Faith.GameApis.Magic;

/// <summary>
/// Handles post-construction initialization of <see cref="MagicApi"/>.
/// Wires up the ActorApi dependency that can't be injected via constructor
/// due to the internal visibility of ActorApi.
/// </summary>
internal class MagicApiInitializer : IHostedService
{
    private readonly MagicApi _magicApi;
    private readonly ActorApi _actorApi;

    public MagicApiInitializer(MagicApi magicApi, ActorApi actorApi)
    {
        _magicApi = magicApi;
        _actorApi = actorApi;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _magicApi.SetActorApi(_actorApi);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
