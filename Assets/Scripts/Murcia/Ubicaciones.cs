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
    [Inject]
    private readonly GameObject Jugador;

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
    // Almacena las alturas iniciales al teletransportar
    private readonly float[] _alturas =
    {
        7.12055f,   // Catedral
        20.83703f,  // FIUM
        6.170428f,  // Romea
        6.170428f,  // Plaza de las Flores
        7.04277f,   // Plaza de Toros
        7.04277f,   // Estadio La Condomina
        14.9816f    // El Corte Inglés
    };

    void Start()
    {
        DataHolder dataHolder = FindObjectOfType<DataHolder>();
        Ubicacion = dataHolder.UbicacionInicial;

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

        // Convertimos las coordenadas a un punto de Unity
        var point = mapRenderer.TransformLatLonAltToWorldPoint(new LatLonAlt(_latlons[index].LatitudeInDegrees, _latlons[index].LongitudeInDegrees, 0));

        Jugador.transform.position = new Vector3(point.x, _alturas[index], point.z);

        Eventos.LanzarLoadingEvent();
    }
}
