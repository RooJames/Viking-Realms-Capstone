using UnityEngine;

public class FollowWorldTargetUI : MonoBehaviour
{
    public Transform target;          // The player or enemy
    public Vector3 offset;            // How high above the head
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }
}
