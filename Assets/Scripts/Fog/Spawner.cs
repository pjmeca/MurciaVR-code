using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [Inject]
    private readonly GameObject Jugador;
    private ParticleSystem _particleSystem;

    /// <summary>
    /// Distancia a partir de la cual deja de reproducirse el sistema de part�culas.
    /// </summary>
    public static int DISTANCE_LIMIT = 700;

    private void Start()
    {
        _particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Obtener la distancia respecto al jugador       
        float distancia = Vector3.Distance(Jugador.transform.position, gameObject.transform.position);

        // Si el jugador est� a m�s de cierta distancia, parar el sistema de part�culas
        if (distancia > DISTANCE_LIMIT)
        {
            if(!_particleSystem.isStopped)
            {
                _particleSystem.Stop();
            }                
        }
        // Si est� a menos, comprobar si el sistema de part�culas se est� reproduciendo y, si no, darle al play
        else
        {
            if (!_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }
        }
    }
}
