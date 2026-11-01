using UnityEngine;
using UnityEngine.InputSystem;

// Turns a screen touch (or mouse click in Editor) into a point on a horizontal table plane.
public class TouchToTable : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform marker;

    [SerializeField]
    private float tableY = 0f;

    [SerializeField]
    private float markerLift = 0.15f; // so the sphere sits on the table, not inside it

    private Plane _table;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
        // Infinite math plane, facing up, at height tableY. No collider needed.
        _table = new Plane(Vector3.up, new Vector3(0f, tableY, 0f));
    }

    private void Update()
    {
        // Pointer.current = last used pointer (touchscreen on phone, mouse in Editor).
        Pointer pointer = Pointer.current;
        if (pointer == null || !pointer.press.isPressed)
            return;

        Vector2 screen = pointer.position.ReadValue(); // pixels
        Ray ray = cam.ScreenPointToRay(screen); // ray from camera through the pixel

        if (_table.Raycast(ray, out float distance)) // false if the ray never hits the plane
            marker.position = ray.GetPoint(distance) + Vector3.up * markerLift;
    }
}
