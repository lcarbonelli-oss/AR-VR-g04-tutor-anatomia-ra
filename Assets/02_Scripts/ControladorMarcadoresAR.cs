using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ControladorMarcadoresAR : MonoBehaviour
{
    [Header("Asigna los Prefabs de los Órganos")]
    [SerializeField] private GameObject prefabCorazon;
    [SerializeField] private GameObject prefabPulmon;

    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> organosInstanciados = new Dictionary<string, GameObject>();

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // 1. Cuando se detecta una nueva imagen en la cámara
        foreach (var trackedImage in eventArgs.added)
        {
            string nombreImagen = trackedImage.referenceImage.name;
            GameObject prefabAEleccion = null;

            if (nombreImagen == "MarcadorCorazon" && prefabCorazon != null)
            {
                prefabAEleccion = prefabCorazon;
            }
            else if (nombreImagen == "MarcadorPulmon" && prefabPulmon != null)
            {
                prefabAEleccion = prefabPulmon;
            }

            if (prefabAEleccion != null && !organosInstanciados.ContainsKey(nombreImagen))
            {
                // Instanciar el modelo 3D como hijo del marcador para que se mueva con el papel
                GameObject nuevoOrgano = Instantiate(prefabAEleccion, trackedImage.transform);
                nuevoOrgano.transform.localPosition = Vector3.zero;
                organosInstanciados.Add(nombreImagen, nuevoOrgano);
            }
        }

        // 2. Si el marcador se mueve o actualiza en pantalla
        foreach (var trackedImage in eventArgs.updated)
        {
            string nombreImagen = trackedImage.referenceImage.name;
            if (organosInstanciados.TryGetValue(nombreImagen, out GameObject organo))
            {
                // Si el celular pierde de vista el papel, ocultamos el modelo 3D
                bool estaActivo = trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking;
                organo.SetActive(estaActivo);
            }
        }
    }
}