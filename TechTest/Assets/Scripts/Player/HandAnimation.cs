using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Handles XR controller input to animate a virtual hand using trigger and grip values.
/// </summary>
public class HandAnimatorController : MonoBehaviour
{
    /// <summary>
    /// The XR controller characteristics to match (e.g., Left/Right hand, controller type).
    /// </summary>
    [Tooltip("Specify the controller characteristics (e.g., Left, Right, Controller)")]
    public InputDeviceCharacteristics controllerCharacteristics;

    /// <summary>
    /// Reference to the hand animator component controlling the hand's animation states.
    /// </summary>
    [Tooltip("Reference to the hand animator that will be controlled by input")]
    public Animator handAnimator;

    /// <summary>
    /// The target XR input device matching the specified characteristics.
    /// </summary>
    private InputDevice targetDevice;


    /// <summary>
    /// Initializes the input device on start.
    /// </summary>
    private void Start()
    {
        TryInitializeDevice();
    }

    /// <summary>
    /// Checks for device validity and updates hand animation every frame.
    /// </summary>
    private void Update()
    {
        if (!targetDevice.isValid)
        {
            TryInitializeDevice();
        }
        else
        {
            UpdateHandAnimation();
        }
    }


    /// <summary>
    /// Attempts to find and assign the first input device matching the desired characteristics.
    /// </summary>
    private void TryInitializeDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    /// <summary>
    /// Updates the hand animator based on trigger and grip input values from the XR controller.
    /// </summary>
    private void UpdateHandAnimation()
    {
        //Trigger
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0f);
        }
        //Grip
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0f);
        }
    }

}
