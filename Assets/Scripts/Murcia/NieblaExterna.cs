using UnityEngine;

/// <summary>
/// Actualiza la posición de la niebla externa siguiendo al jugador.
/// </summary>
public class NieblaExterna : MonoBehaviour
{
    public GameObject jugador;

    void Update()
    {
        var posX = jugador.transform.position.x;
        var posZ = jugador.transform.position.z;
        transform.position = new Vector3(posX, transform.position.y, posZ);
    }
}
