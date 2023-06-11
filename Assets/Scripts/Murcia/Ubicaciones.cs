using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using UnityEngine;
using Zenject;

public class Ubicaciones : MonoBehaviour
{
    [Inject]
    private MapRenderer mapRenderer;

    [SerializeField]
    public Ubicacion ubicacion = Ubicacion.Catedral;
    private Ubicacion ultimaUbicacion;
    public enum Ubicacion
    {
        Catedral,
        FIUM,
        Romea,
        PlazaDeLasFlores,
        PlazaDeToros,
        EstadioLaCondomina,
        ElCorteIngles
    }
    public LatLon[] latlons =
    {
        new LatLon(37.983832, -1.129289), // Catedral
        new LatLon(38.023740, -1.173636), // FIUM
        new LatLon(37.986893, -1.130631), // Romea
        new LatLon(37.984810, -1.133141), // Plaza de las Flores
        new LatLon(37.985201, -1.122540), // Plaza de Toros
        new LatLon(37.986060, -1.121298), // Estadio La Condomina
        new LatLon(37.989306, -1.132552) // El Corte Inglés
        
    };

    // Start is called before the first frame update
    void Start()
    {
        actualizarUbicacion();
    }

    // Update is called once per frame
    void Update()
    {
        if(ultimaUbicacion != ubicacion)
        {
            actualizarUbicacion();
        }
    }

    private void actualizarUbicacion()
    {
        ultimaUbicacion = ubicacion;
        int index = Array.IndexOf(Enum.GetValues(ubicacion.GetType()), ubicacion);
        mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(latlons[index].LatitudeInDegrees, latlons[index].LongitudeInDegrees), mapRenderer.ZoomLevel), MapSceneAnimationKind.None);
    }
}
