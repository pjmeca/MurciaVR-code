/// <summary>
/// Re�ne todos los eventos de la aplicaci�n.<br />
/// Como hay pocos eventos, quedan todos declarados en esta clase
/// para facilitar su consulta y modificaci�n.
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
    private static bool completada = false;
    /// <summary>
    /// Debe usarse este m�todo en lugar de hacer += si se desea que se reciba el evento incluso si se suscribe despu�s de haberse lanzado.
    /// </summary>
    public static void SuscribirseCalidadDelAireServiceReadyEvent(CalidadDelAireServiceReadyDelegate metodo)
    {
        CalidadDelAireServiceReady += metodo;

        // Si el evento ya se lanz�, ejecutar el m�todo de inmediato
        if (completada)
        {
            metodo.Invoke();
        }
    }
    public static void LanzarCalidadDelAireServiceReadyEvent()
    {
        completada = true;
        CalidadDelAireServiceReady?.Invoke();
    }
    #endregion
}
