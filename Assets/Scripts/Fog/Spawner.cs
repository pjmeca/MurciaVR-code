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
        // Si el jugador está a más de cierta distancia, parar el sistema de partículas
        // Si está a menos, comprobar si el sistema de partículas se está reproduciendo y, si no, darle al play
        // Tener en cuenta la vida de la niebla, si ya ha sido limpiada la zona, no hay que darle al play porque no hay nada que mostrar
    }
}
