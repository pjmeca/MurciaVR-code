/*
Script basado en el tutorial de BeginnerVR: https://www.youtube.com/@beginnervr8751
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollision : MonoBehaviour
{
    public Transform cabeza;
    public Transform pies;

    [SerializeField]
    public List<GameObject> ignorarColisiones;

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject o in ignorarColisiones) // hay que llamarla todo el rato, porque pueden cambiarse las manos por mandos
        {
            foreach (Collider c in o.transform.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(c, GetComponent<Collider>(), true);

        }   

        gameObject.transform.position = new Vector3(cabeza.position.x, pies.position.y, cabeza.position.z);
    }
}
