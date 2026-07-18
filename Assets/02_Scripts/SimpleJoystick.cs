using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Referencias Visuales")]
    public RectTransform palanca; // Arrastra aquí el objeto hijo "Palanca"
    public float rangoMovimiento = 50f; // Qué tanto se puede estirar la palanquita

    // Valores públicos que el personaje leerá para caminar
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    private Vector2 posicionCentro;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        if (palanca != null)
        {
            posicionCentro = palanca.anchoredPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 posicionToque;
        // Convierte el toque de la pantalla a coordenadas locales del joystick
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out posicionToque))
        {
            Vector2 direccion = posicionToque - posicionCentro;
            float distancia = Mathf.Min(direccion.magnitude, rangoMovimiento);
            
            // Movemos la palanquita del centro visualmente
            palanca.anchoredPosition = posicionCentro + direccion.normalized * distancia;

            // Mandamos los valores limpios (-1 a 1) para caminar
            Horizontal = (direccion.normalized * (distancia / rangoMovimiento)).x;
            Vertical = (direccion.normalized * (distancia / rangoMovimiento)).y;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Al soltar el dedo, la palanca vuelve al centro y el personaje se detiene
        palanca.anchoredPosition = posicionCentro;
        Horizontal = 0f;
        Vertical = 0f;
    }
}