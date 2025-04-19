using UnityEngine;

namespace RoyTheunissen.UnityHaptics.Rumbling
{
    /// <summary>
    /// Signifies from what position local rumbles should be felt from to calculate attenuation and direction.
    /// </summary>
    public sealed class RumbleListener : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        
        // TODO:
        //private ServiceReference<RumbleService> rumbleService = new();
        
        private void Awake()
        {
            // TODO:
            //rumbleService.Reference.RegisterListener(this);
        }
        
        private void OnDestroy()
        {
            // TODO:
            // if (rumbleService.HasCachedReference)
            //     rumbleService.CachedReference.UnregisterListener(this);
        }
    }
}
