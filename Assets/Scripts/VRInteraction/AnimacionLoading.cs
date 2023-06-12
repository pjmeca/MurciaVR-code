using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class AnimacionLoading : MonoBehaviour
{
    private TextMeshPro textoPuntos;
    private string puntos = "";

    private void Awake()
    {
        textoPuntos= GetComponent<TextMeshPro>();
    }

    void Start()
    {
        // Iniciar la animación repitiendo la función AgregarPunto cada 1 segundo
        InvokeRepeating(nameof(AgregarPunto), 0.5f, 0.5f);
    }

    void AgregarPunto()
    {
        if (puntos.Length < 3)
        {
            puntos += "."; 
            textoPuntos.text = "Cargando" + puntos;
        }
        else
        {
            puntos = ""; 
            textoPuntos.text = "Cargando";
        }
    }
}
