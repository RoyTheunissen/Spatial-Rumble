using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Responsible for playing back a rumble config, similar to an Audio Source. You can also start a rumble by calling
    /// Play on a Rumble Config. That's what I prefer, but this is a more generic re-usable way to do it.
    /// </summary>
    public class RumbleSource : MonoBehaviour
    {
        [SerializeField] private RumbleConfigBase rumbleConfig;
        
        [SerializeField] private bool playOnAwake = true;

        private float opacity = 1.0f;
        public float Opacity
        {
            get => opacity;
            set
            {
                opacity = value;

                if (hasLoopingPlayback)
                    loopingPlayback.OpacityMultiplier = value;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (hasOneOffPlayback && !oneOffPlayback.IsFinished)
                    return true;
                
                if (hasLoopingPlayback && !loopingPlayback.IsFinished)
                    return true;

                return false;
            }
        }
        
        private RumbleOneOffPlayback oneOffPlayback;
        private bool hasOneOffPlayback;
        
        private RumbleLoopingPlayback loopingPlayback;
        private bool hasLoopingPlayback;

        private void OnEnable()
        {
            if (playOnAwake)
                Play();
        }

        private void OnDisable()
        {
            Stop();
        }

        private void Update()
        {
            if (hasOneOffPlayback && oneOffPlayback.IsFinished)
            {
                oneOffPlayback = null;
                hasOneOffPlayback = false;
            }
            
            if (hasLoopingPlayback && loopingPlayback.IsFinished)
            {
                loopingPlayback = null;
                hasLoopingPlayback = false;
            }
        }

        public void Play()
        {
            if (rumbleConfig == null)
                return;

            if (rumbleConfig is RumbleOneOffConfig oneOffConfig)
            {
                hasOneOffPlayback = true;
                oneOffPlayback = oneOffConfig.Play(transform, Opacity);
                return;
            }
            
            if (rumbleConfig is RumbleLoopingConfig loopingConfig)
            {
                // For one-offs, starting a new one while an old one is still ongoing seems like expected behaviour.
                // For loops, pressing play while a loop is already active should just restart the loop.
                StopLoopingPlayback();
                
                hasLoopingPlayback = true;
                loopingPlayback = loopingConfig.Play(transform, Opacity);
                return;
            }
            
            Debug.LogWarning($"Rumble Source '{name}' tried to play Rumble Config '{rumbleConfig}' " +
                             $"which was of an unknown type.", this);
        }

        public void Play(float opacity)
        {
            Opacity = opacity;
            
            Play();
        }

        public void Stop()
        {
            if (hasOneOffPlayback)
            {
                hasOneOffPlayback = false;
                oneOffPlayback.Stop();
                oneOffPlayback = null;
            }

            StopLoopingPlayback();
        }

        private void StopLoopingPlayback()
        {
            if (!hasLoopingPlayback)
                return;
            
            hasLoopingPlayback = false;
            loopingPlayback.Stop();
            loopingPlayback = null;
        }
    }
}
