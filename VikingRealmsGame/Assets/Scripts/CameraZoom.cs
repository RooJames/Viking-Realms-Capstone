using UnityEngine;
using Unity.Cinemachine;


public class CameraZoom : MonoBehaviour
{
    private CinemachineCamera cam;

    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    void Awake()
    {
        cam = GetComponent<CinemachineCamera>();
    }

    void Update()
    {
        if (cam == null) return;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll == 0) return;

        // Orthographic (2D)
        if (cam.Lens.Orthographic)
        {
            float newSize = cam.Lens.OrthographicSize - scroll * zoomSpeed;
            cam.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }
        // Perspective (3D)
        else
        {
            float newFOV = cam.Lens.FieldOfView - scroll * zoomSpeed;
            cam.Lens.FieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
        }
    }
}