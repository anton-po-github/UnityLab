using UnityEngine;
using UnityEngine.InputSystem;

// Slingshot shot: drag away from the ball, release to shoot the other way.
// Prediction = SphereCast (ball-sized) + Vector3.Reflect. Impulse is applied in FixedUpdate.
public class BallShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Camera cam;

    [SerializeField]
    private Rigidbody ball;

    [SerializeField]
    private LineRenderer aimLine;

    [SerializeField]
    private LayerMask wallMask;

    [Header("Shot")]
    [SerializeField]
    private float maxDragMeters = 3f;

    [SerializeField]
    private float minImpulse = 0.5f;

    [SerializeField]
    private float maxImpulse = 10f;

    [SerializeField]
    private float stillSpeed = 0.05f;

    [SerializeField]
    private float minPowerToShoot = 0.05f;

    [Header("Prediction")]
    [SerializeField, Range(1, 4)]
    private int maxBounces = 2;

    [SerializeField]
    private float maxRayLength = 30f;

    [SerializeField]
    private float lineHeight = 0.05f;

    [SerializeField]
    private Color weakColor = Color.green;

    [SerializeField]
    private Color strongColor = Color.red;
    private Material lineMat; // instance of the line material, created once

    private Vector3[] points; // allocated once, reused (no GC in Update)
    private int pointCount;
    private float radius;
    private bool dragging;
    private Vector3 pendingImpulse;
    private bool hasPending;

    private void Awake()
    {
        var col = ball.GetComponent<SphereCollider>();
        radius = col.radius * ball.transform.lossyScale.x;
        points = new Vector3[maxBounces + 2]; // start + one point per segment
        // Default max angular velocity is low (7 rad/s). A fast rolling ball would slide.
        ball.maxAngularVelocity = 50f;
        aimLine.positionCount = 0;
        lineMat = aimLine.material;
    }

    private void Update()
    {
        Pointer pointer = Pointer.current; // mouse on PC, touch on phone
        if (pointer == null)
            return;

        if (!dragging)
        {
            if (pointer.press.wasPressedThisFrame && IsBallStill())
                dragging = true;
            return;
        }

        if (!TryGetTablePoint(pointer.position.ReadValue(), out Vector3 touch))
            return;

        Vector3 pull = ball.position - touch; // shot goes from finger toward the ball
        pull.y = 0f;
        float power01 = ShotPower.DragToPower01(pull.magnitude, maxDragMeters);

        if (pointer.press.wasReleasedThisFrame)
        {
            if (power01 >= minPowerToShoot)
            {
                pendingImpulse =
                    pull.normalized * ShotPower.ToImpulse(power01, minImpulse, maxImpulse);
                hasPending = true;
            }
            EndDrag();
            return;
        }

        if (pull.sqrMagnitude < 0.0001f)
        {
            aimLine.positionCount = 0;
            return;
        }
        lineMat.color = Color.Lerp(weakColor, strongColor, power01);

        pointCount = Predict(ball.position, pull.normalized);
        DrawLine();
    }

    private void FixedUpdate()
    {
        // Physics changes belong in FixedUpdate.
        if (!hasPending)
            return;
        ball.AddForce(pendingImpulse, ForceMode.Impulse);
        hasPending = false;
    }

    private bool IsBallStill() => ball.linearVelocity.sqrMagnitude < stillSpeed * stillSpeed;

    private bool TryGetTablePoint(Vector2 screen, out Vector3 point)
    {
        var plane = new Plane(Vector3.up, new Vector3(0f, ball.position.y, 0f));
        Ray ray = cam.ScreenPointToRay(screen);
        if (plane.Raycast(ray, out float t))
        {
            point = ray.GetPoint(t);
            return true;
        }
        point = default;
        return false;
    }

    // SphereCast = ball-sized ray. Reflect = bounce direction on the wall normal.
    private int Predict(Vector3 origin, Vector3 dir)
    {
        int count = 0;
        points[count++] = origin;
        for (int i = 0; i <= maxBounces; i++)
        {
            if (
                Physics.SphereCast(
                    origin,
                    radius,
                    dir,
                    out RaycastHit hit,
                    maxRayLength,
                    wallMask,
                    QueryTriggerInteraction.Ignore
                )
            )
            {
                Vector3 stop = origin + dir * hit.distance;
                points[count++] = stop;
                dir = Vector3.Reflect(dir, hit.normal);
                dir.y = 0f;
                dir.Normalize();
                origin = stop + hit.normal * 0.001f; // step off the wall, avoid a zero-distance re-hit
            }
            else
            {
                points[count++] = origin + dir * maxRayLength;
                break;
            }
        }
        return count;
    }

    private void DrawLine()
    {
        aimLine.positionCount = pointCount;
        for (int i = 0; i < pointCount; i++)
        {
            Vector3 p = points[i];
            p.y = lineHeight; // draw just above the table
            aimLine.SetPosition(i, p);
        }
    }

    private void EndDrag()
    {
        dragging = false;
        pointCount = 0;
        aimLine.positionCount = 0;
    }

    // Scene view (and Game view with Gizmos on): the ball circle at each predicted point.
    private void OnDrawGizmos()
    {
        if (points == null)
            return;
        Gizmos.color = Color.cyan;
        for (int i = 0; i < pointCount; i++)
        {
            Gizmos.DrawWireSphere(points[i], radius);
            if (i > 0)
                Gizmos.DrawLine(points[i - 1], points[i]);
        }
    }
}
