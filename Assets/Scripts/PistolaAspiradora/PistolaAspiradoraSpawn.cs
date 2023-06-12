using System.Collections;
using UnityEngine;

/// <summary>
/// Gestiona el spawn de la PistolaAspiradora.
/// </summary>
public class PistolaAspiradoraSpawn : MonoBehaviour
{
    public bool Reset = false;

    private const int MAX_LUZ_SIZE = 1;
    private const float LUZ_SPEED = 7f;

    private GameObject PistolaAspiradora;
    private ParticleSystem ParticleSystem;
    private Light luz;

    void Start()
    {
        PistolaAspiradora = GameObject.Find("PistolaAspiradora");
        ParticleSystem = gameObject.GetComponent<ParticleSystem>();
        luz = gameObject.GetComponent<Light>();
    }

    void Update()
    {
        CheckPulsacion();

        if(Reset)
        {
            Reset = false;

            // Si ya está aquí, no hay que ejecutar nada
            if (PistolaAspiradora.transform.position == gameObject.transform.position)
                return;
            StartCoroutine(TraerPistola());
            if (PistolaAspiradora.transform.position == gameObject.transform.position)
                return;

            ParticleSystem.Play();
            StartCoroutine(EscalarLuz());

            StartCoroutine(RestoreGravity());
        }        
    }

    private void CheckPulsacion()
    {
        // Se pulsa el botón y no está siendo agarrada
        if ((OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) ||
            OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch)) &&
            !PistolaAspiradora.GetComponent<PistolaAspiradora>().IsAgarrado)
        {
            Reset = true;
        }
    }

    private IEnumerator TraerPistola()
    {
        PistolaAspiradora.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(0.1f);

        PistolaAspiradora.GetComponent<Rigidbody>().isKinematic = false; // para resetear el movimiento

        // Si ya está aquí, no hay que ejecutar nada
        if (PistolaAspiradora.transform.position != gameObject.transform.position)
        {
            PistolaAspiradora.GetComponent<Rigidbody>().useGravity = false;
            PistolaAspiradora.transform.position = gameObject.transform.position;
            PistolaAspiradora.transform.rotation = Quaternion.Euler(new Vector3(-100, gameObject.transform.rotation.eulerAngles.y, -90));
        }        
    }

    private IEnumerator EscalarLuz()
    {
        for(float i=0; i<MAX_LUZ_SIZE; i += LUZ_SPEED * Time.deltaTime)
        {
            luz.range = i;
            yield return null;
        }
        luz.range = 0;
    }

    private IEnumerator RestoreGravity()
    {
        // Cuando se agarra, se reestablece la gravedad
        while (!PistolaAspiradora.GetComponent<PistolaAspiradora>().IsAgarrado)
        {
            yield return null;
        }
        PistolaAspiradora.GetComponent<Rigidbody>().useGravity = true;
    }
}
