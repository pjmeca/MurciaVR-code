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
    public GameObject Niebla;

    [SerializeField]
    public MapRenderer mapRenderer;

    // Perímetro máximo dentro del cual no se actualizará la posición del mapa
    [Range(0, 500)]
    public float perimetro = 100;
    private float difX = 0, difZ = 0;

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
        // Obtener la posición del jugador
        var posX = jugador.transform.position.x;
        var posZ = jugador.transform.position.z;

        // Calcular la diferencia respecto a la última posición
        difX += posX - transform.position.x;
        difZ += posZ - transform.position.z;

        //Debug.Log("posX: "+posX + " posZ: "+posZ+" difX:" + difX+" difZ:"+difZ);

        // Comprobar si se ha salido del perímetro
        if(!(Mathf.Abs(difX) < perimetro || Mathf.Abs(difZ) < perimetro))
        {
            // Mover el mapa
            MoverMapa(posX, posZ);

            // Reestablecer los acumuladores
            difX = difZ = 0;
        }

        // En cualquier caso, mover la niebla con el jugador
        if(Niebla != null) 
            Niebla.transform.position = new Vector3(posX, transform.position.y, posZ);
    }

    // Actualiza la posición del mapa
    public void MoverMapa(float posX, float posZ)
    {
        // Mover el centro del mapa a esa posición
        var temp = new Vector3(posX, transform.position.y, posZ);
        var latLon = mapRenderer.TransformWorldPointToLatLon(temp); // IMPORTANTE HACERLO ANTES DE MOVERLO
                                                                    // traduce de forma relativa: (0,0,0) -> transform.position

        transform.position = temp;

        // Actualizar las coordenadas del mapa para mostrar el desplazamiento
        var zoom = mapRenderer.ZoomLevel;
        MapChanger(latLon.LatitudeInDegrees, latLon.LongitudeInDegrees, zoom);
    }

    // Basado en: https://seirios48.medium.com/whats-is-microsoft-map-sdk-can-6a7521be3c32
    public void MapChanger(double lat, double lon, float zoom)
    {
        mapRenderer.SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(lat, lon), zoom), MapSceneAnimationKind.None);
    }
}
