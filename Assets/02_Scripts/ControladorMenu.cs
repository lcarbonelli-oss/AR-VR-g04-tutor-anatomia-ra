using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
    // Este método se ejecutará al hacer clic en el botón para ir a la cámara web
    public void IniciarLaboratorio()
    {
        SceneManager.LoadScene(1); // Carga la escena número 1 (LaboratorioAnatomia)
    }

    // Este método servirá por si luego pones un botón de regresar en el laboratorio
    public void RegresarAlMenu()
    {
        SceneManager.LoadScene(0); // Carga la escena número 0 (MenuPrincipal)
    }
}