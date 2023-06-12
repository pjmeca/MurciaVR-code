using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using UnityEngine;
using Zenject;

/// <summary>
/// Actualiza la ubicación del jugador en base a un enumerado de ubicaciones preestablecidas.
/// </summary>
public class Ubicaciones : MonoBehaviour
{
    [Inject]
    private readonly MapRenderer mapRenderer;

    public UbicacionesEnum Ubicacion = UbicacionesEnum.Catedral;
    private UbicacionesEnum ultimaUbicacion;
    public enum UbicacionesEnum
    {
        Catedral,
        FIUM,
        Romea,
        PlazaDeLasFlores,
        PlazaDeToros,
        EstadioLaCondomina,
        ElCorteIngles
    }
    private readonly LatLon[] _latlons =
    {
        new LatLon(37.983832, -1.129289), // Catedral
        new LatLon(38.023740, -1.173636), // FIUM
        new LatLon(37.986893, -1.130631), // Romea
        new LatLon(37.984810, -1.133141), // Plaza de las Flores
        new LatLon(37.985201, -1.122540), // Plaza de Toros
        new LatLon(37.986060, -1.121298), // Estadio La Condomina
        new LatLon(37.989306, -1.132552)  // El Corte Inglés
        
    };

    void Start()
    {
        ActualizarUbicacion();
    }

    void Update()
    {
        if (ultimaUbicacion != Ubicacion)
        {
            ActualizarUbicacion();
        }
    }

    private void ActualizarUbicacion()
    {
        ultimaUbicacion = Ubicacion;
        int index = Array.IndexOf(Enum.GetValues(Ubicacion.GetType()), Ubicacion);
        mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(_latlons[index].LatitudeInDegrees, _latlons[index].LongitudeInDegrees), mapRenderer.ZoomLevel), MapSceneAnimationKind.None);
    }
}
