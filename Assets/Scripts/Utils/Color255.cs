using UnityEngine;

/// <summary>
/// Representa a un color usando el formato RGBA 255.
/// </summary>
public class Color255
{
    public float R { get; private set; }
    public float G { get; private set; }
    public float B { get; private set; }
    public float A { get; private set; }

    public Color255(float r, float g, float b, float a = 255)
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