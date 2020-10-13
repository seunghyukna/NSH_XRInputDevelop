using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using XRInput.Core;

public class XRRayInteractorSubSystem : MonoBehaviour
{
    protected XRBaseInteractor baseInteractor;
    protected XRBaseInteractable baseInteractable;
    protected XRRayInteractor rayInteractor;
    protected LineRenderer lineRenderer;
    protected XRInteractorLineVisual lineVisual;

    [SerializeField] protected XRNode xrNode;
    [SerializeField] protected List<InputDevice> devices = new List<InputDevice>();
    [SerializeField] protected InputDevice device;

    protected Vector3 interactPoint;
    protected float interactLength;

    protected void GetInteractPosition()
    {
        interactPoint = lineRenderer.GetPosition(1);
    }

    protected void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    protected XRButtonState GetXRDeviceButtonState(InputDevice inputDevice, InputFeatureUsage<bool> buttonUsage, ref bool lastState)
    {
        bool tempState = false;

        bool triggerButtonState = false;

        tempState = inputDevice.TryGetFeatureValue(buttonUsage, out triggerButtonState) // did get a value
                        && triggerButtonState // the value we got
                        || tempState; // cumulative result from other controllers

        if (tempState != lastState) // Button state changed since last frame
        {
            lastState = tempState;

            if (tempState)
            {
                return XRButtonState.PressStart;
            }
            else
            {
                return XRButtonState.PressEnd;
            }
        }
        else
        {
            if (tempState)
            {
                return XRButtonState.Pressing;
            }
            else
            {
                return XRButtonState.None;
            }
        }
    }
}
