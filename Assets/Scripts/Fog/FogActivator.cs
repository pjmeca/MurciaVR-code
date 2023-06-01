using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogActivator : MonoBehaviour
{
    public GameObject Fog;

    private Collider fogCollider;
    private GameObject esfera;
    private Collider cuerpo;
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        fogCollider = Fog.GetComponent<SphereCollider>();

        esfera = gameObject;
        cuerpo = GameObject.Find("Jugador/Cuerpo").GetComponent<CapsuleCollider>();
        ps = esfera.GetComponent<ParticleSystem>();

        Hide(true);
    }

    // Update is called once per frame
    void Update()
    {
        bool isInside = IsInside();

        // Si está dentro de fogCollider -> esfera activada
        // Si no -> esfera desactivada
        //Hide(!isInside);
        if(isInside)
        {
            if(!ps.isPlaying)
            {
                ps.Play();
            }
        } else
        {
            ps.Stop();
        }

        // TODO buscar un material para la esfera que simule niebla (darle un poco de transparencia)
        // TODO hacer que la esfera aparezca y desaparezca gradualmente

    }

    // Comprueba si se encuentra dentro de la niebla
    bool IsInside()
    {
        // Está dentro o intersecta
        return (fogCollider.bounds.Contains(cuerpo.ClosestPoint(fogCollider.transform.position)))/* || (fogCollider.bounds.Intersects(cuerpo.bounds))*/;
    }

    // Oculta la esfera
    void Hide(bool hide)
    {
        esfera.GetComponent<Renderer>().enabled = !hide;
    }
}
