using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using KdTree.Math;
using KdTree;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Este servicio proporciona los datos de contaminación para distintas ubicaciones.
/// </summary>
public interface ICalidadDelAireService
{
    bool Ready { get; }

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
    public bool Ready { get; private set; } = false;

    string URL_CARM = "https://sinqlair.carm.es/calidadaire/obtener_datos.aspx?tipo=tablaEstaciones";

    // FACTOR ISOCONCENTRACION RESPECTO A SAN BASILIO
    public readonly KdTree<float, float> _factoresIsoconcentracion;

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

        // Crear el árbol 2D con los factores de isoconcentración
        _factoresIsoconcentracion = new(2, new FloatMath())
        {
            { new[] { 37.993114f, -1.139457f }, 0.95f },
            { new[] { 37.991809f, -1.131184f }, 0.82f },
            { new[] { 37.992278f, -1.134494f }, 0.73f },
            { new[] { 37.992151f, -1.13517f }, 0.79f },
            { new[] { 37.994205f, -1.136691f }, 0.6f },
            { new[] { 37.994053f, -1.137356f }, 0.63f },
            { new[] { 37.994193f, -1.137769f }, 0.66f },
            { new[] { 37.993665f, -1.13675f }, 0.69f },
            { new[] { 37.993864f, -1.12625f }, 0.57f },
            { new[] { 37.993321f, -1.124494f }, 0.54f },
            { new[] { 37.994226f, -1.124473f }, 0.5f },
            { new[] { 37.993802f, -1.123065f }, 0.47f },
            { new[] { 37.994136f, -1.121288f }, 0.44f },
            { new[] { 37.993514f, -1.121057f }, 0.41f },
            { new[] { 37.993708f, -1.120816f }, 0.38f },
            { new[] { 37.991658f, -1.123814f }, 0.6f },
            { new[] { 37.99144f, -1.122559f }, 0.63f },
            { new[] { 37.990388f, -1.137544f }, 0.91f },
            { new[] { 37.990388f, -1.136656f }, 0.95f },
            { new[] { 37.98925f, -1.137246f }, 0.69f },
            { new[] { 37.989461f, -1.137273f }, 0.73f },
            { new[] { 37.98971f, -1.137343f }, 0.76f },
            { new[] { 37.989985f, -1.13744f }, 0.79f },
            { new[] { 37.990018f, -1.137034f }, 0.85f },
            { new[] { 37.990215f, -1.137504f }, 0.88f },
            { new[] { 37.99008f, -1.137461f }, 0.82f },
            { new[] { 37.985612f, -1.138166f }, 0.5f },
            { new[] { 37.984784f, -1.13818f }, 0.54f },
            { new[] { 37.982651f, -1.138555f }, 0.57f },
            { new[] { 37.98685f, -1.13344f }, 0.41f },
            { new[] { 37.985422f, -1.133911f }, 0.44f },
            { new[] { 37.985251f, -1.134087f }, 0.47f },
            { new[] { 37.984045f, -1.134618f }, 0.57f },
            { new[] { 37.981466f, -1.133888f }, 0.54f },
            { new[] { 37.990628f, -1.122248f }, 0.73f },
            { new[] { 37.989551f, -1.122443f }, 0.76f },
            { new[] { 37.987916f, -1.122333f }, 0.79f },
            { new[] { 37.981572f, -1.118773f }, 0.41f },
            { new[] { 37.982759f, -1.11818f }, 0.44f },
            { new[] { 37.984002f, -1.118609f }, 0.47f },
            { new[] { 37.984383f, -1.118663f }, 0.5f },
            { new[] { 37.986829f, -1.119829f }, 0.54f },
            { new[] { 37.983264f, -1.121518f }, 0.38f },
            { new[] { 37.979114f, -1.124617f }, 0.47f },
            { new[] { 37.979258f, -1.124199f }, 0.5f },
            { new[] { 37.979321f, -1.123942f }, 0.54f },
            { new[] { 37.979444f, -1.123427f }, 0.57f },
            { new[] { 37.981301f, -1.120744f }, 0.63f },
            { new[] { 37.980045f, -1.12808f }, 0.73f },
            { new[] { 37.980871f, -1.127074f }, 0.69f },
            { new[] { 37.980972f, -1.126999f }, 0.66f },
            { new[] { 37.986891f, -1.124745f }, 0.63f },
            { new[] { 37.986992f, -1.124257f }, 0.66f },
            { new[] { 37.987508f, -1.124289f }, 0.69f },
            { new[] { 37.988544f, -1.125442f }, 0.6f },
            { new[] { 37.989753f, -1.127704f }, 0.54f },
            { new[] { 37.987248f, -1.128672f }, 0.44f },
            { new[] { 37.988218f, -1.131376f }, 0.57f },
            { new[] { 37.988269f, -1.129906f }, 0.5f },
            { new[] { 37.987519f, -1.139015f }, 0.47f },
        };        

        // Obtener los datos de la CARM
        CoroutineRunner.instance.StartCoroutine(ConsultaCARM(OnConsultaReceived));
    }

    /// <summary>
    /// Realiza la consulta a la web de la CARM para obtener los niveles de contaminación de la última hora.
    /// </summary>
    /// <param name="callback">Método que debe llamarse una vez se haya completado la consulta.</param>
    /// <returns></returns>
    private IEnumerator ConsultaCARM(System.Action<string> callback)
    {
        DateTime fechaHoraActual = DateTime.Now;
        DateTime fechaHoraFormateada = new(fechaHoraActual.Year, fechaHoraActual.Month, fechaHoraActual.Day, fechaHoraActual.Hour, 0, 0);
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
            var json = www.downloadHandler.text;
            callback(json);
        }

        Ready = true;
        Eventos.LanzarCalidadDelAireServiceReadyEvent();
    }

    /// <summary>
    /// Realiza el tratamiento sobre la respuesta recibida de la CARM.
    /// </summary>
    /// <param name="json">Archivo JSON recibido desde la CARM.</param>
    private void OnConsultaReceived(string json)
    {
        // La CARM devuelve un JSON no estándar, hay que corregir el formato
        json = ConvertirJson(json);
        List<Estacion> estaciones = JsonConvert.DeserializeObject<List<Estacion>>(json);

        Estacion SanBasilio = estaciones.Where(e => "San Basilio".Equals(e.nombre)).FirstOrDefault();

        if (SanBasilio != null)
        {
            SO2 = SanBasilio.SO2 == null ? 0 : float.Parse(SanBasilio.SO2, CultureInfo.GetCultureInfo("es-ES"));
            NO2 = SanBasilio.NO2 == null ? 0 : float.Parse(SanBasilio.NO2, CultureInfo.GetCultureInfo("es-ES"));
            PM10 = SanBasilio.PM10 == null ? 0 : float.Parse(SanBasilio.PM10, CultureInfo.GetCultureInfo("es-ES"));
            O3 = SanBasilio.O3 == null ? 0 : float.Parse(SanBasilio.O3, CultureInfo.GetCultureInfo("es-ES"));
            PM25 = SanBasilio.PM25 == null ? 0 : float.Parse(SanBasilio.PM25, CultureInfo.GetCultureInfo("es-ES"));
        }
    }

    /// <summary>
    /// Convierte el JSON recibido de la CARM a un formato estándar.
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
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
        // Calcular a partir de los valores de referencia
        var nodo = _factoresIsoconcentracion.GetNearestNeighbours(new[] { (float) lat, (float) lon }, 1);
        var i = nodo[0].Value;

        float SO2 = this.SO2 * i;
        float NO2 = this.NO2 * i;
        float PM10 = this.PM10 * i;
        float O3 = this.O3 * i;
        float PM25 = this.PM25 * i;

        return new CalidadDelAire(SO2, NO2, PM10, O3, PM25);
    }


    /* Clases para deserializar el JSON */
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
