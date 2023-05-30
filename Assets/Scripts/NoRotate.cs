using UnityEngine;

public class NoRotate : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Start()
    {
        // Guarda la rotación inicial del objeto hijo
        initialRotation = transform.localRotation;
    }

    private void LateUpdate()
    {
        // Restaura la rotación inicial del objeto hijo en cada actualización
        transform.localRotation = initialRotation;
    }
}

