using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Si el jugador est� a m�s de cierta distancia, parar el sistema de part�culas
        // Si est� a menos, comprobar si el sistema de part�culas se est� reproduciendo y, si no, darle al play
        // Tener en cuenta la vida de la niebla, si ya ha sido limpiada la zona, no hay que darle al play porque no hay nada que mostrar
    }
}
