using Oculus.Interaction.HandGrab;
using UnityEngine;

public class PistolaAspiradoraCheckAgarre : MonoBehaviour
{
    void Update()
    {
        CheckAgarre();
    }

    private void CheckAgarre()
    {
        if (GetComponent<GrabbableItem>().IsAgarrado)
        {
            var restado = gameObject.GetComponents<HandGrabInteractable>()[0].State;
            bool rtrigger = GameObject.Find("Jugador/TrackingSpace/RightHandAnchor").GetComponent<JoystickLocomotionOld>().IsGatilloTriggered();

            var lestado = gameObject.GetComponents<HandGrabInteractable>()[1].State;
            bool ltrigger = GameObject.Find("Jugador/TrackingSpace/LeftHandAnchor").GetComponent<JoystickLocomotionOld>().IsGatilloTriggered();

            if ((restado == Oculus.Interaction.InteractableState.Select && rtrigger) || (lestado == Oculus.Interaction.InteractableState.Select && ltrigger))
                Eventos.LanzarPistolaEncendidaEvent();
            else
                Eventos.LanzarPistolaApagadaEvent();
        }
        else
            Eventos.LanzarPistolaApagadaEvent();
    }
}
