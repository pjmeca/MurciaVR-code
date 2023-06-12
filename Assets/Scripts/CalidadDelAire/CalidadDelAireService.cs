using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

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

    string URL_CARM = "https://sinqlair.carm.es/calidadaire/obtener_datos.aspx?tipo=tablaEstaciones";

    #region CONSTANTES CONTAMINACION
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
        // Establecer los valores por defecto
        SO2 = DEFAULT_SO2;
        NO2 = DEFAULT_NO2;
        PM10 = DEFAULT_PM10;
        O3 = DEFAULT_O3;
        PM25 = DEFAULT_PM25;

        // Obtener los datos de la CARM
        CoroutineRunner.instance.StartCoroutine(ConsultaCARM(OnConsultaReceived));        
    }

    private IEnumerator ConsultaCARM(System.Action<string> callback)
    {
        DateTime fechaHoraActual = DateTime.Now;
        DateTime fechaHoraFormateada = new DateTime(fechaHoraActual.Year, fechaHoraActual.Month, fechaHoraActual.Day, fechaHoraActual.Hour, 0, 0);
        string fechaHoraString = fechaHoraFormateada.ToString("dd/MM/yyyy HH:mm");

        UnityWebRequest www = UnityWebRequest.Get(URL_CARM);
        www.url += "&fechaCalidad=" + UnityWebRequest.EscapeURL(fechaHoraString);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);            
        }
        else
        {
            // Show results as text
            var json = www.downloadHandler.text;
            callback(json);
        }

        Eventos.LanzarCalidadDelAireServiceReadyEvent();
    }

    private void OnConsultaReceived(string json)
    {
        // La CARM devuelve un JSON no estándar, hay que corregir el formato
        json = ConvertirJson(json);
        List<Estacion> estaciones = JsonConvert.DeserializeObject<List<Estacion>>(json);

        Estacion SanBasilio = estaciones.Where(e => "San Basilio".Equals(e.nombre)).FirstOrDefault();

        if(SanBasilio!= null)
        {
            SO2  =  SanBasilio.SO2  == null ? 0 : float.Parse(SanBasilio.SO2, CultureInfo.GetCultureInfo("es-ES"));
            NO2  =  SanBasilio.NO2  == null ? 0 : float.Parse(SanBasilio.NO2, CultureInfo.GetCultureInfo("es-ES"));
            PM10 =  SanBasilio.PM10 == null ? 0 : float.Parse(SanBasilio.PM10, CultureInfo.GetCultureInfo("es-ES"));
            O3   =  SanBasilio.O3   == null ? 0 : float.Parse(SanBasilio.O3, CultureInfo.GetCultureInfo("es-ES"));
            PM25 =  SanBasilio.PM25 == null ? 0 : float.Parse(SanBasilio.PM25, CultureInfo.GetCultureInfo("es-ES"));
        }
    }

    private static string ConvertirJson(string json)
    {
        json.Trim();
        // Eliminar el caracter U+FEFF (marca de orden de bytes de UTF-8)
        if (json.Length > 0 && json[0] == '\uFEFF')
            json = json.Substring(1);
        json = Regex.Replace(json, @"([^{]+?):'(.+?)'(,?)", "\"$1\":\"$2\"$3");


        return json;
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

    
    public class EstacionesContainer
    {
        public List<Estacion> estaciones;
    }
    public class Estacion
    {
        public string nombre { get; set; }
        public string zona { get; set; }
        public string estacion { get; set; }
        public string NO2 { get; set; }
        public string calidadNO2 { get; set; }
        public string PM10 { get; set; }
        public string calidadPM10 { get; set; }
        public string O3 { get; set; }
        public string calidadO3 { get; set; }
        public string GLOBAL { get; set; }
        public string calidadGLOBAL { get; set; }
        public string SO2 { get; set; }
        public string calidadSO2 { get; set; }
        public string PM25 { get; set; }
        public string calidadPM25 { get; set; }
    }
}
