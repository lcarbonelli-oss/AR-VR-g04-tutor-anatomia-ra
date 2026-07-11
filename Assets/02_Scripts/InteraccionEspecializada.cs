using UnityEngine;

public class InteraccionEspecializada : MonoBehaviour
{
    [Header("Ajustes de Sensibilidad")]
    public float velocidadRotacion = 40f;
    public float velocidadZoom = 8f;

    [Header("Limites de Tamano")]
    public float escalaMinima = 0.3f;
    public float escalaMaxima = 3.5f;

    void Update()
    {
        // ROTACIÓN: Si el usuario deja presionado el clic izquierdo y mueve el mouse
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * velocidadRotacion;
            // Hace que gire en su propio eje sobre el mundo virtual
            transform.Rotate(Vector3.up, -rotX, Space.World);
        }

        // ZOOM: Si el usuario mueve la rueda del mouse (Scroll)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 nuevaEscala = transform.localScale + Vector3.one * scroll * velocidadZoom;
            
            // Evitamos que el órgano se rompa volviéndose gigante o invisible
            nuevaEscala.x = Mathf.Clamp(nuevaEscala.x, escalaMinima, escalaMaxima);
            nuevaEscala.y = Mathf.Clamp(nuevaEscala.y, escalaMinima, escalaMaxima);
            nuevaEscala.z = Mathf.Clamp(nuevaEscala.z, escalaMinima, escalaMaxima);
            
            transform.localScale = nuevaEscala;
        }
    }
}