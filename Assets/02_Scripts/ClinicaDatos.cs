using UnityEngine;

public class ClinicaDatos : MonoBehaviour
{
    // Esta variable estática guardará la sala seleccionada en la memoria global
    public static string salaSeleccionada = "Ninguna";

    void Awake()
    {
        // Esto hace que este objeto no se destruya al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }
}