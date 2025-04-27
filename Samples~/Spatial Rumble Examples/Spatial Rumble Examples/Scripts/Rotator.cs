using UnityEngine;

namespace RoyTheunissen.SpatialRumbleSample
{
    /// <summary>
    /// Rotates the transform over time.
    /// </summary>
    public sealed class Rotator : MonoBehaviour
    {
        [SerializeField] private float angularSpeed = -90.0f;

        private void Update()
        {
            transform.Rotate(0.0f, angularSpeed * Time.deltaTime, 0.0f);
        }
    }
}
