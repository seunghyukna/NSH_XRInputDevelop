using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRInputManager = XRInput.Core.XRInputStateManager;

public class XRDraggableUI : MonoBehaviour
{
    private GraphicRaycaster graphicRaycaster;
    private List<RaycastResult> rightResults = new List<RaycastResult>();
    private List<RaycastResult> leftResults = new List<RaycastResult>();
    private PointerEventData rightPointerEventData;
    private PointerEventData leftPointerEventData;

    public XRInteractorLineVisual rightLineVisual;

    private Vector3 interactOffset;

    private void Start()
    {
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        rightPointerEventData = new PointerEventData(EventSystem.current);
        leftPointerEventData = new PointerEventData(EventSystem.current);


    }

    private void Update()
    {
        RaycastHit hit;
        Physics.Raycast

        //rightPointerEventData.position = Camera.main.WorldToScreenPoint(XRInputManager.Instance.GetDeviceHitPosition(XRNode.RightHand));
        //graphicRaycaster.Raycast(rightPointerEventData, rightResults);

        //leftPointerEventData.position = Camera.main.WorldToScreenPoint(XRInputManager.Instance.GetDeviceHitPosition(XRNode.LeftHand));
        //graphicRaycaster.Raycast(leftPointerEventData, leftResults);

        //if (rightResults.Count <= 0 || leftResults.Count <= 0)
        //    return;

        //DetectRay();
        //rightResults.Clear();
        //leftResults.Clear();

        //var ped = new PointerEventData(EventSystem.current);
        //ped.position = Camera.main.WorldToScreenPoint(XRInputManager.Instance.rightHitPosition);
        //graphicRaycaster.Raycast(ped, results);

        //if (results.Count <= 0)
        //    return;

        //DetectRay();
        //results.Clear();
    }

    private void DetectRay()
    {
        if (rightResults[0].gameObject.name == "GrabbablePanel")
        {
            if (XRInputManager.Instance.GetDeviceInteractObject(XRNode.RightHand) != null)
                return;

            XRInputManager.Instance.SetDeviceInteractObject(XRNode.RightHand, transform.gameObject);
            if (XRInputManager.Instance.GetDeviceTriggerState(XRNode.RightHand))
            {
                XRInputManager.Instance.SetDeviceInteractingState(XRNode.RightHand, true);
                transform.position = XRInputManager.Instance.GetDeviceHitPosition(XRNode.RightHand);
            }
        }
    }
}
