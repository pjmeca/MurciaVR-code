using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eventos
{
    #region PISTOLA ASPIRADORA
    public delegate void PistolaEncendidaDelegate();
    public static event PistolaEncendidaDelegate PistolaEncendida;
    public static void LanzarPistolaEncendidaEvent()
    {
        PistolaEncendida?.Invoke();
    }

    public delegate void PistolaApagadaDelegate();
    public static event PistolaApagadaDelegate PistolaApagada;
    public static void LanzarPistolaApagadaEvent()
    {
        PistolaApagada?.Invoke();
    }
    #endregion
}
