namespace RoyTheunissen.SpatialRumble.Rumbling
{
    /// <summary>
    /// Interface that lets you send rumble values to the hardware in some custom way instead of
    /// via the Unity Input System. For example, using ReWired instead.
    /// </summary>
    public interface ICustomRumbleHandler
    {
        void PassRumbleOnToHardware(RumbleProperties rumbleProperties);
    }
}
