using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class JoystickLocomotion : MonoBehaviour
{
    public Rigidbody player;
    public float speed;
    public enum tipoMovimiento
    {
        Caminar, Rotar
    };
    public tipoMovimiento dropDown = tipoMovimiento.Caminar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(dropDown == tipoMovimiento.Caminar)
            caminar();
        else if (dropDown == tipoMovimiento.Rotar)
            rotar();
    }

    void caminar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

        player.position += (transform.right * joystickAxis.x + transform.forward * joystickAxis.y) * Time.deltaTime * speed;

        float fixedY = player.position.y;
        player.position = new Vector3(player.position.x, fixedY,player.position.z); // para no volar
    }

    void rotar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

        player.transform.eulerAngles -= (transform.right * joystickAxis.x + transform.forward * joystickAxis.y) * Time.deltaTime * speed;
    }
}
