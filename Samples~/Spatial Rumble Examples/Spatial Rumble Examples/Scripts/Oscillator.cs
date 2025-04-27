using UnityEngine;

namespace RoyTheunissen.SpatialRumbleSample
{
    /// <summary>
    /// Oscillates an object around.
    /// </summary>
    public sealed class Oscillator : MonoBehaviour 
    {
        [SerializeField] private float amplitude = 0.1f;
        [SerializeField] private float frequency = 10.0f;
        [SerializeField] private Vector3 direction = Vector3.up;

        private Vector3 localPositionOriginal;

        private float amplitudeMultiplier = 1.0f;
        public float AmplitudeMultiplier
        {
            get => amplitudeMultiplier;
            set => amplitudeMultiplier = value;
        }

        private void Awake()
        {
            localPositionOriginal = transform.localPosition;
        }

        private void Update()
        {
            transform.localPosition = localPositionOriginal +
                                      direction * (Mathf.Sin(Time.time * frequency) * amplitude * amplitudeMultiplier);
        }
    }
}
