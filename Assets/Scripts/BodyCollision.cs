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
    public GameObject mano1;
    [SerializeField]
    public GameObject mano2;

    // Start is called before the first frame update
    void Start()
    {
        // Para que las manos no colisionen con el cuerpo, necesitamos ignorar todos las colisiones con este
        List<Collider> colList = new List<Collider>();
        colList.AddRange(mano1.transform.GetComponentsInChildren<Collider>());
        colList.AddRange(mano2.transform.GetComponentsInChildren<Collider>());
        foreach (Collider c in colList)  {
            Physics.IgnoreCollision(c, GetComponent<Collider>(), true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(cabeza.position.x, pies.position.y, cabeza.position.z);
    }
}
