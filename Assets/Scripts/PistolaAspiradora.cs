using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PistolaAspiradora : MonoBehaviour
{
    public enum Estado
    {
        Encendido, Apagado
    }
    public Estado estado = Estado.Apagado;
    private bool play = false;
    private bool wasPlaying = false;
    private bool stop = false;

    public AudioClip SonidoInicio, SonidoMedio, SonidoFin;
    private AudioSource[] sources;
    public float InicioAdelantado;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if(estado == Estado.Encendido)
        {
            play = true;
        } else if(estado == Estado.Apagado)
        {
            stop = true;
        }

        if (play && !wasPlaying)
        {
            stop = false;
            wasPlaying = true;

            Inicio();
            StartCoroutine(Medio());
        }
        
        if(stop && wasPlaying)
        {
            play = wasPlaying = false;

            StartCoroutine(Fin());
        }
    }

    private void Inicio()
    {
        sources[0].Stop();
        sources[0].Play();
        sources[1].mute = true;
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
    }
}
