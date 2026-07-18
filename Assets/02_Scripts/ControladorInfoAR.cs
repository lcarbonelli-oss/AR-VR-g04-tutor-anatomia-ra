using UnityEngine;
using TMPro; // Necesario para controlar los textos de TextMeshPro

public class ControladorEscenasLab : MonoBehaviour
{
    [Header("Componentes de la Interfaz")]
    public TextMeshProUGUI TextoTituloOrgano;
    public TextMeshProUGUI TextoDescripcion;

    [Header("Láminas de Vuforia (Image Targets)")]
    public GameObject LaminaCorazon;
    public GameObject LaminaPulmones;

    void Start()
    {
        // 1. Leemos la memoria del juego
        string sala = ClinicaDatos.salaSeleccionada;

        // 2. Si el usuario eligió la Sala de Cardiología
        if (sala == "Corazon")
        {
            TextoTituloOrgano.text = "SISTEMA CARDIOVASCULAR";
            TextoDescripcion.text = "El corazón es el órgano principal del sistema circulatorio. Funciona como una bomba hidráulica muscular que impulsa la sangre por todo el cuerpo para transportar oxígeno y nutrientes.";
            
            // ¡MAGIA!: Encendemos la lámina del corazón y APAGAMOS por completo la del pulmón
            if (LaminaCorazon != null) LaminaCorazon.SetActive(true);
            if (LaminaPulmones != null) LaminaPulmones.SetActive(false);
        }
        // 3. Si el usuario eligió la Sala de Neumología
        else if (sala == "Pulmones")
        {
            TextoTituloOrgano.text = "SISTEMA RESPIRATORIO";
            TextoDescripcion.text = "Los pulmones son los órganos en los cuales se realiza la hematosis o intercambio gaseoso. Permiten captar el oxígeno del aire y eliminar el dióxido de carbono del cuerpo.";
            
            // ¡MAGIA!: Apagamos la lámina del corazón y ENCENDEMOS la del pulmón
            if (LaminaCorazon != null) LaminaCorazon.SetActive(false);
            if (LaminaPulmones != null) LaminaPulmones.SetActive(true);
        }
        // 4. Caso de emergencia si entras directo a la escena
        else
        {
            TextoTituloOrgano.text = "CÁMARA DE ESCANEO AR";
            TextoDescripcion.text = "Por favor, regresa al menú principal para seleccionar una sala de especialidad médica específica.";
            
            // Por seguridad apagamos ambos hasta que elija una sala
            if (LaminaCorazon != null) LaminaCorazon.SetActive(false);
            if (LaminaPulmones != null) LaminaPulmones.SetActive(false);
        }
    }
}