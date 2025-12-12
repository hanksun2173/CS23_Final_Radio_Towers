using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Tooltip("Assign all platforms this lever should control.")]
    public MovingPlatform[] platformsToControl;

    private bool isActivated = false;

    // Call this method when the lever is activated (e.g., by player input or trigger)
    public void ActivateLever()
    {
        if (isActivated) return;
        isActivated = true;
        foreach (var platform in platformsToControl)
        {
            if (platform != null)
            {
                platform.ActivateLever();
            }
        }
        // Optionally, play lever animation or sound here
    }

    // Optionally, add a method to reset the lever and platforms
    // public void ResetLever()
    // {
    //     isActivated = false;
    //     foreach (var platform in platformsToControl)
    //     {
    //         if (platform != null)
    //         {
    //             platform.ResetPlatform();
    //         }
    //     }
    // }
}
