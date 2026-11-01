using UnityEngine;

// Rotates this object around the Y axis.
// Time.deltaTime makes the speed the same at 30 fps and 60 fps.
public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float degreesPerSecond = 90f;

    private void Update()
    {
        transform.Rotate(0f, degreesPerSecond * Time.deltaTime, 0f);
    }
}
