using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRInputManager = Crengine.XRInput.Core.XRInputStateManager;

namespace Crengine.XRInput
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineVisual : MonoBehaviour
    {
        [SerializeField] private Vector3 followPoint;
        public Vector3 FollowPoint { get { return followPoint; } set { followPoint = value; } }

        public Vector3 localToGlobalPoint;

        public Transform startPoint;
        public Transform midPoint;
        public Transform endPoint;

        public void LineVisualize(bool _isGrab)
        {
            startPoint.position = transform.position;

            if (!_isGrab)
            {
                // Standard state

                midPoint.position = transform.position + (followPoint - transform.position) / 2;

                endPoint.position = followPoint;
            }
            else
            {
                // Grabbing state

                midPoint.position = transform.position + (followPoint - transform.position) / 2;

                endPoint.position = localToGlobalPoint;
            }
        }
    }
}