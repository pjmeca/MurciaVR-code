using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using Oculus.Interaction.Input;

/*
 * Script para el movimiento VR del jugador sin tener en cuenta la API de la ciudad.
 */
public class JoystickLocomotionOld : MonoBehaviour
{
    public GameObject player;
    private Rigidbody playerRB;
    public float speed;
    public enum tipoMovimiento
    {
        Caminar, Rotar
    };
    public tipoMovimiento dropDown = tipoMovimiento.Caminar;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (dropDown == tipoMovimiento.Caminar)
            caminar();
        else if (dropDown == tipoMovimiento.Rotar)
            rotar();
    }

    // https://www.youtube.com/watch?v=rwGv1rmm1kQ
    void caminar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

        float fixedY = playerRB.position.y;

        playerRB.position += speed * Time.deltaTime * (transform.right * joystickAxis.x + transform.forward * joystickAxis.y);

        playerRB.position = new Vector3(playerRB.position.x, fixedY, playerRB.position.z); // para no volar
    }

    void rotar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);

        player.transform.Rotate(speed * Time.deltaTime * new Vector3(0, joystickAxis.x, 0), Space.World);
    }

    /*
     * Comprueba si se está pulsando el gatillo con o sin mando.
     */
    public bool IsGatilloTriggered()
    {
        var hand = gameObject.GetComponentInChildren<OVRHand>();
        bool isIndexFingerPinching = hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) > 0 || hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) > 0;
        if (isIndexFingerPinching)
            return true;

        if (gameObject.name.Equals("LeftHandAnchor"))
            return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        if (gameObject.name.Equals("RightHandAnchor"))
            return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);        

        return false;
    }
}