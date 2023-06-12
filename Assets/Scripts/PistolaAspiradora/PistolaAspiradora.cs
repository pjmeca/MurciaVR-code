using System.Collections;
using UnityEngine;

/// <summary>
/// Gestiona el comportamiento de la PistolaAspiradora, más concretamente, 
/// gestiona los sonidos y el sistema de partículas.
/// </summary>
public class PistolaAspiradora : MonoBehaviour
{
    public enum Estado
    {
        Encendido, Apagado
    }
    public Estado estado = Estado.Apagado;
    public bool IsAgarrado { get { return GetComponent<GrabbableItem>().IsAgarrado; } }

    private bool wasPlaying = false;

    public AudioClip SonidoInicio, SonidoMedio, SonidoFin;
    private AudioSource[] sources;
    public float InicioAdelantado;

    private ParticleSystem Particulas;

    private void Awake()
    {
        // Dar de alta los manejadores de eventos
        Eventos.PistolaEncendida += OnPistolaEncendida;
        Eventos.PistolaApagada += OnPistolaApagada;
    }

    private void OnDestroy()
    {
        // Eliminar los manejadores de eventos
        Eventos.PistolaEncendida -= OnPistolaEncendida;
        Eventos.PistolaApagada -= OnPistolaApagada;
    }

    void Start()
    {
        Particulas = gameObject.GetComponentInChildren<ParticleSystem>();

        sources = new AudioSource[3];
        for (int i = 0; i < 3; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
        }
        sources[0].clip = SonidoInicio;
        sources[0].playOnAwake = false;
        sources[1].clip = SonidoMedio;
        sources[1].loop = true;
        sources[1].playOnAwake = false;
        sources[1].mute = true;
        sources[1].Play();
        sources[2].clip = SonidoFin;
        sources[2].playOnAwake = false;
    }

    #region MANEJO DE EVENTOS
    private void OnPistolaEncendida()
    {
        estado = Estado.Encendido;

        if (!wasPlaying)
        {
            wasPlaying = true;

            Inicio();
            StartCoroutine(Medio());
        }
    }

    private void OnPistolaApagada()
    {
        estado = Estado.Apagado;

        if (wasPlaying)
        {
            wasPlaying = false;

            StartCoroutine(Fin());
        }
    }
    #endregion

    private void Inicio()
    {
        sources[0].Stop();
        sources[0].Play();
        sources[1].mute = true;

        Particulas.Play();
    }

    private IEnumerator Medio() 
    {
        yield return new WaitForSeconds(SonidoInicio.length - InicioAdelantado);
        
        if(!(estado == Estado.Apagado))
            sources[1].mute = false;
    }

    private IEnumerator Fin()
    {        
        sources[2].Play();
        yield return new WaitForSeconds(InicioAdelantado);
        sources[0].Stop();
        sources[1].mute = true;

        Particulas.Stop();
    }    
}
