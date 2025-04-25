using UnityEngine;
using UnityEngine.Playables;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// It's easy to determine when a playable behaviour starts, but it's a little more hands-on to determine when a
    /// behaviour *stops*, while that's unfortunately exactly what we need to know in most cases. This class does all
    /// the leg work for getting a nice start/end flow for behaviours.
    /// </summary>
    public abstract class StartStopBehaviour : PlayableBehaviour
    {
        private bool isPlaying;

        private object cachedPlayerData;
        protected object CachedPlayerData => cachedPlayerData;

        protected virtual bool CanStart => true;
        protected virtual bool CanStop => true;

        // Called when the owning graph starts playing
        public override void OnGraphStart(Playable playable)
        {

        }

        // Called when the owning graph stops playing
        public override void OnGraphStop(Playable playable)
        {

        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (Application.isPlaying && !CanStop)
                return;
            
            double duration = playable.GetDuration ();
            //var delay = playable.GetDelay (); // probably used in some cases, but for now, just let it be
            double time = playable.GetTime ();
            float delta = info.deltaTime;

            if (info.evaluationType != FrameData.EvaluationType.Playback)
                return;

            if (Application.isPlaying)
                Stop();
            
            // Copied from the example I found online but doesn't seem to do what they say it does...
            double count = time + delta;
            if (count < duration)
                return;
        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            cachedPlayerData = playerData;
            
            // We don't want start callbacks while scrubbing in the editor.
            if (info.evaluationType != FrameData.EvaluationType.Playback)
                return;

            // NOTE: Can't use OnBehaviourPlay because playerData is not available at that point. Yeah, seriously >_>
            if (Application.isPlaying && CanStart)
                Start();
        }

        private void Start()
        {
            if (isPlaying)
                return;
            
            isPlaying = true;

            OnStart();
        }

        protected abstract void OnStart();

        private void Stop()
        {
            if (!isPlaying)
                return;

            isPlaying = false;
            
            OnStop();
        }
        
        protected abstract void OnStop();
    }
}
