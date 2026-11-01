using UnityEngine;
using UnityEngine.InputSystem;

// Arrow turns toward the touch point on the table.
public class RotateArrow : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Transform arrow;

    [SerializeField]
    private float turnSpeed = 360f; // degrees per second

    private Plane _plane;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
        _plane = new Plane(Vector3.up, new Vector3(0f, arrow.position.y, 0f));
    }

    private void Update()
    {
        Pointer pointer = Pointer.current;
        if (pointer == null || !pointer.press.isPressed)
            return;

        Ray ray = cam.ScreenPointToRay(pointer.position.ReadValue());
        if (!_plane.Raycast(ray, out float d))
            return;

        Vector3 p = ray.GetPoint(d);
        Vector3 a = arrow.position;
        float target = AngleMath.YawDegrees(a.x, a.z, p.x, p.z);

        // MoveTowardsAngle uses the shortest way and deltaTime: same speed at 30 and 60 fps.
        float yaw = Mathf.MoveTowardsAngle(arrow.eulerAngles.y, target, turnSpeed * Time.deltaTime);
        arrow.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}
