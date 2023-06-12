/// <summary>
/// Reúne todos los eventos de la aplicación.<br />
/// Como hay pocos eventos, quedan todos declarados en esta clase
/// para facilitar su consulta y modificación.
/// </summary>
public class Eventos
{
    #region LOADING
    public delegate void LoadingDelegate();
    public delegate void LoadedDelegate();
    public static event LoadingDelegate Loading;
    public static event LoadedDelegate Loaded;
    public static void LanzarLoadingEvent()
    {
        Loading?.Invoke();
    }
    public static void LanzarLoadedEvent()
    {
        Loaded?.Invoke();
    }
    #endregion

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
