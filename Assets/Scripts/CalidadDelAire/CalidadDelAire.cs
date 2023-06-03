using System;
using UnityEngine;

/// <summary>
/// Representa el índice de calidad del aire de un lugar
/// </summary>
public class CalidadDelAire
{
    #region PROPIEDADES
    public enum Indices
    {
        Buena,
        RazonablementeBuena,
        Regular,
        Desfavorable,
        MuyDesfavorable,
        ExtremadamenteDesfavorable
    }
    public Indices Indice { get; set; }
    public static int NumIndices { get { return Enum.GetNames(typeof(Indices)).Length; } }

    private Color[] _colores =
    {
        Color255.New(0, 255, 250),
        Color255.New(80, 200, 160),
        Color255.New(255, 255, 0),
        Color255.New(255, 79, 92),
        Color255.New(192, 0, 0),
        Color255.New(153, 0, 255)
    };
    public Color Color { get { return _colores[(int)Indice]; } }
    #endregion

    public CalidadDelAire(Indices indice=Indices.Buena)
    {
        Indice = indice;
    }
}