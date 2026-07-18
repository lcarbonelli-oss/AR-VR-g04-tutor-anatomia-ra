using UnityEngine;

public class InteraccionTactilAR : MonoBehaviour
{
    [Header("Configuración de Rotación")]
    [SerializeField] private float velocidadRotacion = 0.5f;

    [Header("Configuración de Escala (Zoom)")]
    [SerializeField] private float escalaMinima = 0.5f;
    [SerializeField] private float escalaMaxima = 2.0f;
    [SerializeField] private float velocidadZoom = 0.01f;

    private Vector3 escalaInicial;

    void Start()
    {
        // Guardamos la escala que tiene el prefab original para usarla de base
        escalaInicial = transform.localScale;
    }

    void Update()
    {
        // 1. ROTACIÓN CON UN SOLO DEDO
        if (Input.touchCount == 1)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Moved)
            {
                // Rotar en el eje Y (izquierda/derecha) y en el eje X (arriba/abajo) según el movimiento del dedo
                float rotacionX = toque.deltaPosition.y * velocidadRotacion;
                float rotacionY = -toque.deltaPosition.x * velocidadRotacion;

                transform.Rotate(Vector3.up, rotacionY, Space.World);
                transform.Rotate(Vector3.right, rotacionX, Space.World);
            }
        }

        // 2. ZOOM HACIENDO PINZA CON DOS DEDOS
        if (Input.touchCount == 2)
        {
            Touch toque0 = Input.GetTouch(0);
            Touch toque1 = Input.GetTouch(1);

            // Calcular las posiciones del cuadro anterior de cada toque
            Vector2 posicionPreviaToque0 = toque0.position - toque0.deltaPosition;
            Vector2 posicionPreviaToque1 = toque1.position - toque1.deltaPosition;

            // Calcular la distancia entre los dedos en el cuadro anterior y en el actual
            float magnitudDistanciaPrevia = (posicionPreviaToque0 - posicionPreviaToque1).magnitude;
            float magnitudDistanciaActual = (toque0.position - toque1.position).magnitude;

            // La diferencia nos dice si se están alejando (zoom in) o acercando (zoom out)
            float diferenciaDistancia = magnitudDistanciaActual - magnitudDistanciaPrevia;

            AplicarZoom(diferenciaDistancia);
        }
    }

    void AplicarZoom(float incremento)
    {
        // Calculamos la nueva escala multiplicando por el incremento
        Vector3 nuevaEscala = transform.localScale + Vector3.one * incremento * velocidadZoom;

        // Limitamos la escala para que el órgano no se vuelva gigante o desaparezca
        float limiteMinimoX = escalaInicial.x * escalaMinima;
        float limiteMaximoX = escalaInicial.x * escalaMaxima;

        nuevaEscala.x = Mathf.Clamp(nuevaEscala.x, limiteMinimoX, limiteMaximoX);
        nuevaEscala.y = Mathf.Clamp(nuevaEscala.y, limiteMinimoX, limiteMaximoX);
        nuevaEscala.z = Mathf.Clamp(nuevaEscala.z, limiteMinimoX, limiteMaximoX);

        transform.localScale = nuevaEscala;
    }
}