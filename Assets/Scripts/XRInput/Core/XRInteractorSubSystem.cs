using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRInputManager = Crengine.XRInput.Core.XRInputStateManager;

namespace Crengine.XRInput.Core
{
    public abstract class XRInteractorSubSystem : MonoBehaviour
    {
        protected XRBaseInteractor baseInteractor;
        protected XRBaseInteractable baseInteractable;
        [SerializeField] protected XRNode xrNode;
        [SerializeField] protected List<InputDevice> devices = new List<InputDevice>();
        [SerializeField] protected InputDevice device;

        // Device button states
        protected bool isTriggerPress;
        protected bool isGripPress;
        protected Vector2 primaryVector;
        protected Vector2 secondaryVector;
        protected bool isMenuPress;

        protected void GetDevice()
        {
            InputDevices.GetDevicesAtXRNode(xrNode, devices);
            device = devices.FirstOrDefault();
        }

        protected void DetectControllerUse()
        {
            device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPress);
            XRInputManager.Instance.SetDeviceTriggerState(xrNode, isTriggerPress);

            device.TryGetFeatureValue(CommonUsages.gripButton, out isGripPress);
            XRInputManager.Instance.SetDeviceGripState(xrNode, isGripPress);

            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primaryVector);
            XRInputManager.Instance.SetDevicePrimaryState(xrNode, primaryVector);

            device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out secondaryVector);
            XRInputManager.Instance.SetDeviceSecondaryState(xrNode, secondaryVector);

            device.TryGetFeatureValue(CommonUsages.menuButton, out isMenuPress);
            XRInputManager.Instance.SetDeviceMenuState(xrNode, isMenuPress);
        }

        protected abstract void SetController();
    }
}