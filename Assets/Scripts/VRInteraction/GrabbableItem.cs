using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;

/// <summary>
/// Gestiona la interacción de agarre de un objeto a través de su propiedad IsAgarrado.<br />
/// Toma como base el componente Grabbable, pero controla las colisiones con el jugador,
/// de lo contrario, un objeto agarrado podría colisionar con el cuerpo del jugador, 
/// provocando comportamientos indeseados como su desplazamiento debido a la fuerza de repulsión.
/// </summary>

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(HandGrabInteractable))]
public class GrabbableItem : MonoBehaviour
{
    private GameObject Jugador;
    private Grabbable grabbable;

    public bool IsAgarrado { get; private set; }

    void Start()
    {
        grabbable = gameObject.GetComponent<Grabbable>();

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        HandGrabInteractable h = gameObject.GetComponent<HandGrabInteractable>();
        h.InjectRigidbody(rb);

        Jugador = GameObject.Find("Jugador/Cuerpo");

        IsAgarrado = false;
    }

    void Update()
    {
        // Comprueba si está siendo agarrado
        if (!IsAgarrado && grabbable.GrabPoints.Count > 0)
        {
            // Ignora todas sus colisiones con todas las colisiones del jugador
            foreach (Collider c in Jugador.GetComponentsInChildren<Collider>())
                foreach (Collider c2 in gameObject.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(c, c2, true);
            IsAgarrado = true;
        }
        else if(IsAgarrado && grabbable.GrabPoints.Count == 0)
        {
            foreach (Collider c in Jugador.GetComponentsInChildren<Collider>())
                foreach (Collider c2 in gameObject.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(c, c2, false);
            IsAgarrado = false;
        }
    }
}
