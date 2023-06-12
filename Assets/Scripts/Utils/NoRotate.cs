using UnityEngine;

/// <summary>
/// Deshabilita la rotaci�n de un objeto.
/// </summary>
public class NoRotate : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Start()
    {
        // Guarda la rotaci�n inicial del objeto hijo
        initialRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        // Restaura la rotaci�n inicial del objeto hijo en cada actualizaci�n
        transform.localRotation = initialRotation;
    }
}

