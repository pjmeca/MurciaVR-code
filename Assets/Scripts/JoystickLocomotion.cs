using System.Collections;
using UnityEngine;
using Microsoft.Maps.Unity;
using System.Globalization;
using UnityEngine.Networking;
using System.Collections.Generic;

public class JoystickLocomotion : MonoBehaviour
{
    private Rigidbody playerRB;

    [Header("Movimiento")]
    public float velocidadCaminar = 20;
    public float velocidadRotar = 60;

    [Header("Volar")]
    public bool isActive = true;
    public float velocidad = 1;
    private bool gravityActiveState;
    private float ajusteVuelo;

    [Header("Ajuste de altitud")]
    [SerializeField]
    public MapRenderer mapa;
    [Range(0, 20)]
    public float precision = 10; // importante!!! si es demasiado bajo, la API puede quejarse de que hagamos demasiadas peticiones
    [Range(-50, 50)]
    public float ajuste;
    private float ultimaPosX, ultimaPosY, ultimaPosZ;
    private float posX, posY, posZ;
    private float nuevaPosX, nuevaPosY, nuevaPosZ;
    private string json;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        gravityActiveState = GetComponent<Rigidbody>().useGravity;

        posX = ultimaPosX = nuevaPosX = playerRB.transform.position.x;
        posY = ultimaPosY = nuevaPosY = playerRB.transform.position.y;
        posZ = ultimaPosZ = nuevaPosZ = playerRB.transform.position.z;

        actualizarPosicion(posX, posZ);
    }

    // Update is called once per frame
    void Update()
    {
        caminar();        // 1
        rotar();          // 2
        volar();          // 3
        ajustarAltitud(); // 4

        playerRB.position = new Vector3(nuevaPosX, nuevaPosY, nuevaPosZ);

        ultimaPosX = posX;
        ultimaPosY = posY;
        ultimaPosZ = posZ;
    }

    // https://www.youtube.com/watch?v=rwGv1rmm1kQ
    void caminar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

        Vector3 aux = playerRB.position + velocidadCaminar * Time.deltaTime * (transform.right * joystickAxis.x + transform.forward * joystickAxis.y);

        nuevaPosX = aux.x;
        nuevaPosZ = aux.z;
    }

    void rotar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);

        transform.Rotate(velocidadRotar * Time.deltaTime * new Vector3(0, joystickAxis.x, 0), Space.World);
    }

    void volar()
    {
        // Desactivamos la gravedad
        GetComponent<Rigidbody>().useGravity = !isActive && gravityActiveState;

        if (isActive)
        {
            var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);

            ajusteVuelo = velocidad * Time.deltaTime * joystickAxis.y;
        }
    }

    void ajustarAltitud()
    {
        // Si ha cambiado respecto a la última posición
        if (Mathf.Abs(posX) < Mathf.Abs(ultimaPosX) - precision || Mathf.Abs(posX) > Mathf.Abs(ultimaPosX) + precision
            || Mathf.Abs(posZ) < Mathf.Abs(ultimaPosZ) - precision || Mathf.Abs(posZ) > Mathf.Abs(ultimaPosZ) + precision)
        {
            actualizarPosicion(posX, posZ);
        }
        // Si no
        else
        {
            // Aplicamos el vuelo
            nuevaPosY += ajusteVuelo;
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

        //Debug.Log("Altitud: " + altitud);

        // Actualizar la posición del jugador en base a la altitud
        float escala = transform.Find("Cuerpo").localScale.y;
        float posY = altitud / escala + (ajuste + (53 - altitud) * Mathf.Abs(ajuste / 4)); // 53 es la altitud tomada como base en la plaza de la Catedral
        nuevaPosY = posY;
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
