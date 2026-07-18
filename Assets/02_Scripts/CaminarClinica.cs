using UnityEngine;
using UnityEngine.SceneManagement;

public class CaminarClinica : MonoBehaviour
{
    public float velocidad = 5.0f;
    public float sensibilidadMouse = 2.0f;
    public float sensibilidadTactil = 0.2f; // Sensibilidad de la cámara en el celular

    [Header("Configuración Celular")]
    public SimpleJoystick joystickCelular; // Arrastra aquí tu BaseJoystick

    private float rotacionX = 0f;
    private float rotacionY = 0f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Guardamos la rotación inicial de la cámara
        rotacionY = transform.localEulerAngles.y;
        rotacionX = transform.localEulerAngles.x;

        #if UNITY_STANDALONE || UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        #endif
    }

    void Update()
    {
        // 1. ROTACIÓN DE CÁMARA (PC y Celular separados)
        #if UNITY_STANDALONE || UNITY_EDITOR
        // Control con Mouse para PC
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionY += mouseX;
        rotacionX -= mouseY;
        rotacionX = Mathf.Clamp(rotacionX, -60f, 60f);

        transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);
        #else
        // Control táctil para Celular: Girar la cámara deslizando en el lado DERECHO
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                // Solo si el toque empezó en la mitad derecha de la pantalla
                if (touch.position.x > Screen.width / 2)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        float deltaX = touch.deltaPosition.x * sensibilidadTactil;
                        float deltaY = touch.deltaPosition.y * sensibilidadTactil;

                        rotacionY += deltaX;
                        rotacionX -= deltaY;
                        rotacionX = Mathf.Clamp(rotacionX, -60f, 60f);

                        transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);
                    }
                }
            }
        }
        #endif

        // 2. DETECTAR ENTRADAS DE MOVIMIENTO (¡Corregido aquí!)
        float movimientoH = 0f;
        float movimientoV = 0f;

        if (joystickCelular != null && joystickCelular.gameObject.activeInHierarchy)
        {
            // Leemos los valores del pulgar izquierdo
            movimientoH = joystickCelular.Horizontal;
            movimientoV = joystickCelular.Vertical;
        }
        else
        {
            // Teclado de PC (WASD)
            movimientoH = Input.GetAxis("Horizontal"); 
            movimientoV = Input.GetAxis("Vertical");   
        }

        // 3. MOVIMIENTO 3D REAL (Relativo a hacia dónde mira la cámara)
        Vector3 haciaAdelante = transform.forward;
        Vector3 haciaLado = transform.right;

        // Mantenemos al personaje pegado al piso (Y = 0) al moverse
        haciaAdelante.y = 0f;
        haciaLado.y = 0f;

        haciaAdelante.Normalize();
        haciaLado.Normalize();

        Vector3 direccionFinal = (haciaLado * movimientoH) + (haciaAdelante * movimientoV);
        
        // Gravedad constante para que no flote
        Vector3 gravedad = new Vector3(0, -9.81f, 0);
        Vector3 vectorMovimientoTotal = (direccionFinal * velocidad) + gravedad;

        // 4. MOVER AL PERSONAJE (Evita traspasar muros)
        if (controller != null)
        {
            controller.Move(vectorMovimientoTotal * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider otro)
    {
        if (otro.gameObject.name == "PuertaCardio")
        {
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