using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(HandGrabInteractable))]
public class GrabbableItem : MonoBehaviour
{
    private GameObject Jugador;
    private bool IsAgarrado;

    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        HandGrabInteractable h = gameObject.GetComponent<HandGrabInteractable>();
        h.InjectRigidbody(rb);

        Jugador = GameObject.Find("Jugador/Cuerpo");

        IsAgarrado = false;
    }

    void Update()
    {
        if(!IsAgarrado && gameObject.GetComponent<Grabbable>().GrabPoints.Count > 0) // está siendo agarrado
        {
            // Ignora todas sus colisiones con todas las colisiones del jugador
            foreach (Collider c in Jugador.GetComponentsInChildren<Collider>())
                foreach (Collider c2 in gameObject.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(c, c2, true);
            IsAgarrado = true;
        }
        else if(IsAgarrado && gameObject.GetComponent<Grabbable>().GrabPoints.Count == 0)
        {
            foreach (Collider c in Jugador.GetComponentsInChildren<Collider>())
                foreach (Collider c2 in gameObject.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(c, c2, false);
            IsAgarrado =false;
        }
    }
}
