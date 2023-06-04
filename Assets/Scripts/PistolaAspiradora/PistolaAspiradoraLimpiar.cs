using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Este componente se encarga de la limpieza de las nubes de contaminación.
/// Lleva un control de las nubes con las que colisiona y, cuando se lanza el
/// evento PistolaEncendida, comienza la limpieza de la nube.
/// </summary>
public class PistolaAspiradoraLimpiar : MonoBehaviour
{
    private List<GameObject> CollidedFogs;

    #region GESTIONAR LOS MANEJADORES DE EVENTOS
    private void Awake()
    {
        // Dar de alta los manejadores de eventos
        Eventos.PistolaEncendida += OnPistolaEncendida;
    }

    private void OnDestroy()
    {
        // Eliminar los manejadores de eventos
        Eventos.PistolaEncendida -= OnPistolaEncendida;
    }
    #endregion

    #region GESTION DE COLLIDEDFOGS
    void Start()
    {
        CollidedFogs = new();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fog"))
        {
            GameObject niebla = collider.gameObject;
            CollidedFogs.Add(niebla);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fog"))
        {
            GameObject niebla = collider.gameObject;
            CollidedFogs.Remove(niebla);
        }
    }
    #endregion

    private void OnPistolaEncendida()
    {
        if (CollidedFogs.Count == 0)
            return;

        // Nube actual = última en añadirse a la lista
        GameObject nubeActual = CollidedFogs.Last();

        nubeActual.GetComponent<Fog>().Limpiar();
    }
}
