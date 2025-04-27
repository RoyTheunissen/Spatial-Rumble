using UnityEngine;

namespace RoyTheunissen.SpatialRumble.Rumbling
{
    /// <summary>
    /// Signifies from what position local rumbles should be felt from to calculate attenuation and direction.
    /// </summary>
    public sealed class RumbleListener : MonoBehaviour
    {
        public Vector3 Position => transform.position;
        
        private void Start()
        {
            if (HapticsServices.Rumble == null)
            {
                Debug.LogError(
                    $"Rumble Listener '{name}' tried to register itself at the rumble service, " +
                    $"but no rumble service was registered at the Haptics Services class.", this);
            }
            else
            {
                HapticsServices.Rumble.RegisterListener(this);
            }
        }
        
        private void OnDestroy()
        {
            if (HapticsServices.Rumble != null)
                HapticsServices.Rumble.UnregisterListener(this);
        }
    }
}
