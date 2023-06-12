using System.Collections;
using System.Collections.Generic;
using Microsoft.Maps.Unity;
using UnityEngine;

/// <summary>
/// Gestiona la pantalla de carga mientras se descargan los mapas.
/// </summary>
public class LoadingController : MonoBehaviour
{

    public bool IsLoaded { get; private set; } = false;

    private MapRenderer[] mapas;

    void Awake()
    {
        // Obtener todos los mapas en la escena
        mapas = (MapRenderer[])GameObject.FindObjectsOfType(typeof(MapRenderer));

        Eventos.Loading += OnLoadingEvent;
    }

    void Update()
    {
        if (IsLoaded)
            return;        

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
            AudioListener.pause = false;
            Eventos.LanzarLoadedEvent();
            gameObject.SetActive(false);
        }        
    }

    private void OnLoadingEvent()
    {
        gameObject.SetActive(true);
        IsLoaded = false;
        AudioListener.pause = true;
    }
}
