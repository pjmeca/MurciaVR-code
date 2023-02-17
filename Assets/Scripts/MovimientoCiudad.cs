//
// Actualiza la posición y las coordenadas del mapa (la región renderizada por Bing Maps SDK)
// para que se mueva con el jugador.
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;

public class MovimientoCiudad : MonoBehaviour
{
    public GameObject jugador;
    private float velocidad;
    [SerializeField]
    public MapRenderer mapRenderer;

    //[Range(0.00001f, 0.005f)]
    public float precision = 0.0001f;

    void Start()
    {
        if (mapRenderer == null)
        {
            mapRenderer = GameObject.Find("Map").GetComponent<MapRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Comprobaciones previas
        ActualizarVelocidad();

        // Obtener la posición del jugador
        var posX = jugador.transform.position.x;
        var posZ = jugador.transform.position.z;

        // Calcular la diferencia respecto a la última posición
        var difX = posX - transform.position.x;
        var difZ = posZ - transform.position.z;

        // Mover el centro del mapa a esa posición
        var temp = new Vector3(posX, transform.position.y, posZ);
        transform.position = temp;

        // Actualizar las coordenadas del mapa para mostrar el desplazamiento
        var zoom = mapRenderer.ZoomLevel;
        MapChanger(
            (float)(mapRenderer.Center.LatitudeInDegrees + difZ * precision * Time.deltaTime * velocidad), 
            (float)(mapRenderer.Center.LongitudeInDegrees + difX * precision * Time.deltaTime * velocidad), 
            zoom);
    }

    // Actualiza el valor de la velocidad de movimiento del jugador
    public void ActualizarVelocidad()
    {
        Transform childTrans = jugador.transform.FindChildRecursive("LeftHandAnchor");
        velocidad = childTrans.gameObject.GetComponent<JoystickLocomotion>().speed;
    }

    // https://seirios48.medium.com/whats-is-microsoft-map-sdk-can-6a7521be3c32
    public void MapChanger(float lat, float lon, float zoom)
    {
        mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(lat, lon), zoom), MapSceneAnimationKind.None);
    }
}
