namespace RoyTheunissen.UnityHaptics.Rumbling.Sequencing
{
    public sealed class RumbleOneOffBehaviour : RumbleBehaviour
    {
        public RumbleOneOffConfig rumbleOneOff;

        protected override void OnStart()
        {
            if (rumbleOneOff != null)
                rumbleOneOff.Play(origin, opacity);
        }

        protected override void OnStop()
        {
        }
    }
}
