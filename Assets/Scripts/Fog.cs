using System.Collections;
using System.Collections.Generic;
using Oculus.Platform.Models;
using UnityEngine;

/*
 *  Esta clase identifica las propiedades de la niebla, como el color o el tamaño.
 */
[RequireComponent(typeof(ParticleSystem))]
public class Fog : MonoBehaviour
{
    public const float MIN_ALPHA = 0.2f;
 
    private ParticleSystem _particleSystem;

    public enum CalidadDelAire
    {
        Buena, RazonablementeBuena, Regular, Desfavorable, MuyDesfavorable, ExtremadamenteDesfavorable
    }
    private Color[] _colores = 
    {
        new Color(0, 255, 250), new Color(80, 200, 160), new Color(255, 255, 0), new Color(255, 79, 92), new Color(192, 0, 0), new Color(153, 0, 255)
    };
    
    public CalidadDelAire Calidad = CalidadDelAire.Buena;      
    

    public void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();        
    }

    public void Update()
    {
        // Actualizar el color
        var colorOverLifetimeModule = _particleSystem.colorOverLifetime;
        Debug.Log(_colores[(int)Calidad]);
        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(CrearGradiente(_colores[(int) Calidad], GetAlpha(Calidad)));
    }

    /*
     * Crea un gradiente transparente-color-transparente
     */
    private Gradient CrearGradiente(Color c1, float maxAlpha=1.0f)
    {
        var gradient = new Gradient();

        var colorKey = new GradientColorKey[1];
        colorKey[0].color = c1;
        colorKey[0].time = 0.0f;

        var alphaKey = new GradientAlphaKey[4];
        alphaKey[0].alpha = 0.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = maxAlpha;
        alphaKey[1].time = 0.3f;
        alphaKey[2].alpha = maxAlpha;
        alphaKey[2].time = 0.75f;
        alphaKey[3].alpha = 0.0f;
        alphaKey[3].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient;
    }

    /*
     * Devuelve el alpha correspondiente para el nivel de calidad
     */
    private float GetAlpha(CalidadDelAire c)
    {
        return Remap((int)c, 0, _colores.Length, MIN_ALPHA, 1.0f);
    }

    // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
