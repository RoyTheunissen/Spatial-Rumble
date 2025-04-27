using RoyTheunissen.SpatialRumble.Rumbling;
using UnityEngine;

namespace RoyTheunissen.SpatialRumbleSample
{
    /// <summary>
    /// Demonstrates how to emit a continuous rumble from code and modify it as it's playing.
    /// </summary>
    public sealed class RumbleEmitterContinuous : MonoBehaviour
    {
        [SerializeField] private RumbleLoopingConfig rumbleLoopingConfig;
        [SerializeField] private Oscillator oscillator;
        
        private RumbleLoopingPlayback rumbleLoopingPlayback;

        private void OnEnable()
        {
            // Start the looping playback and cache the instance so we can manipulate / stop it.
            rumbleLoopingPlayback = rumbleLoopingConfig.Play(transform);
        }

        private void OnDisable()
        {
            // Always call Cleanup on a rumble playback when you're done with it.
            rumbleLoopingPlayback?.Cleanup();
        }

        private void Update()
        {
            float scale = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time * 2.0f / 2.0f);
            transform.localScale = Vector3.one * Mathf.Lerp(0.1f, 1.0f, scale);
            oscillator.AmplitudeMultiplier = scale;
            rumbleLoopingPlayback.OpacityMultiplier = scale;
        }
    }
}
