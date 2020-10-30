using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using XRInputManager = Crengine.XRInput.Core.XRInputStateManager;

namespace Crengine.XRInput.Core
{
    public class XRDraggableUI : MonoBehaviour
    {
        [Header("Manipulate Plate")]
        public UnityEvent event1;

        private GraphicRaycaster graphicRaycaster;
        private List<RaycastResult> rightResults = new List<RaycastResult>();
        private List<RaycastResult> leftResults = new List<RaycastResult>();
        private PointerEventData rightPointerEventData;
        private PointerEventData leftPointerEventData;

        private void Start()
        {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            rightPointerEventData = new PointerEventData(EventSystem.current);
            leftPointerEventData = new PointerEventData(EventSystem.current);
        }

        private void Update()
        {
            DetectRightRay();
            DetectLeftRay();
        }

        private void DetectRightRay()
        {
            rightPointerEventData.position = Camera.main.WorldToScreenPoint(XRInputManager.Instance.GetDeviceHitPosition(XRNode.RightHand));
            graphicRaycaster.Raycast(rightPointerEventData, rightResults);

            if (rightResults.Count <= 0)
            {
                return;
            }

            if (rightResults[0].gameObject.name == "GrabbablePanel")
            {
                // Compare interacting object / this
                if (XRInputManager.Instance.GetDeviceInteractObject(XRNode.RightHand) != null &&
                    XRInputManager.Instance.GetDeviceInteractObject(XRNode.RightHand) != transform.gameObject)
                    return;

                // Dragging
                if (XRInputManager.Instance.GetDeviceTriggerState(XRNode.RightHand))
                {
                    XRInputManager.Instance.SetDeviceInteractObject(XRNode.RightHand, transform.gameObject);
                    XRInputManager.Instance.SetDeviceInteractingState(XRNode.RightHand, true);
                    // Initialize interact length
                    if (XRInputManager.Instance.GetDeviceInteractLength(XRNode.RightHand) == 0.0f)
                    {
                        XRInputManager.Instance.SetDeviceInteractLength(XRNode.RightHand,
                            Vector3.Distance(XRInputManager.Instance.rightController.transform.position,
                            XRInputManager.Instance.GetDeviceHitPosition(XRNode.RightHand)));
                    }
                    // Initialize interact offset
                    if (XRInputManager.Instance.GetDeviceInteractOffset(XRNode.RightHand) == Vector3.zero)
                    {
                        XRInputManager.Instance.SetDeviceInteractOffet(XRNode.RightHand, transform.position,
                            XRInputManager.Instance.GetDeviceHitPosition(XRNode.RightHand));
                    }
                }
                // Clear interact values
                else if (!XRInputManager.Instance.GetDeviceTriggerState(XRNode.RightHand))
                {
                    XRInputManager.Instance.SetDeviceInteractObject(XRNode.RightHand, null);
                    XRInputManager.Instance.SetDeviceInteractingState(XRNode.RightHand, false);
                    XRInputManager.Instance.SetDeviceInteractOffet(XRNode.RightHand, Vector3.zero, Vector3.zero);
                    XRInputManager.Instance.SetDeviceInteractLength(XRNode.RightHand, 0.0f);
                }
            }

            rightResults.Clear();
        }

        private void DetectLeftRay()
        {
            leftPointerEventData.position = Camera.main.WorldToScreenPoint(XRInputManager.Instance.GetDeviceHitPosition(XRNode.LeftHand));
            graphicRaycaster.Raycast(leftPointerEventData, leftResults);

            if (leftResults.Count <= 0)
            {
                return;
            }

            if (leftResults[0].gameObject.name == "GrabbablePanel")
            {
                // Compare interacting object / this
                if (XRInputManager.Instance.GetDeviceInteractObject(XRNode.LeftHand) != null &&
                    XRInputManager.Instance.GetDeviceInteractObject(XRNode.LeftHand) != transform.gameObject)
                    return;

                if (XRInputManager.Instance.GetDeviceTriggerState(XRNode.LeftHand))
                {
                    // Dragging
                    XRInputManager.Instance.SetDeviceInteractObject(XRNode.LeftHand, transform.gameObject);
                    XRInputManager.Instance.SetDeviceInteractingState(XRNode.LeftHand, true);
                    // Initialize interact length
                    if (XRInputManager.Instance.GetDeviceInteractLength(XRNode.LeftHand) == 0.0f)
                    {
                        XRInputManager.Instance.SetDeviceInteractLength(XRNode.LeftHand,
                            Vector3.Distance(XRInputManager.Instance.leftController.transform.position,
                            XRInputManager.Instance.GetDeviceHitPosition(XRNode.LeftHand)));
                    }
                    // Initialize interact offset
                    if (XRInputManager.Instance.GetDeviceInteractOffset(XRNode.LeftHand) == Vector3.zero)
                    {
                        XRInputManager.Instance.SetDeviceInteractOffet(XRNode.LeftHand, transform.position,
                            XRInputManager.Instance.GetDeviceHitPosition(XRNode.LeftHand));
                    }
                }
                // Clear interact values
                else if (!XRInputManager.Instance.GetDeviceTriggerState(XRNode.LeftHand))
                {
                    XRInputManager.Instance.SetDeviceInteractObject(XRNode.LeftHand, null);
                    XRInputManager.Instance.SetDeviceInteractingState(XRNode.LeftHand, false);
                    XRInputManager.Instance.SetDeviceInteractOffet(XRNode.LeftHand, Vector3.zero, Vector3.zero);
                    XRInputManager.Instance.SetDeviceInteractLength(XRNode.LeftHand, 0.0f);
                }
            }

            leftResults.Clear();
        }
    }
}