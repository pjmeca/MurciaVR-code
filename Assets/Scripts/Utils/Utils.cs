using System;
using UnityEngine;

/// <summary>
/// Esta clase agrupa utilidades genéricas usadas por la aplicación.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Mapea un valor s en el rango a1-a2 al rango b1-b2.<br />
    /// https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    /// </summary>
    /// <param name="s">Valor a mapear.</param>
    /// <param name="a1">Inicio del rango dentro del que se encuentra s.</param>
    /// <param name="a2">Fin del rango dentro del que se encuentra s.</param>
    /// <param name="b1">Inicio del nuevo rango al que se desea convertir s.</param>
    /// <param name="b2">Fin del nuevo rango al que se desea convertir s.</param>
    /// <returns>El nuevo valor de s convertido al rango b1-b2.</returns>
    public static float Remap(float s, float a1, float a2, float b1, float b2)
    {
        if (a1 > a2 || b1 > b2)
            throw new ArgumentException("Los rangos no son válidos.");

        if (s < a1)
            s = a1;

        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    /// <summary>
    /// Calcula el valor real correspondiente a una posicion (0f-1f) dado inicio y final reales.
    /// Por ejemplo, si position es 0.5f y start es 0 y end 100, devolverá 50.
    /// </summary>
    /// <param name="position">Valor de la posición en el rango 0f-1f.</param>
    /// <param name="start">Inicio del nuevo rango.</param>
    /// <param name="end">Fin del nuevo rango.</param>
    /// <returns>El valor convertido al nuevo rango.</returns>
    public static float RelativeToRealValue(float position, float start, float end)
    {
        // Si es mayor, hay que ir al revés (tomar el rango opuesto)
        if (start > end)
        {
            (start, end) = (end, start);
            position = 1f - position;
        }

        if (position < 0f || position > 1f)
        {
            throw new ArgumentException("La posicion debe encontrarse entre 0f y 1f.");
        }

        return Mathf.Lerp(start, end, position);
    }
}
