using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Representa el �ndice de calidad del aire de un lugar
/// </summary>
public class CalidadDelAire
{
    #region PROPIEDADES

    #region GLOBALES
    public const float MIN_ALPHA = 0.2f;
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

    public static readonly float SPEED_LIMPIEZA = 500f;
    #endregion

    #region DE OBJETO
    private Indices _indice;
    public Indices Indice
    {
        get
        {
            // Si Concentraciones es null es porque se ha construido con un �ndice fijo no calculable.
            return Concentraciones == null ? _indice : CalcularIndice(Concentraciones); 
        }
        set
        {
            _indice = value;
        }
    }
    public Color Color { 
        get 
        {
            // Cambia el color progresivamente en base al nivel de contaminaci�n
            // Nota: es posible que, en base a los niveles de contaminaci�n, siga habiendo
            // cambios bruscos en el color, pero evitar esto implicar�a mostrar err�neamente
            // los valores de contaminaci�n actuales.
            float porcentajeContaminacion = ContaminacionEnIndice();
            Color colorActual = _colores[(int)Indice];
            Color colorAnterior = Indice == 0 ? colorActual : _colores[(int)Indice - 1];            

            var r = Utils.RelativeToRealValue(porcentajeContaminacion, colorAnterior.r, colorActual.r);
            var g = Utils.RelativeToRealValue(porcentajeContaminacion, colorAnterior.g, colorActual.g);
            var b = Utils.RelativeToRealValue(porcentajeContaminacion, colorAnterior.b, colorActual.b);
            return new Color(r, g, b); 
        } 
    }
    public float Alpha
    {
        get
        {
            return Utils.Remap((int)Indice, 0, NumIndices, MIN_ALPHA, 1.0f);
        }
    }
    public float[] Concentraciones {  get; private set; }
    public bool Cleaned 
    { 
        get
        {
            return Concentraciones.ToList().All(x => x == 0);
        }
    }
    #endregion

    #endregion

    #region CONSTRUCTORES
    public CalidadDelAire(Indices indice=Indices.Buena)
    {
        Indice = indice;
    }
    public CalidadDelAire(float SO2, float NO2, float PM10, float O3, float PM25)
    {
        if (SO2 < 0 || NO2 < 0 || PM10 < 0 || O3 < 0 || PM25 < 0)
            throw new ArgumentException("El nivel de concentraci�n no puede ser negativo.");

        Concentraciones = new float[] { SO2, NO2, PM10, O3, PM25 };
    }
    public CalidadDelAire() : this(0f, 0f, 0f, 0f, 0f) { }
    #endregion

    #region CALCULAR EL �NDICE DE CALIDAD DEL AIRE
    /// <summary>
    /// Calcula el �ndice de calidad del aire en base a los nibeles de concetraci�n de las sustancias.
    /// M�s informaci�n: https://sinqlair.carm.es/calidadaire/principal/interpretacion.aspx
    /// </summary>
    /// <param name="SO2">Nivel de concentraci�n de SO<sub>2</sub> (�g/m�).</param>
    /// <param name="NO2">Nivel de concentraci�n de NO<sub>2</sub> (�g/m�).</param>
    /// <param name="PM10">Nivel de concentraci�n de PM10 (�g/m�).</param>
    /// <param name="O3">Nivel de concentraci�n de O<sub>3</sub> (�g/m�).</param>
    /// <param name="PM25">Nivel de concentraci�n de PM2,5 (�g/m�).</param>
    /// <returns>El �ndice de contaminaci�n escogido.</returns>
    public static Indices CalcularIndice(float SO2, float NO2, float PM10, float O3, float PM25)
    {
        // El �ndice de Calidad del Aire Global se calcula como el peor de los �ndices individuales.
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
            throw new ArgumentException("El nivel de concentraci�n no puede ser negativo.");
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
            throw new ArgumentException("El nivel de concentraci�n no puede ser negativo.");
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
            throw new ArgumentException("El nivel de concentraci�n no puede ser negativo.");
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
            throw new ArgumentException("El nivel de concentraci�n no puede ser negativo.");
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
            throw new ArgumentException("El nivel de concentraci�n no puede ser negativo.");
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
    /// Calcula el porcentaje de contaminaci�n dentro del �ndice actual.
    /// </summary>
    /// <returns>Un valor (0f-1f) que representa el porcentaje de distancia
    /// respecto al �ndice anterior (0 es �ndice menor, 1 es �ndice mayor).</returns>
    public float ContaminacionEnIndice()
    {
        // Obtener las diferencias y quedarnos con la mayor
        // Pasar esa diferencia al t�rmino relativo respecto al anterior y al sucesor

        float maxDiferencia = 0;
        int bandaElegida = 0;
        int nivelBanda = 0;
        for(int j = 0; j < BANDAS_CONCENTRACION.GetLength(1); j++)
        {
            float diferencia = 0;
            int banda = 0;
            for (int i=0; i < BANDAS_CONCENTRACION.GetLength(0); i++)
            {                
                float resta = Concentraciones[j] - BANDAS_CONCENTRACION[i, j];

                // Si es negativa -> fin bucle interno
                if (resta < 0)
                    break;

                // Si esta es positiva -> actualizar diferencia                
                diferencia = resta;
                banda = i;
            }

            // Si es mayor que la maxDiferencia, actualizar
            if(diferencia > maxDiferencia)
            {
                maxDiferencia = diferencia;
                bandaElegida = j;
                nivelBanda = banda;
            }
        }

        // Se sale de la escala
        if ((nivelBanda+1) == BANDAS_CONCENTRACION.GetLength(0))
            return 1f;

        var resultado = Utils.Remap(Concentraciones[bandaElegida], BANDAS_CONCENTRACION[nivelBanda, bandaElegida], BANDAS_CONCENTRACION[nivelBanda+1, bandaElegida], 0.1f, 1f);

        return resultado;
    }

    /// <summary>
    /// Reduce las concentraciones proporcionalmente como resultado de una limpieza de aire.
    /// </summary>
    public void Limpiar()
    {
        // Si se ha creado con �ndice fijo, no se modifica.
        if (Concentraciones == null)
            return;

        // Restar de manera proporcional a la escala        
        var resta = 100 * SPEED_LIMPIEZA      * Time.deltaTime / BANDAS_CONCENTRACION[BANDAS_CONCENTRACION.GetLength(0) - 1, 0];
        Concentraciones[0] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / BANDAS_CONCENTRACION[BANDAS_CONCENTRACION.GetLength(0) - 1, 0];
        Concentraciones[1] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / BANDAS_CONCENTRACION[BANDAS_CONCENTRACION.GetLength(0) - 1, 1];
        Concentraciones[2] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / BANDAS_CONCENTRACION[BANDAS_CONCENTRACION.GetLength(0) - 1, 2];
        Concentraciones[3] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / BANDAS_CONCENTRACION[BANDAS_CONCENTRACION.GetLength(0) - 1, 3];
        Concentraciones[4] -= 100 * SPEED_LIMPIEZA * Time.deltaTime / BANDAS_CONCENTRACION[BANDAS_CONCENTRACION.GetLength(0) - 1, 4];

        // Asegurarse de que ning�n valor sea negativo
        Concentraciones[0] = Concentraciones[0] < 0 ? 0 : Concentraciones[0];
        Concentraciones[1] = Concentraciones[1] < 0 ? 0 : Concentraciones[1];
        Concentraciones[2] = Concentraciones[2] < 0 ? 0 : Concentraciones[2];
        Concentraciones[3] = Concentraciones[3] < 0 ? 0 : Concentraciones[3];
        Concentraciones[4] = Concentraciones[4] < 0 ? 0 : Concentraciones[4];
    }
}