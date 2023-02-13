using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class JoystickLocomotion : MonoBehaviour
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

        if(dropDown == tipoMovimiento.Caminar)
            caminar();
        else if (dropDown == tipoMovimiento.Rotar)
            rotar();
    }

    void caminar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick, OVRInput.Controller.LTouch);

        playerRB.position += (transform.right * joystickAxis.x + transform.forward * joystickAxis.y) * Time.deltaTime * speed;

        float fixedY = playerRB.position.y;
        playerRB.position = new Vector3(playerRB.position.x, fixedY,playerRB.position.z); // para no volar
    }

    void rotar()
    {
        var joystickAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch);

        player.transform.Rotate(new Vector3(0, joystickAxis.x, 0) * Time.deltaTime * speed, Space.World);
    }
}
