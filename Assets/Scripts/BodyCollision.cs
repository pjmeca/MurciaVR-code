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

    // Start is called before the first frame update
    void Start()
    {
        // Para que las manos no colisionen con el cuerpo, necesitamos ignorar todos las colisiones con este
        foreach (GameObject o in ignorarColisiones)
        {
            foreach (Collider c in o.transform.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(c, GetComponent<Collider>(), true);
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject o in ignorarColisiones)
        {
            foreach (Collider c in o.transform.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(c, GetComponent<Collider>(), true);

        }

        gameObject.transform.position = new Vector3(cabeza.position.x, pies.position.y, cabeza.position.z);
    }
}
