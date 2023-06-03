using UnityEngine;

/// <summary>
/// Identifica las propiedades de la niebla, como el color o el tamaño.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class Fog : MonoBehaviour
{
    #region PROPIEDADES
    public const float MIN_ALPHA = 0.2f;
 
    private ParticleSystem _particleSystem;
    private SphereCollider _sphereCollider;
    private float INITIAL_RADIUS;        
    
    private Vector3[] _scales =
    {
        new Vector3(1,1,1),
        new Vector3(2,2,2),
        new Vector3(2.5f,2.5f,2.5f),
        new Vector3(3.5f,3.5f,2.5f),
        new Vector3(4.8f,4.8f,2.5f),
        new Vector3(8,8f,2.5f)
    };
    private int[] _rateOverTime =
    {
        20,
        40,
        60,
        150,
        350,
        500
    };

    public CalidadDelAire Calidad;
    public CalidadDelAire.Indices Indice;
    private CalidadDelAire.Indices? _prevIndice = null;
    public ParticleSystem.MinMaxGradient Gradiente
    { 
        get
        {
            return new ParticleSystem.MinMaxGradient(CrearGradiente(Calidad.Color, GetAlpha(Calidad)));
        } 
    }
    #endregion

    #region MÉTODOS UNITY
    public void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();    
        _sphereCollider = GetComponent<SphereCollider>();

        var emission = _particleSystem.emission;
        var rateOverTime = emission.rateOverTime;

        INITIAL_RADIUS = _sphereCollider.radius;

        Calidad = new();
        Indice = Calidad.Indice;
    }

    public void Update()
    {
        Calidad.Indice = Indice;

        if (_prevIndice == null || _prevIndice != Calidad.Indice)
        {
            // Actualizar el color
            UpdateGradient();
            // Actualizar el tamaño
            UpdateSize();

            _prevIndice = Calidad.Indice;
        }        
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
        var shape = _particleSystem.shape;
        shape.scale = _scales[(int)Calidad.Indice];
        var emission = _particleSystem.emission;
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = _rateOverTime[(int)Calidad.Indice];
        emission.rateOverTime = rateOverTime;

        // Actualizar también el tamaño del collider
        _sphereCollider.radius = INITIAL_RADIUS * _scales[(int)Calidad.Indice].x;
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
        return Remap((int)c.Indice, 0, CalidadDelAire.NumIndices, MIN_ALPHA, 1.0f);
    }

    // https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    private float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
    #endregion

    #region FUNCIONALIDAD
    /// <summary>
    /// Limpia la nube de contaminación, reduciendo tamaño y color   
    /// </summary>
    public void Limpiar()
    {

    }
    #endregion
}