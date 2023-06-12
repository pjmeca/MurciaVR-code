using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ignora todas las colisiones con el cuerpo del jugador.<br />
/// Como el jugador está compuesto por varios GameObject con colliders,
/// es importante ignorar sus colisiones, de lo contrario, el jugador podría
/// verse sometido a fuerzas inesperadas que produzcan movimientos involuntarios.
/// </summary>
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
