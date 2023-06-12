/// <summary>
/// Reúne todos los eventos de la aplicación.<br />
/// Como hay pocos eventos, quedan todos declarados en esta clase
/// para facilitar su consulta y modificación.
/// </summary>
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

    #region CONSULTA CARM
    public delegate void CalidadDelAireServiceReadyDelegate();
    public static event CalidadDelAireServiceReadyDelegate CalidadDelAireServiceReady;
    public static void LanzarCalidadDelAireServiceReadyEvent()
    {
        CalidadDelAireServiceReady?.Invoke();
    }
    #endregion
}
