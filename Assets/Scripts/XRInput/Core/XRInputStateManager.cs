using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Crengine.XRInput.Core.Singleton;

namespace Crengine.XRInput.Core
{
    public class XRInputStateManager : XRInputSingleton<XRInputStateManager>
    {
        public XRController leftController;
        public XRController rightController;

        [SerializeField] XRControlTypeManager controlTypeManager;

        private void Start()
        {
            leftController = controlTypeManager.LeftController.GetComponent<XRController>();
            rightController = controlTypeManager.RightController.GetComponent<XRController>();
        }

        #region events

        public delegate void InteractHoverEvent(XRBaseInteractable _interactable);
        public delegate void InteractSelectEvent(XRBaseInteractable _interactable);

        public static InteractHoverEvent HoverEnterEvent;
        public static InteractHoverEvent HoveringEvent;
        public static InteractHoverEvent HoverExitEvent;

        public static InteractSelectEvent SelectEnterEvent;
        public static InteractSelectEvent SelectingEvent;
        public static InteractSelectEvent SelectExitEvent;

        public void InvokeHoverEnter(XRBaseInteractable _interactable)
        {
            HoverEnterEvent.Invoke(_interactable);
        }

        public void InvokeHovering(XRBaseInteractable _interactable)
        {
            HoveringEvent.Invoke(_interactable);
        }

        public void InvokeHoverEnd(XRBaseInteractable _interactable)
        {
            HoverExitEvent.Invoke(_interactable);
        }

        public void InvokeSelectEnter(XRBaseInteractable _interactable)
        {
            SelectEnterEvent.Invoke(_interactable);
        }

        public void InvokeSelecting(XRBaseInteractable _interactable)
        {
            SelectingEvent.Invoke(_interactable);
        }

        public void InvokeSelectExit(XRBaseInteractable _interactable)
        {
            SelectExitEvent.Invoke(_interactable);
        }

        #endregion

        #region value, getter, setter

        private Vector3 leftHitPosition;
        private Vector3 leftInteractOffset;
        private float leftInteractLength;
        private bool leftTriggerState;
        private bool leftGripState;
        private Vector2 leftPrimaryVector;
        private Vector2 leftSecondaryVector;
        private bool leftMenuState;
        private bool leftInteracting;
        private GameObject leftInteractObject;

        private Vector3 rightHitPosition;
        private Vector3 rightInteractOffset;
        private float rightInteractLength;
        private bool rightTriggerState;
        private bool rightGripState;
        private Vector2 rightPrimaryVector;
        private Vector2 rightSecondaryVector;
        private bool rightMenuState;
        private bool rightInteracting;
        private GameObject rightInteractObject;

        public Vector3 GetDeviceHitPosition(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightHitPosition : leftHitPosition;
        }

        public void SetDeviceHitPosition(XRNode _xrNode, Vector3 _vector)
        {
            Vector3 tmp = _xrNode == XRNode.RightHand ? rightHitPosition = _vector : leftHitPosition = _vector;
        }

        public Vector3 GetDeviceInteractOffset(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightInteractOffset : leftInteractOffset;
        }
        public void SetDeviceInteractOffet(XRNode _xrNode, Vector3 _targetPos, Vector3 _interactPos)
        {
            Vector3 offset = (_targetPos - _interactPos);
            Vector3 tmp = _xrNode == XRNode.RightHand ? rightInteractOffset = offset : leftInteractOffset = offset;
        }

        public float GetDeviceInteractLength(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightInteractLength : leftInteractLength;
        }
        public void SetDeviceInteractLength(XRNode _xrNode, float _length)
        {
            float tmp = _xrNode == XRNode.RightHand ? rightInteractLength = _length : leftInteractLength = _length;
        }

        public bool GetDeviceTriggerState(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightTriggerState : leftTriggerState;
        }
        public void SetDeviceTriggerState(XRNode _xrNode, bool _state)
        {
            bool tmp = _xrNode == XRNode.RightHand ? rightTriggerState = _state : leftTriggerState = _state;
        }

        public bool GetDeviceGripState(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightGripState : leftGripState;
        }
        public void SetDeviceGripState(XRNode _xrNode, bool _state)
        {
            bool tmp = _xrNode == XRNode.RightHand ? rightGripState = _state : leftGripState = _state;
        }

        public Vector2 GetDevicePrimaryState(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightPrimaryVector : leftPrimaryVector;
        }
        public void SetDevicePrimaryState(XRNode _xrNode, Vector2 _vector)
        {
            Vector2 tmp = _xrNode == XRNode.RightHand ? rightPrimaryVector = _vector : leftPrimaryVector = _vector;
        }

        public Vector2 GetDeviceSecondaryState(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightSecondaryVector : leftSecondaryVector;
        }
        public void SetDeviceSecondaryState(XRNode _xrNode, Vector2 _vector)
        {
            Vector2 tmp = _xrNode == XRNode.RightHand ? rightSecondaryVector = _vector : leftSecondaryVector = _vector;
        }

        public bool GetDeviceMenuState(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightMenuState : leftMenuState;
        }
        public void SetDeviceMenuState(XRNode _xrNode, bool _state)
        {
            bool tmp = _xrNode == XRNode.RightHand ? rightMenuState = _state : leftMenuState = _state;
        }

        public bool GetDeviceInteractingState(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightInteracting : leftInteracting;
        }
        public void SetDeviceInteractingState(XRNode _xrNode, bool _state)
        {
            bool tmp = _xrNode == XRNode.RightHand ? rightInteracting = _state : leftInteracting = _state;
        }

        public GameObject GetDeviceInteractObject(XRNode _xrNode)
        {
            return _xrNode == XRNode.RightHand ? rightInteractObject : leftInteractObject;
        }
        public void SetDeviceInteractObject(XRNode _xrNode, GameObject _object)
        {
            GameObject tmp = _xrNode == XRNode.RightHand ? rightInteractObject = _object : leftInteractObject = _object;
        }

        #endregion
    }
}