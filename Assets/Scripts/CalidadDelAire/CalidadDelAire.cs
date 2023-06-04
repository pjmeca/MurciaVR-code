using System;
using UnityEngine;

/// <summary>
/// Representa el índice de calidad del aire de un lugar
/// </summary>
public class CalidadDelAire
{
    #region PROPIEDADES

    #region GLOBALES
    public enum Indices
    {
        Buena,
        RazonablementeBuena,
        Regular,
        Desfavorable,
        MuyDesfavorable,
        ExtremadamenteDesfavorable
    }
    public static int NumIndices { get { return Enum.GetNames(typeof(Indices)).Length; } }

    private static Color[] _colores =
    {
        Color255.New(0, 255, 250),
        Color255.New(80, 200, 160),
        Color255.New(255, 255, 0),
        Color255.New(255, 79, 92),
        Color255.New(192, 0, 0),
        Color255.New(153, 0, 255)
    };

    private static readonly int[,] BANDAS_CONCENTRACION =
    {
        //                               SO2  NO2  PM10   O3  PM2,5
        /*Buena*/                      { 100,  40,   20,  50,    10},
        /*Razonablemente buena*/       { 200,  90,   40, 100,    20},
        /*Regular*/                    { 350, 120,   50, 130,    25},
        /*Desfavorable*/               { 500, 230,  100, 240,    50},
        /*Muy desfavorable*/           { 750, 340,  150, 380,    75},
        /*Extremadamente desfavorable*/{1250,1000, 1200, 800,   800},
    };

    public static readonly float SPEED_LIMPIEZA = 100f;
    #endregion

    #region DE OBJETO
    private Indices _indice;
    public Indices Indice
    {
        get
        {
            // Si Concentraciones es null es porque se ha construido con un índice fijo no calculable.
            return Concentraciones == null ? _indice : CalcularIndice(Concentraciones); 
        }
        set
        {
            _indice = value;
        }
    }
    public Color Color { get { return _colores[(int)Indice]; } }
    public float[] Concentraciones {  get; private set; }
    #endregion

    #endregion

    #region CONSTRUCTOR
    public CalidadDelAire(Indices indice=Indices.Buena)
    {
        Indice = indice;
    }
    public CalidadDelAire(float SO2, float NO2, float PM10, float O3, float PM25)
    {
        if (SO2 < 0 || NO2 < 0 || PM10 < 0 || O3 < 0 || PM25 < 0)
            throw new ArgumentException("El nivel de concentración no puede ser negativo.");

        Concentraciones = new float[] { SO2, NO2, PM10, O3, PM25 };
    }
    #endregion

    #region CALCULAR EL ÍNDICE DE CALIDAD DEL AIRE
    /// <summary>
    /// Calcula el índice de calidad del aire en base a los nibeles de concetración de las sustancias.
    /// Más información: https://sinqlair.carm.es/calidadaire/principal/interpretacion.aspx
    /// </summary>
    /// <param name="SO2">Nivel de concentración de SO<sub>2</sub> (µg/m³).</param>
    /// <param name="NO2">Nivel de concentración de NO<sub>2</sub> (µg/m³).</param>
    /// <param name="PM10">Nivel de concentración de PM10 (µg/m³).</param>
    /// <param name="O3">Nivel de concentración de O<sub>3</sub> (µg/m³).</param>
    /// <param name="PM25">Nivel de concentración de PM2,5 (µg/m³).</param>
    /// <returns>El índice de contaminación escogido.</returns>
    public static Indices CalcularIndice(float SO2, float NO2, float PM10, float O3, float PM25)
    {
        // El Índice de Calidad del Aire Global se calcula como el peor de los índices individuales.
        int indice = 0;
        indice = Mathf.Max(indice, (int)CalcularIndiceSO2(SO2));
        indice = Mathf.Max(indice, (int)CalcularIndiceNO2(NO2));
        indice = Mathf.Max(indice, (int)CalcularIndicePM10(PM10));
        indice = Mathf.Max(indice, (int)CalcularIndiceO3(O3));
        indice = Mathf.Max(indice, (int)CalcularIndicePM25(PM25));

        return (Indices) indice;
    }
    public static Indices CalcularIndice(float[] concentraciones)
    {
        if (concentraciones.Length != 5)
            throw new ArgumentException("El array debe contener 5 enteros que se correspondan con SO2, NO2, PM10, O3 y PM25.");

        return CalcularIndice(concentraciones[0], concentraciones[1], concentraciones[2], concentraciones[3], concentraciones[4]);
    }

    #region AUXILIARES
    private static Indices CalcularIndiceSO2(float SO2)
    {
        if (SO2 < 0)
        {
            throw new ArgumentException("El nivel de concentración no puede ser negativo.");
        }

        for (int i = 0; i < 5; i++)
        {
            if (SO2 <= BANDAS_CONCENTRACION[i, 0])
                return (Indices)i;
        }

        return Indices.ExtremadamenteDesfavorable;
    }
    private static Indices CalcularIndiceNO2(float NO2)
    {
        if (NO2 < 0)
        {
            throw new ArgumentException("El nivel de concentración no puede ser negativo.");
        }

        for (int i = 0; i < 5; i++)
        {
            if (NO2 <= BANDAS_CONCENTRACION[i, 1])
                return (Indices)i;
        }
              
        return Indices.ExtremadamenteDesfavorable;
    }
    private static Indices CalcularIndicePM10(float PM10)
    {
        if (PM10 < 0)
        {
            throw new ArgumentException("El nivel de concentración no puede ser negativo.");
        }

        for (int i = 0; i < 5; i++)
        {
            if (PM10 <= BANDAS_CONCENTRACION[i, 2])
                return (Indices)i;
        }

        return Indices.ExtremadamenteDesfavorable;
    }
    private static Indices CalcularIndiceO3(float O3)
    {
        if (O3 < 0)
        {
            throw new ArgumentException("El nivel de concentración no puede ser negativo.");
        }

        for (int i = 0; i < 5; i++)
        {
            if (O3 <= BANDAS_CONCENTRACION[i, 3])
                return (Indices)i;
        }

        return Indices.ExtremadamenteDesfavorable;
    }
    private static Indices CalcularIndicePM25(float PM25)
    {
        if (PM25 < 0)
        {
            throw new ArgumentException("El nivel de concentración no puede ser negativo.");
        }

        for (int i = 0; i < 5; i++)
        {
            if (PM25 <= BANDAS_CONCENTRACION[i, 4])
                return (Indices)i;
        }

        return Indices.ExtremadamenteDesfavorable;
    }
    #endregion
    #endregion

    /// <summary>
    /// Reduce las concentraciones proporcionalmente como resultado de una limpieza de aire.
    /// </summary>
    public void Limpiar()
    {
        // Si se ha creado con índice fijo, no se modifica.
        if (Concentraciones == null)
            return;

        // Restar de manera proporcional a la escala
        var resta = 100 * SPEED_LIMPIEZA * Time.deltaTime / 1250;
        Concentraciones[0] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / 1250;
        Concentraciones[1] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / 1000;
        Concentraciones[2] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / 1200;
        Concentraciones[3] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / 800;
        Concentraciones[4] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / 800;

        // Asegurarse de que ningún valor sea negativo
        Concentraciones[0] = Concentraciones[0] < 0 ? 0 : Concentraciones[0];
        Concentraciones[1] = Concentraciones[1] < 0 ? 0 : Concentraciones[1];
        Concentraciones[2] = Concentraciones[2] < 0 ? 0 : Concentraciones[2];
        Concentraciones[3] = Concentraciones[3] < 0 ? 0 : Concentraciones[3];
        Concentraciones[4] = Concentraciones[4] < 0 ? 0 : Concentraciones[4];
    }
}