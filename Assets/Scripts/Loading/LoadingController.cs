using System.Collections.Generic;
using System.Linq;
using Microsoft.Maps.Unity;
using UnityEngine;

/// <summary>
/// Gestiona la pantalla de carga mientras se descargan los mapas.
/// </summary>
public class LoadingController : MonoBehaviour
{
    private LayerMask MascaraOriginal;
    private LayerMask MascaraLoading;
    private bool ActivarMascara = true;

    public bool IsLoaded { get; private set; } = false;
    private bool ManualLoad = false;

    private MapRenderer[] mapas;

    void Awake()
    {       
        Eventos.Loading += OnLoadingEvent;
        Eventos.Loaded += OnLoadedEvent;

        MascaraLoading = LayerMask.NameToLayer("Loading");
        List<Camera> Camaras = ObtenerCamaras();
        MascaraOriginal = Camaras.First().cullingMask;
    }

    private void OnDestroy()
    {
        Eventos.Loading -= OnLoadingEvent;
        Eventos.Loaded -= OnLoadedEvent;
    }

    /// <summary>
    /// Obtiene las cámaras del frame actual.
    /// Es importante consultarlas en el frame actual porque puede haber cambiado de escena.
    /// </summary>
    private List<Camera> ObtenerCamaras()
    {
        List<Camera> Camaras = new();
        Camaras.Add(GameObject.Find("LeftEyeAnchor").GetComponent<Camera>());
        Camaras.Add(GameObject.Find("CenterEyeAnchor").GetComponent<Camera>());
        Camaras.Add(GameObject.Find("RightEyeAnchor").GetComponent<Camera>());

        return Camaras;
    }

    void Update()
    {
        // Obtener todos los mapas en la escena
        mapas = (MapRenderer[])GameObject.FindObjectsOfType(typeof(MapRenderer));

        if (IsLoaded || ManualLoad)
            return; 
        
        // Si estamos en la primera iteración -> obtener cámaras y activar la capa
        if(ActivarMascara)
        {
            List<Camera> Camaras = ObtenerCamaras();

            // Configuramos solo esa capa
            Camaras.ForEach(c => c.cullingMask = 1 << MascaraLoading);

            ActivarMascara = false;
        }

        // Comprobar si se han cargado
        int loadedMaps = 0;
        foreach(var mapa in mapas)
        {
            if (mapa.IsLoaded)
                loadedMaps++;
            else
                break;
        }

        IsLoaded = loadedMaps == mapas.Length;

        if (IsLoaded)
        {
            // Reactivamos las máscaras originales
            List<Camera> Camaras = ObtenerCamaras();
            Camaras.ForEach(c => c.cullingMask = MascaraOriginal);
            ActivarMascara = true;

            AudioListener.pause = false;
            Eventos.LanzarLoadedEvent();
        }        
    }

    private void OnLoadingEvent()
    {
        if (IsLoaded)
            ManualLoad = true;

        IsLoaded = false;        
        AudioListener.pause = true;
    }

    private void OnLoadedEvent()
    {
        if (ManualLoad)
        {
            ManualLoad = false;
        }
    }
}
