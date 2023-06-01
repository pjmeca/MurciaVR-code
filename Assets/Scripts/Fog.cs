using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Oculus.Platform.Models;
using UnityEngine;

/// <summary>
/// Identifica las propiedades de la niebla, como el color o el tamaño.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class Fog : MonoBehaviour
{
    public const float MIN_ALPHA = 0.2f;
 
    private ParticleSystem _particleSystem;

    public enum CalidadDelAire
    {
        Buena,
        RazonablementeBuena,
        Regular,
        Desfavorable,
        MuyDesfavorable,
        ExtremadamenteDesfavorable
    }
    private Color[] _colores = 
    {
        Color255.New(0, 255, 250),
        Color255.New(80, 200, 160),
        Color255.New(255, 255, 0),
        Color255.New(255, 79, 92),
        Color255.New(192, 0, 0),
        Color255.New(153, 0, 255)
    };
    
    public CalidadDelAire Calidad = CalidadDelAire.Buena;
    private CalidadDelAire? _prevCalidad = null;
    

    public void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();    
    }

    public void Update()
    {
        // Actualizar el color
        if (_prevCalidad == null || _prevCalidad != Calidad)
        {
            var colorOverLifetimeModule = _particleSystem.colorOverLifetime;
            colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(CrearGradiente(_colores[(int)Calidad], GetAlpha(Calidad)));
            _prevCalidad = Calidad;
        }        
    }

    /// <summary>
    /// Crea un gradiente transparente-color-transparente
    /// </summary>
    /// <param name="c"></param>
    /// <param name="maxAlpha">Valor máximo del canal alfa</param>
    /// <returns></returns>
    private Gradient CrearGradiente(Color c, float maxAlpha=1.0f)
    {
        var gradient = new Gradient();

        var colorKey = new GradientColorKey[1];
        colorKey[0].color = c;
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

    /// <summary>
    /// Devuelve el alpha correspondiente para el nivel de calidad
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
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

/// <summary>
/// Esta clase auxiliar se utiliza para poder crear colores usando el formato RGBA 255.
/// </summary>
public class Color255
{
    public float R { get; private set; }
    public float G { get; private set; }
    public float B { get; private set; }
    public float A { get; private set; }

    private Color255(float r, float g, float b, float a=255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public static Color New(float r, float g, float b, float a = 255)
    {
        Color255 c = new Color255(r, g, b, a);
        return c.ToColor();
    }

    private Color ToColor()
    {
        return new Color(R / 255f, G / 255f, B / 255f, A / 255f);
    }
}
