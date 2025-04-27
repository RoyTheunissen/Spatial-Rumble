using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Rumbling.Sequencing
{
    /// <summary>
    /// Responsible for performing the runtime functionality associated with a serialized timeline clip.
    /// </summary>
    public abstract class RumbleBehaviour : StartStopBehaviour
    {
        public Transform origin;
        public float opacity;
    }
}
