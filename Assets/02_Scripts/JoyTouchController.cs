using UnityEngine;

public class JoyTouchController : MonoBehaviour
{
    // Velocidad de movimiento
    public float moveSpeed = 5f;
    // Sensibilidad del control táctil
    public float touchSensitivity = 0.5f;

    private Vector2 startTouchPosition;
    private Vector2 currentTouchPosition;
    private bool isTouching = false;
    private Transform mainCameraTransform;

    void Start()
    {
        // Buscamos la cámara principal automáticamente
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Solo detectamos movimiento en Android o en el editor para probar
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Cuando el dedo toca la pantalla
                    startTouchPosition = touch.position;
                    isTouching = true;
                    break;

                case TouchPhase.Moved:
                    // Cuando el dedo se mueve por la pantalla
                    if (isTouching)
                    {
                        currentTouchPosition = touch.position;
                        MoveCamera();
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // Cuando el dedo se levanta
                    isTouching = false;
                    break;
            }
        }
    }

    void MoveCamera()
    {
        // Calculamos la dirección del movimiento del dedo
        Vector2 inputDirection = (currentTouchPosition - startTouchPosition) * touchSensitivity;

        // Solo nos movemos en el plano horizontal (X, Z) para "caminar"
        Vector3 move = mainCameraTransform.forward * inputDirection.y + mainCameraTransform.right * inputDirection.x;
        move.y = 0; // Evita que la cámara suba o baje

        // Aplicamos el movimiento
        transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
    }
}
