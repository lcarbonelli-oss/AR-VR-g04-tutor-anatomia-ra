using UnityEngine;

[ExecuteInEditMode] // Permite ver las líneas en el editor sin dar Play
public class LineaIndicadora : MonoBehaviour
{
    [Header("Conexiones de la Línea")]
    public Transform puntoTexto;      // Arrastra aquí el texto flotante
    public Transform puntoOrgano;     // Arrastra aquí la parte del órgano 3D

    [Header("Estilo de la Línea")]
    public float grosorLinea = 0.003f; // Grosor ideal para Realidad Aumentada
    public Color colorLinea = Color.black; // Color de la línea de tu lámina

    private LineRenderer lineRenderer;

    void Start()
    {
        ActualizarComponente();
    }

    void Update()
    {
        if (puntoTexto != null && puntoOrgano != null)
        {
            if (lineRenderer == null) ActualizarComponente();

            // Dibujar la línea desde el texto hasta la parte del órgano
            lineRenderer.SetPosition(0, puntoTexto.position);
            lineRenderer.SetPosition(1, puntoOrgano.position);
        }
    }

    void ActualizarComponente()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configuración básica del renderizador de línea
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = grosorLinea;
        lineRenderer.endWidth = grosorLinea;
        
        // Material simple y plano para que no necesite luces
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = colorLinea;
        lineRenderer.endColor = colorLinea;
        
        // Evita que la línea proyecte sombras pesadas
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
    }
}