using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este script se encarga de persistir información entre escenas.
/// </summary>
public class DataHolder : MonoBehaviour
{
    private static DataHolder instance;

    public Ubicaciones.UbicacionesEnum UbicacionInicial = Ubicaciones.UbicacionesEnum.Catedral;    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
