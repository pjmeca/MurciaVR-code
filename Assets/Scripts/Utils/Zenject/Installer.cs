using System;
using Microsoft.Maps.Unity;
using Zenject;

/// <summary>
/// Esta clase se encarga de la gesti�n de inyecci�n de dependencias a trav�s de Zenject.
/// </summary>
public class Installer : MonoInstaller
{
    public MapRenderer MapRenderer;

    public override void InstallBindings()
    {
        Container.Bind<ICalidadDelAireService>()
            .FromMethod(() => CalidadDelAireCARMService.Instance)
            .AsSingle();

        if (MapRenderer == null)
            throw new NullReferenceException("No se ha establecido un valor para MapRenderer.");

        Container.BindInstance(MapRenderer)
            .AsSingle();
    }
}
