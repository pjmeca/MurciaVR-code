using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gestiona la niebla que orbita alrededor de la cabeza del jugador cuando entra en una nube de contaminación.
/// </summary>
public class FogActivator : MonoBehaviour
{
    private GameObject esfera;
    private ParticleSystem ps;

    private List<GameObject> CollidedFogs;

    void Start()
    {
        CollidedFogs = new();

        esfera = gameObject;
        ps = esfera.GetComponent<ParticleSystem>();

        Hide(true);
    }

    private void Update()
    {
        // Eliminar de CollidedFogs las nieblas que se hayan destruido (limpiado completamente)
        CollidedFogs.RemoveAll(x => x.IsDestroyed());

        if (CollidedFogs.Count > 0)
        {
            // La niebla puede haber cambiado de color
            GameObject niebla = CollidedFogs.Last();

            UpdateColor(niebla);
        } else
        {
            if (!ps.isStopped)
            {
                ps.Stop();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fog"))
        {
            GameObject niebla = collider.gameObject;
            CollidedFogs.Add(niebla);

            if (!ps.isPlaying)
            {
                ps.Play();
            }

            // Actualizamos el color de la niebla
            UpdateColor(niebla);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fog"))
        {
            GameObject niebla = collider.gameObject;
            CollidedFogs.Remove(niebla);

            // Si no hay más nieblas, se para
            if (CollidedFogs.Count == 0)
            {
                if (!ps.isStopped)
                    ps.Stop();
            } 
            // Si aún quedan nieblas, se actualiza el color
            else
            {
                // LIFO
                niebla = CollidedFogs[CollidedFogs.Count-1];
                // Actualizamos el color de la niebla
                UpdateColor(niebla);
            }            
        }
    }

    private void UpdateColor(GameObject niebla)
    {
        var colorOverLifetimeModule = ps.colorOverLifetime;
        colorOverLifetimeModule.color = niebla.GetComponent<Fog>().Gradiente;
    }

    // Oculta la esfera
    void Hide(bool hide)
    {
        esfera.GetComponent<Renderer>().enabled = !hide;
    }
}
