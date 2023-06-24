using System.Collections.Generic;
using System.Linq;
using Microsoft.Maps.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// Esta clase añade un botón en el inspector para visualizar los puntos de isoconcentración en el mapa.
/// </summary>
public class ViewPuntosIsoconcentracion : MonoBehaviour
{
    [Inject]
    private ICalidadDelAireService calidadDelAireService;
    [Inject]
    private MapRenderer mapRenderer;

    private bool prevEstado = false;
    [SerializeField]
    public bool MOSTRAR = false;

    private List<GameObject> esferasDebug = new();

    // Update is called once per frame
    void Update()
    {
        if(MOSTRAR && !prevEstado) 
        {
            ((CalidadDelAireCARMService)calidadDelAireService)._factoresIsoconcentracion.ToList().ForEach(x => {
                var punto = mapRenderer.TransformLatLonAltToWorldPoint(new Microsoft.Geospatial.LatLonAlt(x.Point[0], x.Point[1], 0));
                esferasDebug.Add(CrearEsferaDebug(punto, new Vector3(100, 100, 100)));
            });

            prevEstado = true;
        } else if (!MOSTRAR && prevEstado) 
        {
            esferasDebug.ForEach(x => Destroy(x));
            esferasDebug = new();
            prevEstado = false;
        }
    }

    private GameObject CrearEsferaDebug(Vector3 posicion, Vector3 escala)
    {
        GameObject esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Establecer las coordenadas de posición
        esfera.transform.position = posicion;

        // Establecer la escala de la esfera
        esfera.transform.localScale = escala;

        return esfera;
    }
}
