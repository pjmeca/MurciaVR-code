using Microsoft.Maps.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// Esta clase se encarga de la gestión de inyección de dependencias.
/// </summary>
public class Installer : MonoInstaller
{
    public MapRenderer mapRenderer;

    public override void InstallBindings()
    {
        Container.Bind<ICalidadDelAireService>()
            .FromMethod(CreateCalidadDelAireService)
            .AsSingle();

        Container.BindInstance(mapRenderer)
            .AsSingle();
    }

    private ICalidadDelAireService CreateCalidadDelAireService(InjectContext context)
    {
        return CalidadDelAireCARMService.Instance;
    }
}
