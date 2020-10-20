using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRInput.Core.Singleton;

namespace XRInput.Core
{
    public class XRInputStateManager : XRInputSingleton<XRInputStateManager>
    {
        public XRController leftController;
        public XRController rightController;

        #region Getter, Setter

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
            //return Getter(_xrNode, leftHitPosition, rightHitPosition);
            if (_xrNode == XRNode.RightHand)
            {
                return rightHitPosition;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftHitPosition;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return Vector3.zero;
            }
        }
        public void SetDeviceHitPosition(XRNode _xrNode, Vector3 _vector)
        {
            //Setter(_xrNode, leftHitPosition, rightHitPosition, _vector);
            if (_xrNode == XRNode.RightHand)
            {
                rightHitPosition = _vector;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftHitPosition = _vector;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public Vector3 GetDeviceInteractOffset(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftInteractOffset, rightInteractOffset);
            if (_xrNode == XRNode.RightHand)
            {
                return rightInteractOffset;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftInteractOffset;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return Vector3.zero;
            }
        }
        public void SetDeviceInteractOffet(XRNode _xrNode, Vector3 _targetPos, Vector3 _interactPos)
        {
            //Setter(_xrNode, leftInteractOffset, rightInteractOffset, _targetPos - _interactPos);
            if (_xrNode == XRNode.RightHand)
            {
                rightInteractOffset = _targetPos - _interactPos;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftInteractOffset = _targetPos - _interactPos;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public float GetDeviceInteractLength(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftInteractLength, rightInteractLength);
            if (_xrNode == XRNode.RightHand)
            {
                return rightInteractLength;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftInteractLength;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return 0.0f;
            }
        }
        public void SetDeviceInteractLength(XRNode _xrNode, float _length)
        {
            //Setter(_xrNode, leftInteractLength, rightInteractLength, _length);
            if (_xrNode == XRNode.RightHand)
            {
                rightInteractLength = _length;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftInteractLength = _length;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public bool GetDeviceTriggerState(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftTriggerState, rightTriggerState);
            if (_xrNode == XRNode.RightHand)
            {
                return rightTriggerState;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftTriggerState;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return false;
            }
        }
        public void SetDeviceTriggerState(XRNode _xrNode, bool _state)
        {
            //Setter(_xrNode, leftTriggerState, rightTriggerState, _state);
            if (_xrNode == XRNode.RightHand)
            {
                rightTriggerState = _state;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftTriggerState = _state;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public bool GetDeviceGripState(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftGripState, rightGripState);
            if (_xrNode == XRNode.RightHand)
            {
                return rightGripState;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftGripState;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return false;
            }
        }
        public void SetDeviceGripState(XRNode _xrNode, bool _state)
        {
            //Setter(_xrNode, leftGripState, rightGripState, _state);
            if (_xrNode == XRNode.RightHand)
            {
                rightGripState = _state;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftGripState = _state;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public Vector2 GetDevicePrimaryState(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftPrimaryVector, rightPrimaryVector);
            if (_xrNode == XRNode.RightHand)
            {
                return rightPrimaryVector;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftPrimaryVector;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return Vector2.zero;
            }
        }
        public void SetDevicePrimaryState(XRNode _xrNode, Vector2 _vector)
        {
            //Setter(_xrNode, leftPrimaryVector, rightPrimaryVector, _vector);
            if (_xrNode == XRNode.RightHand)
            {
                rightPrimaryVector = _vector;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftPrimaryVector = _vector;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public Vector2 GetDeviceSecondaryState(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftSecondaryVector, rightSecondaryVector);
            if (_xrNode == XRNode.RightHand)
            {
                return rightSecondaryVector;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftSecondaryVector;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return Vector2.zero;
            }
        }
        public void SetDeviceSecondaryState(XRNode _xrNode, Vector2 _vector)
        {
            //Setter(_xrNode, leftSecondaryVector, rightSecondaryVector, _vector);
            if (_xrNode == XRNode.RightHand)
            {
                rightSecondaryVector = _vector;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftSecondaryVector = _vector;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public bool GetDeviceMenuState(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftMenuState, rightMenuState);
            if (_xrNode == XRNode.RightHand)
            {
                return rightMenuState;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftMenuState;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return false;
            }
        }
        public void SetDeviceMenuState(XRNode _xrNode, bool _state)
        {
            //Setter(_xrNode, leftMenuState, rightMenuState, _state);
            if (_xrNode == XRNode.RightHand)
            {
                rightMenuState = _state;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftMenuState = _state;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public bool GetDeviceInteractingState(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftInteracting, rightInteracting);
            if (_xrNode == XRNode.RightHand)
            {
                return rightInteracting;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftInteracting;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return false;
            }
        }
        public void SetDeviceInteractingState(XRNode _xrNode, bool _state)
        {
            //Setter(_xrNode, leftInteracting, rightInteracting, _state);
            if (_xrNode == XRNode.RightHand)
            {
                rightInteracting = _state;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftInteracting = _state;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }

        public GameObject GetDeviceInteractObject(XRNode _xrNode)
        {
            //return Getter(_xrNode, leftInteractObject, rightInteractObject);
            if (_xrNode == XRNode.RightHand)
            {
                return rightInteractObject;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return leftInteractObject;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return null;
            }
        }
        public void SetDeviceInteractObject(XRNode _xrNode, GameObject _object)
        {
            //Setter(_xrNode, leftInteractObject, rightInteractObject, _object);
            if (_xrNode == XRNode.RightHand)
            {
                rightInteractObject = _object;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                leftInteractObject = _object;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }


        private Vector3 Getter(XRNode _xrNode, Vector3 _left, Vector3 _right)
        {
            if (_xrNode == XRNode.RightHand)
            {
                return _right;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return _left;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return Vector3.zero;
            }
        }
        private Vector2 Getter(XRNode _xrNode, Vector2 _left, Vector2 _right)
        {
            if (_xrNode == XRNode.RightHand)
            {
                return _right;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return _left;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return Vector2.zero;
            }
        }
        private float Getter(XRNode _xrNode, float _left, float _right)
        {
            if (_xrNode == XRNode.RightHand)
            {
                return _right;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return _left;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return 0.0f;
            }
        }
        private bool Getter(XRNode _xrNode, bool _left, bool _right)
        {
            if (_xrNode == XRNode.RightHand)
            {
                return _right;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return _left;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return false;
            }
        }
        private GameObject Getter(XRNode _xrNode, GameObject _left, GameObject _right)
        {
            if (_xrNode == XRNode.RightHand)
            {
                return _right;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                return _left;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
                return null;
            }
        }

        private void Setter(XRNode _xrNode, Vector3 _left, Vector3 _right, Vector3 _input)
        {
            if (_xrNode == XRNode.RightHand)
            {
                _right = _input;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                _left = _input;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }
        private void Setter(XRNode _xrNode, Vector2 _left, Vector2 _right, Vector2 _input)
        {
            if (_xrNode == XRNode.RightHand)
            {
                _right = _input;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                _left = _input;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }
        private void Setter(XRNode _xrNode, float _left, float _right, float _input)
        {
            if (_xrNode == XRNode.RightHand)
            {
                _right = _input;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                _left = _input;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }
        private void Setter(XRNode _xrNode, bool _left, bool _right, bool _input)
        {
            if (_xrNode == XRNode.RightHand)
            {
                _right = _input;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                _left = _input;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }
        }
        private void Setter(XRNode _xrNode, GameObject _left, GameObject _right, GameObject _input)
        {
            if (_xrNode == XRNode.RightHand)
            {
                _right = _input;
            }
            else if (_xrNode == XRNode.LeftHand)
            {
                _left = _input;
            }
            else
            {
                Debug.LogError("Not Assigned Hand Controller");
            }


        }

        #endregion
    }
}