using UnityEngine;
using UnityEngine.InputSystem;

// Pick a cube with a touch, then drag it on the table.
// Pick = Physics.Raycast (hits the collider). Move = ray vs math plane (no collider needed).
public class DragOnTable : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float halfSize = 4.5f; // Plane scale 1 = 10 x 10 units, so edge is 5

    private Plane _plane;
    private bool _dragging;
    private Vector3 _offset; // cube center minus touch point, so the cube does not jump to the finger
    private float _y;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
        _y = target.position.y;
        // Plane at the cube's center height. The touch point is measured at this height.
        _plane = new Plane(Vector3.up, new Vector3(0f, _y, 0f));
    }

    private void Update()
    {
        Pointer pointer = Pointer.current;
        if (pointer == null)
            return;

        if (pointer.press.wasPressedThisFrame)
        {
            TryBeginDrag(pointer.position.ReadValue());
        }
        else if (_dragging && pointer.press.isPressed)
        {
            Drag(pointer.position.ReadValue());
        }

        if (pointer.press.wasReleasedThisFrame)
            _dragging = false;
    }

    private void TryBeginDrag(Vector2 screen)
    {
        Ray ray = cam.ScreenPointToRay(screen);

        // Did the finger touch our cube?
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f) || hit.transform != target)
            return;
        if (!_plane.Raycast(ray, out float distance))
            return;

        _offset = target.position - ray.GetPoint(distance);
        _offset.y = 0f;
        _dragging = true;
    }

    private void Drag(Vector2 screen)
    {
        Ray ray = cam.ScreenPointToRay(screen);
        if (!_plane.Raycast(ray, out float distance))
            return;

        Vector3 pos = ray.GetPoint(distance) + _offset;
        pos.x = Mathf.Clamp(pos.x, -halfSize, halfSize); // keep the cube on the table
        pos.z = Mathf.Clamp(pos.z, -halfSize, halfSize);
        pos.y = _y;
        target.position = pos;
    }
}
