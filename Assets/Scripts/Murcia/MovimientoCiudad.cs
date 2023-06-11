//
// Actualiza la posición y las coordenadas del mapa (la región renderizada por Bing Maps SDK)
// para que se mueva con el jugador.
//

using UnityEngine;
using Microsoft.Maps.Unity;
using Microsoft.Geospatial;
using Unity.VisualScripting;
using Zenject;

public class MovimientoCiudad : MonoBehaviour
{
    public GameObject jugador;

    [Inject]
    private MapRenderer mapaCentral;

    // Perímetro máximo dentro del cual no se actualizará la posición del mapa
    [Range(0, 500)]
    public float perimetro = 100;
    private float difX = 0, difZ = 0;

    // Update is called once per frame
    void Update()
    {        
        // Obtener la posición del jugador
        var posX = jugador.transform.position.x;
        var posZ = jugador.transform.position.z;

        // Calcular la diferencia respecto a la última posición
        difX += posX - mapaCentral.transform.position.x;
        difZ += posZ - mapaCentral.transform.position.z;

        // Debug.Log("posX: "+posX + " posZ: "+posZ+" difX:" + difX+" difZ:"+difZ);

        // Comprobar si se ha salido del perímetro
        if(Mathf.Abs(difX) + Mathf.Abs(difZ) > perimetro)
        {
            var difMapX = transform.position.x - mapaCentral.transform.position.x;
            var difMapZ = transform.position.z - mapaCentral.transform.position.z;

            // Mover el mapa
            MoverMapa(posX + difMapX, posZ + difMapZ);

            // Reestablecer los acumuladores
            difX = difZ = 0;
        }
    }

    // Actualiza la posición del mapa
    public void MoverMapa(float posX, float posZ)
    {
        // Mover el centro del mapa a esa posición
        var temp = new Vector3(posX, transform.position.y, posZ);
        var latLon = mapaCentral.TransformWorldPointToLatLon(temp); // IMPORTANTE HACERLO ANTES DE MOVERLO
                                                                    // traduce de forma relativa: (0,0,0) -> transform.position

        transform.position = temp;

        // Actualizar las coordenadas del mapa para mostrar el desplazamiento
        var zoom = GetComponent<MapRenderer>().ZoomLevel;
        MapChanger(latLon.LatitudeInDegrees, latLon.LongitudeInDegrees, zoom);
    }

    // Basado en: https://seirios48.medium.com/whats-is-microsoft-map-sdk-can-6a7521be3c32
    public void MapChanger(double lat, double lon, float zoom)
    {
        GetComponent<MapRenderer>().SetMapScene(new MapSceneOfLocationAndZoomLevel(new LatLon(lat, lon), zoom), MapSceneAnimationKind.None);
    }
}
