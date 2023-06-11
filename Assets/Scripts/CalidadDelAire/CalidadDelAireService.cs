using System.Collections;
using System.Collections.Generic;
using Microsoft.Maps.Unity;
using UnityEngine;
using Zenject;

/// <summary>
/// Este servicio proporciona los datos de contaminación para distintas ubicaciones.
/// </summary>
public interface ICalidadDelAireService
{
    /// <summary>
    /// Devuelve la calidad del aire de una ubicación.
    /// </summary>
    /// <param name="lat">Componente latitud de la ubicación.</param>
    /// <param name="lon">Componente longitud de la ubicación.</param>
    /// <returns></returns>
    CalidadDelAire GetByLatLon(double lat, double lon);
}

public sealed class CalidadDelAireCARMService : ICalidadDelAireService
{
    /// <summary>
    /// Instancia Singleton del servicio.
    /// </summary>
    public static readonly CalidadDelAireCARMService Instance = new();

    #region CONSTANTES
    public float DEFAULT_SO2 = 1000f;
    public float DEFAULT_NO2 = 400f;
    public float DEFAULT_PM10 = 170f;
    public float DEFAULT_O3 = 400f;
    public float DEFAULT_PM25 = 0f;
    #endregion

    #region CONTAMINACION SAN BASILIO
    public float SO2 = 0f;
    public float NO2 = 0f;
    public float PM10 = 0f;
    public float O3 = 0f;
    public float PM25 = 0f;
    #endregion

    private CalidadDelAireCARMService() 
    {        
        // TODO Obtener los datos de la CARM

        // Si hay un error, establecer los valores por defecto
        SO2 = DEFAULT_SO2;
        NO2= DEFAULT_NO2;
        PM10 = DEFAULT_PM10;
        O3 = DEFAULT_O3;
        PM25 = DEFAULT_PM25;
    }

    public CalidadDelAire GetByLatLon(double lat, double lon)
    {
        // TODO Realizar el cálculo a partir de los valores de referencia
        float SO2 = this.SO2;
        float NO2 = this.NO2;
        float PM10 = this.PM10;
        float O3 = this.O3;
        float PM25 = this.PM25;

        return new CalidadDelAire(SO2, NO2, PM10, O3, PM25);
    }
}
