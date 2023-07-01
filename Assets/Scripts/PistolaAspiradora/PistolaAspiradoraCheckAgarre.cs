using Oculus.Interaction.HandGrab;
using UnityEngine;
using Zenject;

/// <summary>
/// Esta clase se encarga de la gestión de los eventos de agarre de la pistola aspiradora.<br />
/// Cuando se agarra la PistolaAspiradora se lanzará un evento PistolaEncendida, 
/// cuando se suelte se lanzará un evento PistolaApagada.
/// </summary>
public class PistolaAspiradoraCheckAgarre : MonoBehaviour
{
    [Inject]
    GameObject Jugador;

    private OVRHand manoD, manoI;

    private void Awake()
    {
        manoD = GameObject.Find("Jugador/TrackingSpace/RightHandAnchor").GetComponentInChildren<OVRHand>();
        manoI = GameObject.Find("Jugador/TrackingSpace/LeftHandAnchor").GetComponentInChildren<OVRHand>();
    }

    private void Update()
    {
        CheckAgarre();
    }

    private void CheckAgarre()
    {
        if (GetComponent<GrabbableItem>().IsAgarrado)
        {
            var restado = gameObject.GetComponents<HandGrabInteractable>()[0].State;
            bool isIndexFingerPinchingD = manoD.GetFingerPinchStrength(OVRHand.HandFinger.Index) > 0;
            bool rtrigger = isIndexFingerPinchingD || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);


            var lestado = gameObject.GetComponents<HandGrabInteractable>()[1].State;
            bool isIndexFingerPinchingI = manoI.GetFingerPinchStrength(OVRHand.HandFinger.Index) > 0;
            bool ltrigger = isIndexFingerPinchingI || OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);

            if ((restado == Oculus.Interaction.InteractableState.Select && rtrigger) || (lestado == Oculus.Interaction.InteractableState.Select && ltrigger))
                Eventos.LanzarPistolaEncendidaEvent();
            else
                Eventos.LanzarPistolaApagadaEvent();
        }
        else
            Eventos.LanzarPistolaApagadaEvent();
    }
}
