using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volar : MonoBehaviour
{
    public bool isActive = true;
    private bool gravityActiveState;

    // Start is called before the first frame update
    void Start()
    {
        gravityActiveState = gameObject.GetComponent<Rigidbody>().useGravity;
    }

    // Update is called once per frame
    void Update()
    {
        // Desactivamos la gravedad
        gameObject.GetComponent<Rigidbody>().useGravity = !isActive && gravityActiveState;

        if (isActive)
        {
            
            
        }
    }
}
