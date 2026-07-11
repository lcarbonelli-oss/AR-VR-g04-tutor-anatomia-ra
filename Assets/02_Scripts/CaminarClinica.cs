using UnityEngine;
using UnityEngine.SceneManagement;

public class CaminarClinica : MonoBehaviour
{
    public float velocidad = 5.0f;
    public float sensibilidadMouse = 2.0f;

    private float rotacionX = 0f;

    void Start()
    {
        // Bloquea el mouse al empezar el pasillo para poder mirar cómodo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. ROTACIÓN CON EL MOUSE (Mirar a los lados y arriba/abajo)
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        transform.Rotate(Vector3.up * mouseX);

        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -60f, 60f);
        transform.localRotation = Quaternion.Euler(rotacionX, transform.localRotation.eulerAngles.y, 0f);

        // 2. MOVIMIENTO FIRME EN EL SUELO (Ya no flotas al mirar arriba o abajo)
        float movimientoH = Input.GetAxis("Horizontal"); 
        float movimientoV = Input.GetAxis("Vertical");   

        // Calculamos la dirección usando solo la orientación horizontal del jugador
        Vector3 haciaAdelante = transform.forward;
        haciaAdelante.y = 0; // Esto obliga a que nunca vaya hacia arriba o abajo en el aire
        haciaAdelante.Normalize();

        Vector3 haciaLado = transform.right;
        haciaLado.y = 0;
        haciaLado.Normalize();

        Vector3 direccionFinal = (haciaLado * movimientoH) + (haciaAdelante * movimientoV);
        
        // Movemos el cuerpo plano sobre el suelo
        transform.Translate(direccionFinal * velocidad * Time.deltaTime, Space.World);
    }

    // 3. DETECTAR LAS PUERTAS Y LIBERAR EL MOUSE
    void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.name == "PuertaCardio")
        {
            // ¡IMPORTANTE!: Liberamos el mouse antes de cambiar de escena para que puedas hacer clics
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ClinicaDatos.salaSeleccionada = "Corazon";
            SceneManager.LoadScene("LaboratorioAnatomia");
        }
        else if (otro.gameObject.name == "PuertaNeumo")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ClinicaDatos.salaSeleccionada = "Pulmones";
            SceneManager.LoadScene("LaboratorioAnatomia");
        }
    }
}