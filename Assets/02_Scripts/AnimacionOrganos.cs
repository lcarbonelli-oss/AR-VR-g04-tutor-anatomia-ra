using UnityEngine;
using TMPro; // Necesario para mostrar los nombres de las partes

public class AnimacionOrganos : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public bool esCorazon = false;
    public bool esPulmon = false;

    [Header("Configuración del Texto (Lámina)")]
    public string nombreDeLaParte; // Escribe aquí el nombre (ej: "Tráquea", "Aorta", "Ventrículo")
    public TextMeshProUGUI textoUI; // Arrastra aquí el texto de tu Canvas donde saldrá el nombre

    [Header("Retorno Suave al Soltar")]
    public float velocidadRetorno = 5f;

    private Vector3 escalaInicial;
    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;

    private bool siendoSostenido = false;
    private bool regresando = false;
    private Camera camaraPrincipal;
    private Vector3 compensacionDistancia;

    void Start()
    {
        // Guardamos las poses y escalas iniciales exactas de la pieza para que no se deforme
        escalaInicial = transform.localScale;
        posicionOriginal = transform.localPosition;
        rotacionOriginal = transform.localRotation;

        camaraPrincipal = Camera.main;

        if (textoUI != null)
        {
            textoUI.text = ""; // Empezamos con el texto limpio
        }
    }

    void Update()
    {
        // 1. SI EL USUARIO NO LO ESTÁ TOCANDO:
        if (!siendoSostenido)
        {
            // A. Si se está recuperando de haber sido arrastrado, lo movemos suavemente a su origen
            if (regresando)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, posicionOriginal, Time.deltaTime * velocidadRetorno);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, rotacionOriginal, Time.deltaTime * velocidadRetorno);
                
                // Si ya llegó muy cerca de su origen, desactivamos el modo regreso para que vuelva la animación normal
                if (Vector3.Distance(transform.localPosition, posicionOriginal) < 0.005f)
                {
                    transform.localPosition = posicionOriginal;
                    transform.localRotation = rotacionOriginal;
                    regresando = false;
                }
            }

            // B. ANIMACIONES ORIGINALES DE LATIDO / RESPIRACIÓN (Solo actúan si no lo arrastras)
            if (esCorazon)
            {
                // Efecto de latido: un pulso rápido y constante
                float latido = 1.0f + Mathf.Sin(Time.time * 6.0f) * 0.08f;
                transform.localScale = escalaInicial * latido;
            }
            else if (esPulmon)
            {
                // Efecto de respiración: se infla y desinfla más lento y suave
                float respiracion = 1.0f + Mathf.Sin(Time.time * 2.5f) * 0.05f;
                transform.localScale = escalaInicial * respiracion;
            }
        }
    }

    // --- FUNCIONES DE ARRASTRE TÁCTIL (MOUSE/DEDO) ---

    private void OnMouseDown()
    {
        siendoSostenido = true;
        regresando = false;

        // Al tocar la parte, mostramos su nombre en pantalla como en tus láminas
        if (textoUI != null && !string.IsNullOrEmpty(nombreDeLaParte))
        {
            textoUI.text = nombreDeLaParte;
        }

        // Guardamos la distancia con respecto a la cámara para moverlo de manera correcta
        Vector3 posicionObjetoEnPantalla = camaraPrincipal.WorldToScreenPoint(gameObject.transform.position);
        compensacionDistancia = gameObject.transform.position - camaraPrincipal.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, posicionObjetoEnPantalla.z));
    }

    private void OnMouseDrag()
    {
        if (siendoSostenido)
        {
            // Convertimos la posición de nuestro dedo/mouse para mover la pieza en 3D
            Vector3 posicionObjetoEnPantalla = camaraPrincipal.WorldToScreenPoint(gameObject.transform.position);
            Vector3 posicionMouseActual = new Vector3(Input.mousePosition.x, Input.mousePosition.y, posicionObjetoEnPantalla.z);
            Vector3 posicionMundo = camaraPrincipal.ScreenToWorldPoint(posicionMouseActual) + compensacionDistancia;
            
            transform.position = posicionMundo;

            // Rotamos un poco la pieza mientras la arrastramos para verla mejor
            float rotX = Input.GetAxis("Mouse X") * 10f;
            float rotY = Input.GetAxis("Mouse Y") * 10f;
            transform.Rotate(Vector3.up, -rotX, Space.World);
            transform.Rotate(Vector3.right, rotY, Space.World);
        }
    }

    private void OnMouseUp()
    {
        siendoSostenido = false;
        regresando = true; // Al soltar el dedo, se activa el retorno suave
    }
}