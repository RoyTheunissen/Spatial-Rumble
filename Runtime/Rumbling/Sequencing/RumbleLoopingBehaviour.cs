namespace RoyTheunissen.UnityHaptics.Rumbling.Sequencing
{
    public sealed class RumbleLoopingBehaviour : RumbleBehaviour
    {
        public RumbleLoopingConfig rumbleLooping;
        private RumbleLoopingPlayback rumbleLoopingPlayback;

        protected override void OnStart()
        {
            if (rumbleLooping != null)
                rumbleLoopingPlayback = rumbleLooping.Play(origin, opacity);
        }

        protected override void OnStop()
        {
            if (rumbleLoopingPlayback != null)
            {
                rumbleLoopingPlayback.Cleanup();
                rumbleLoopingPlayback = null;
            }
        }
    }
}
