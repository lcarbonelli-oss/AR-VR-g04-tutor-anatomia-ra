using UnityEngine;

public class AnimacionOrganos : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public bool esCorazon = false;
    public bool esPulmon = false;

    private Vector3 escalaInicial;

    void Start()
    {
        // Guardamos el tamaño original del órgano para que no se deforme
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        if (esCorazon)
        {
            // Efecto de latido: un pulso rápido y constante
            float latido = 1.0f + Mathf.Sin(Time.time * 6.0f) * 0.08f;
            transform.localScale = escalaInicial * latido;
        }

        if (esPulmon)
        {
            // Efecto de respiración: se infla y desinfla más lento y suave
            float respiracion = 1.0f + Mathf.Sin(Time.time * 2.5f) * 0.05f;
            transform.localScale = escalaInicial * respiracion;
        }
    }
}