using UnityEngine;

/// <summary>
/// Esta clase puede utilizarse durante el desarrollo para debuggear la fuerza final a la que se ve sometido un objeto.
/// </summary>
public class DebugForces : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        // Obtener el componente Rigidbody del objeto
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Dibujar un rayo en la dirección de la fuerza que se aplica al objeto
        Debug.DrawRay(transform.position + new Vector3(3, 3, 0), rb.velocity * 5, Color.red);
    }
}
