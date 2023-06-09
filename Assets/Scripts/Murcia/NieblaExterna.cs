using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NieblaExterna : MonoBehaviour
{
    public GameObject jugador;

    // Update is called once per frame
    void Update()
    {
        var posX = jugador.transform.position.x;
        var posZ = jugador.transform.position.z;
        transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
