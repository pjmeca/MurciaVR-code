using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Este componente gestiona los cambios de escenas.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [ButtonInvoke("LoadMurcia_Catedral")]
    public bool TestLoadMurcia_Catedral;
    [ButtonInvoke("LoadMurcia_FIUM")]
    public bool TestLoadMurcia_FIUM;
    [ButtonInvoke("LoadMurcia_Romea")]
    public bool TestLoadMurcia_Romea;

    /// <summary>
    /// Carga la escena de Murcia.
    /// </summary>
    private IEnumerator CargarMurcia(Ubicaciones.UbicacionesEnum ubicacion = Ubicaciones.UbicacionesEnum.Catedral)
    {
        Eventos.LanzarLoadingEvent();

        DataHolder dataHolder = FindObjectOfType<DataHolder>();
        dataHolder.UbicacionInicial = ubicacion;

        var nuevaEscena = SceneManager.LoadSceneAsync("Murcia", LoadSceneMode.Single);

        while (!nuevaEscena.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Carga la escena de Murcia en la Catedral.
    /// </summary>
    public void LoadMurcia_Catedral()
    {
        StartCoroutine(CargarMurcia(Ubicaciones.UbicacionesEnum.Catedral));
    }

    /// <summary>
    /// Carga la escena de Murcia en la Catedral.
    /// </summary>
    public void LoadMurcia_FIUM()
    {
        StartCoroutine(CargarMurcia(Ubicaciones.UbicacionesEnum.FIUM));
    }

    /// <summary>
    /// Carga la escena de Murcia en la Catedral.
    /// </summary>
    public void LoadMurcia_Romea()
    {
        StartCoroutine(CargarMurcia(Ubicaciones.UbicacionesEnum.Romea));
    }
}
