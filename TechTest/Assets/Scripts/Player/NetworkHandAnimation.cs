using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Controls hand animation based on XR input on the client that owns this networked object.
/// </summary>
public class NetworkHandAnimation : NetworkBehaviour
{
    #region Client Variables

    /// <summary>
    /// Specifies the characteristics of the XR controller (e.g., Left/Right hand).
    /// </summary>
    [Tooltip("Specify the controller characteristics (e.g., Left, Right, Controller)")]
    public InputDeviceCharacteristics controllerCharacteristics;

    /// <summary>
    /// Animator responsible for animating the virtual hand model.
    /// </summary>
    [Tooltip("Animator controlling the virtual hand model.")]
    public Animator handAnimator;

    /// <summary>
    /// XR input device corresponding to the specified characteristics.
    /// </summary>
    private InputDevice targetDevice;


    /// <summary>
    /// Initializes the input device on start if this client owns the object.
    /// </summary>
    private void Start()
    {
        if (!isOwned) return;
        TryInitialize();
    }

    /// <summary>
    /// Checks device validity and updates the hand animation every frame.
    /// </summary>
    private void Update()
    {
        if (!isOwned) return;

        if (!targetDevice.isValid)
        {
            TryInitialize();
            return;
        }

        UpdateHandAnimation();
    }


    /// <summary>
    /// Attempts to find and assign the XR input device that matches the specified characteristics.
    /// </summary>
    private void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    /// <summary>
    /// Updates the hand animation based on grip and trigger input values.
    /// </summary>
    private void UpdateHandAnimation()
    {
        // Trigger
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0f);
        }

        // Grip
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0f);
        }
    }

    #endregion
}