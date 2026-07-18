using UnityEngine;

public class ControladorJoystick : MonoBehaviour
{
    void Awake()
    {
        // Si el juego se ejecuta en PC (o en el Editor de Unity), desactivamos el Joystick
        #if UNITY_STANDALONE || UNITY_EDITOR
        gameObject.SetActive(false);
        #endif
    }
}