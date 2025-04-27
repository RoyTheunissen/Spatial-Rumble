using RoyTheunissen.SpatialRumble.Rumbling;
using UnityEngine;

namespace RoyTheunissen.SpatialRumbleSample
{
    /// <summary>
    /// Demonstrates how to emit a one-off rumble from code.
    /// </summary>
    public sealed class RumbleEmitterOneOff : MonoBehaviour
    {
        [SerializeField] private RumbleOneOffConfig rumbleOneOffConfig;

        private Vector3 startPositionLocal;

        private float lastRumbleTime;

        private void Awake()
        {
            startPositionLocal = transform.localPosition;
        }

        private void Update()
        {
            // Bounce up and down.
            const float interval = 2.0f;
            float t = Mathf.Repeat(Time.time, interval) / interval;
            float height = (1.0f - Mathf.Pow((t - 0.5f) * 2.0f, 2.0f)) * 3;
            transform.localPosition = startPositionLocal + Vector3.up * height;

            // When we hit the ground, play a rumble.
            if (Time.time > lastRumbleTime + interval)
            {
                lastRumbleTime = Time.time;
                
                rumbleOneOffConfig.Play(transform);
            }
        }
    }
}
