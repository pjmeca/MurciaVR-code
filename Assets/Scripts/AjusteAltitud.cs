using Microsoft.Maps.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class AjusteAltitud : MonoBehaviour
{
    [SerializeField]
    public MapRenderer mapa;
    [Range(0, 20)]
    public float precision; // importante!!! si es demasiado bajo, la API puede quejarse de que hagamos demasiadas peticiones
    [Range(-50, 50)]
    public float ajuste;

    private float posX, posZ;
    private string json;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        posZ = transform.position.z;

        actualizarPosicion(posX, posZ);
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener la posición del jugador
        float posXAct = transform.position.x;
        float posZAct = transform.position.z;

        // Si ha cambiado respecto a la última posición
        if (Mathf.Abs(posXAct) < Mathf.Abs(posX) - precision || Mathf.Abs(posXAct) > Mathf.Abs(posX) + precision
            || Mathf.Abs(posZAct) < Mathf.Abs(posZ) - precision || Mathf.Abs(posZAct) > Mathf.Abs(posZ) + precision)
        {
            posX = posXAct;
            posZ = posZAct;

            actualizarPosicion(posX, posZ);
        }
    }

    void actualizarPosicion(float posX, float posZ)
    {
        // Convertirla a latitud/longitud mediante el mapa
        var temp = new Vector3(posX, 0, posZ);
        var latlon = mapa.TransformWorldPointToLatLon(temp);

        // Enviar una consulta a la API REST para obtener la altitud del punto en metros
        TextAsset mapSessionConfig = (TextAsset)Resources.Load("MapSessionConfig");
        string key = mapSessionConfig.text;
        string url = "http://dev.virtualearth.net/REST/v1/Elevation/List?points="
            + latlon.LatitudeInDegrees.ToString(new CultureInfo("en-US")) + ", " + latlon.LongitudeInDegrees.ToString(new CultureInfo("en-US"))
            + "&key=" + key;
        StartCoroutine(GetText(url, OnTextReceived));
        // Nos quedamos esperando en OnTextReceived para seguir        
    }

    IEnumerator GetText(string url, System.Action<string> callback)
    {
        using UnityWebRequest www = UnityWebRequest.Get(url);
        //Debug.Log(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            json = www.downloadHandler.text;
            callback(json);
        }
    }

    void OnTextReceived(string json)
    {
        //Debug.Log(json);
        Resultado resultado = JsonUtility.FromJson<Resultado>(json);
        int altitud = resultado.resourceSets[0].resources[0].elevations[0];

        Debug.Log("Altitud: " + altitud);

        // Actualizar la posición del jugador en base a la altitud
        float escala = transform.Find("Cuerpo").localScale.y;
        float posY = altitud/escala + (ajuste + (53-altitud)*Mathf.Abs(ajuste/4)); // 53 es la altitud tomada como base en la plaza de la Catedral
        transform.position = new Vector3(posX, posY, posZ);
    }
}

// Clases para el JSON
[System.Serializable]
public class Resource
{
    public string __type;
    public List<int> elevations;
    public int zoomLevel;
}

[System.Serializable]
public class ResourceSet
{
    public int estimatedTotal;
    public List<Resource> resources;
}

[System.Serializable]
public class Resultado
{
    public string authenticationResultCode;
    public string brandLogoUri;
    public string copyright;
    public List<ResourceSet> resourceSets;
    public int statusCode;
    public string statusDescription;
    public string traceId;
}