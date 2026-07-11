using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorEscenas : MonoBehaviour
{
    public void IrACardiologia()
    {
        ClinicaDatos.salaSeleccionada = "Corazon";
        SceneManager.LoadScene("LaboratorioAnatomia");
    }

    public void IrANeumologia()
    {
        ClinicaDatos.salaSeleccionada = "Pulmones";
        SceneManager.LoadScene("LaboratorioAnatomia");
    }
    
    // Dejamos esta por si la usas en el menú principal
    public void CambiarEscena(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }
}