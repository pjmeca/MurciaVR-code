using System;
using Microsoft.Maps.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// Esta clase se encarga de la gestión de inyección de dependencias a través de Zenject.
/// </summary>
public class Installer : MonoInstaller
{
    public MapRenderer MapRenderer;
    public GameObject Jugador;

    public override void InstallBindings()
    {
        Container.Bind<ICalidadDelAireService>()
            .FromMethod(() => CalidadDelAireCARMService.Instance)
            .AsSingle();

        if (MapRenderer == null)
            throw new NullReferenceException("No se ha establecido un valor para MapRenderer.");

        Container.BindInstance(MapRenderer)
            .AsSingle();

        if (Jugador == null) 
            throw new NullReferenceException("No se ha establecido un valor para Jugador.");

        Container.BindInstance(Jugador)
            .AsSingle();
    }
}
