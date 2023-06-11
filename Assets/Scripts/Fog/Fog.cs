using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Maps.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// Identifica las propiedades de la niebla, como el color o el tamaño.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class Fog : MonoBehaviour
{
    [Inject]
    private ICalidadDelAireService calidadDelAireService;
    [Inject]
    private MapRenderer mapRenderer;

    #region PROPIEDADES
    #region GLOBALES
    private readonly static Vector3[] _scales =
    {
        new Vector3(1,1,1),
        new Vector3(2,2,2),
        new Vector3(2.5f,2.5f,2.5f),
        new Vector3(3.5f,3.5f,2.5f),
        new Vector3(4.8f,4.8f,2.5f),
        new Vector3(8,8f,2.5f)
    };
    private readonly static int[] _rateOverTime =
    {
        20,
        40,
        60,
        150,
        350,
        500
    };
    #endregion

    #region DE OBJETO
    private ParticleSystem _particleSystem;
    private SphereCollider _sphereCollider;
    private float INITIAL_RADIUS;

    public float ContaminacionSO2 = 1000f;
    public float ContaminacionNO2 = 400f;
    public float ContaminacionPM10 = 170f;
    public float ContaminacionO3 = 400f;
    public float ContaminacionPM25 = 0;
    public CalidadDelAire Calidad;
    public CalidadDelAire.Indices Indice;

    public ParticleSystem.MinMaxGradient Gradiente
    {
        get
        {
            return new ParticleSystem.MinMaxGradient(CrearGradiente(Calidad.Color, Calidad.Alpha));
        }
    }
    #endregion
    #endregion

    public Fog() { }

    #region MÉTODOS UNITY
    public void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();    
        _sphereCollider = GetComponent<SphereCollider>();

        INITIAL_RADIUS = _sphereCollider.radius;

        // Obtener la calidad del aire
        // Primero, obtener su latitud/longitud a partir de su posición
        var posicion = mapRenderer.TransformWorldPointToLatLon(transform.position);
        Calidad = calidadDelAireService.GetByLatLon(posicion.LatitudeInDegrees, posicion.LongitudeInDegrees);
        Indice = Calidad.Indice;
    }

    public void Update()
    {   
        // Comprobar si se ha cambiado el índice (desde el Inspector)
        Calidad.Indice = Indice;
        Indice = Calidad.Indice;
        ContaminacionSO2 = Calidad.Concentraciones[0];
        ContaminacionNO2 = Calidad.Concentraciones[1];
        ContaminacionPM10 = Calidad.Concentraciones[2];
        ContaminacionO3 = Calidad.Concentraciones[3];
        ContaminacionPM25 = Calidad.Concentraciones[4];

        // Actualizar el tamaño
        UpdateSize();

        // Actualizar el color
        UpdateGradient();       
    }
    #endregion

    #region MÉTODOS AUXILIARES
    private void UpdateGradient()
    {
        var colorOverLifetimeModule = _particleSystem.colorOverLifetime;
        colorOverLifetimeModule.color = Gradiente;
    }

    private void UpdateSize()
    {
        float porcentajeContaminacion = Calidad.ContaminacionEnIndice();

        if (porcentajeContaminacion == 0)
        {
            _particleSystem.Stop();
        }
        else if (_particleSystem.isStopped)
        {
            _particleSystem.Play();
        }

        var shape = _particleSystem.shape;
        shape.scale = new Vector3(
            Utils.RelativeToRealValue(porcentajeContaminacion, Calidad.Indice == 0 ? 0 : _scales[(int)Calidad.Indice-1].x, _scales[(int)Calidad.Indice].x),
            Utils.RelativeToRealValue(porcentajeContaminacion, Calidad.Indice == 0 ? 0 : _scales[(int)Calidad.Indice-1].y, _scales[(int)Calidad.Indice].y),
            Utils.RelativeToRealValue(porcentajeContaminacion, Calidad.Indice == 0 ? 0 : _scales[(int)Calidad.Indice-1].z, _scales[(int)Calidad.Indice].z)
        );
        var emission = _particleSystem.emission;
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = _rateOverTime[(int)Calidad.Indice];
        emission.rateOverTime = rateOverTime;

        // Actualizar también el tamaño del collider
        _sphereCollider.radius = INITIAL_RADIUS * Utils.RelativeToRealValue(porcentajeContaminacion, Calidad.Indice == 0 ? 0 : _scales[(int)Calidad.Indice - 1].x, _scales[(int)Calidad.Indice].x);
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
    #endregion

    #region FUNCIONALIDAD
    /// <summary>
    /// Limpia la nube de contaminación, reduciendo tamaño y color en la siguiente iteración
    /// </summary>
    [ButtonInvoke("Limpiar")]
    public bool testLimpiar;
    public void Limpiar()
    {
        Calidad.Limpiar();
    }
    #endregion
}