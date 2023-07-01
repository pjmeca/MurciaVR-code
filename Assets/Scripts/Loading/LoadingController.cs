using System.Collections.Generic;
using System.Linq;
using Microsoft.Maps.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private AudioSource sonidoLobby;
    private List<AudioSource> sonidosToUnmute;

    void Awake()
    {       
        Eventos.Loading += OnLoadingEvent;
        Eventos.Loaded += OnLoadedEvent;

        MascaraLoading = LayerMask.NameToLayer("Loading");
        List<Camera> Camaras = ObtenerCamaras();
        MascaraOriginal = Camaras.First().cullingMask;

        DataHolder dataHolder = FindObjectOfType<DataHolder>();
        sonidoLobby = dataHolder.GetComponent<AudioSource>();
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
        List<Camera> Camaras = new()
        {
            GameObject.Find("LeftEyeAnchor").GetComponent<Camera>(),
            GameObject.Find("CenterEyeAnchor").GetComponent<Camera>(),
            GameObject.Find("RightEyeAnchor").GetComponent<Camera>()
        };

        return Camaras;
    }

    void Update()
    {
        if (IsLoaded || ManualLoad)
            return;

        // Obtener todos los mapas en la escena
        mapas = (MapRenderer[])GameObject.FindObjectsOfType(typeof(MapRenderer));        
        
        // Si estamos en la primera iteración -> obtener cámaras y activar la capa
        if(ActivarMascara)
        {
            List<Camera> Camaras = ObtenerCamaras();

            // Configuramos solo esa capa
            Camaras.ForEach(c => c.cullingMask = 1 << MascaraLoading);

            ActivarMascara = false;
        }

        // Comprobar si se han cargado
        IsLoaded = mapas.ToList().Where(x => x.IsLoaded).Count() == mapas.Length;

        if (IsLoaded)
        {
            // Reactivamos las máscaras originales
            List<Camera> Camaras = ObtenerCamaras();
            Camaras.ForEach(c => c.cullingMask = MascaraOriginal);
            ActivarMascara = true;

            if (sonidoLobby != null)
            {
                sonidosToUnmute?.ForEach(x => x.mute = false);
                
                if (SceneManager.GetActiveScene().name != "Selector Ubicación Inicio")
                    sonidoLobby.mute = true;
            }
            else
            {
                AudioListener.pause = false;
            }            
            Eventos.LanzarLoadedEvent();
        }        
    }

    private void OnLoadingEvent()
    {
        if (IsLoaded)
            ManualLoad = true;

        IsLoaded = false;

        if (sonidoLobby != null)
        {
            sonidosToUnmute = FindObjectsOfType<AudioSource>().ToList().Where(x => !x.mute).ToList();
            sonidosToUnmute.ForEach(x => x.mute = true);
            sonidoLobby.mute = false;
        }
        else
        {
            AudioListener.pause = true;
        }        
    }

    private void OnLoadedEvent()
    {
        if (ManualLoad)
        {
            ManualLoad = false;
        }
    }
}
