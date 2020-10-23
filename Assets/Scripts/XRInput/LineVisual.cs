using UnityEngine;

namespace Crengine.XRInput
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineVisual : MonoBehaviour
    {
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform midPoint;
        [SerializeField] private Transform endPoint;

        private Vector3 followPoint;
        public Vector3 FollowPoint { get { return followPoint; } set { followPoint = value; } }

        private Vector3 localToGlobalPoint;
        public Vector3 LocalToGlobalPoint { get { return localToGlobalPoint; } set { localToGlobalPoint = value; } }

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