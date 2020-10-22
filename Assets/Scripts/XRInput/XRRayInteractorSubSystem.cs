using System.Collections.Generic;
using UnityEngine;
using Crengine.XRInput.Core;
using UnityEngine.XR.Interaction.Toolkit;
using XRLogger = Crengine.XRInput.Core.XRDebugLogger;
using XRInputManager = Crengine.XRInput.Core.XRInputStateManager;

namespace Crengine.XRInput
{
    public class XRRayInteractorSubSystem : XRInteractorSubSystem
    {
        private XRRayInteractor rayInteractor;
        private LineRenderer lineRenderer;
        private XRInteractorLineVisual lineVisual;
        private Vector3 currentInteractPoint;
        private Vector3 interactedPoint;
        private Vector3 interactorPoint;
        private float interactLength;
        private RaycastHit currentHit;
        [SerializeField] private float rayMaxLength;
        [SerializeField] private float dragCoefficient;

        [SerializeField] private LineVisual myLineVisual;

        [Header("Hover")]
        [SerializeField] private bool hoverUse;
        private List<GameObject> hoveringObjects = new List<GameObject>();
        private bool isHovering;
        //private XRLinerendererExtend lineExtend;

        [Header("Grab")]
        [SerializeField] private bool grabUse;
        private GameObject grabbingObject;
        private bool isGrabbing;

        [Header("Outline")]
        [SerializeField] private bool outlineUse;
        [SerializeField] private Color outlineColor;
        [SerializeField] [Range(0,10)] private float outlineWidth;
        private List<Outline> outlines = new List<Outline>();

        [Header("Grab Drag")]
        //[SerializeField] private InputHelpers.Button dragUsage;
        [SerializeField] private bool dragUse;
        [SerializeField] private float dragSpeed = 5f;

        [Header("Grab Raoate")]
        //[SerializeField] private InputHelpers.Button rotateUsage;
        [SerializeField] private bool rotateUse;
        [SerializeField] private float rotateSpeed = 10f;


        private void Start()
        {
            baseInteractor = GetComponent<XRBaseInteractor>();
            rayInteractor = GetComponent<XRRayInteractor>();
            lineRenderer = GetComponent<LineRenderer>();
            lineVisual = GetComponent<XRInteractorLineVisual>();

            baseInteractor.onHoverEnter.AddListener(OnHoverEnterInteractable);
            baseInteractor.onHoverExit.AddListener(OnHoverExitInteractable);
            baseInteractor.onSelectEnter.AddListener(OnSelectEnterInteractable);
            baseInteractor.onSelectExit.AddListener(OnSelectExitInteractable);

            MatchLineLength();
        }

        private void Update()
        {
            if (!device.isValid)
            {
                GetDevice();
            }

            DetectControllerUse();
            Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);
            Debug.DrawRay(transform.position, transform.forward * rayMaxLength, Color.green);

            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                ray = new Ray(transform.position, transform.forward * XRInputManager.Instance.GetDeviceInteractLength(xrNode));
                Debug.DrawRay(transform.position, transform.forward * XRInputManager.Instance.GetDeviceInteractLength(xrNode), Color.blue);
            }

            // Get last ray position sequentially
            GetInteractPosition(ray);
            XRInputManager.Instance.SetDeviceHitPosition(xrNode, currentInteractPoint);

            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                HoldInteractable(ray);   
            }
            
            myLineVisual.LineVisualize(isGrabbing);
        }

        #region Initialization

        private void GetInteractPosition(Ray _ray)
        {
            RaycastHit hit;
            Physics.Raycast(_ray, out hit);
            currentInteractPoint = transform.position + _ray.direction * rayMaxLength;
            //if (hit.transform != null)
            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, hit.transform.position), Color.black);
                currentInteractPoint = transform.position + _ray.direction * Vector3.Distance(transform.position, hit.transform.position);
            }

            myLineVisual.FollowPoint = currentInteractPoint;
        }

        private void MatchLineLength()
        {
            rayInteractor.maxRaycastDistance = rayMaxLength;
            lineVisual.lineLength = rayMaxLength;
        }

        #endregion

        #region Hovering

        private void OutlineActivate()
        {
            if (!outlineUse)
                return;

            int currentIdx = hoveringObjects.Count - 1;
             
            if (hoveringObjects[currentIdx].GetComponent<Outline>() == null)
                hoveringObjects[currentIdx].gameObject.AddComponent<Outline>();

            outlines.Add(hoveringObjects[currentIdx].GetComponent<Outline>());
            outlines[currentIdx].enabled = false;
            outlines[currentIdx].OutlineMode = Outline.Mode.OutlineAll;
            outlines[currentIdx].OutlineColor = outlineColor;
            outlines[currentIdx].OutlineWidth = outlineWidth;
        }

        private void OutlineDeactivate(XRBaseInteractable _interactable)
        {
            if (!outlineUse || outlines.Count == 0)
                return;

            int currentIdx = outlines.IndexOf(_interactable.gameObject.GetComponent<Outline>());
            outlines[currentIdx].enabled = false;
            outlines.RemoveAt(currentIdx);
        }

        private void ActiveOutlineCurrentHit()
        {
            if (currentHit.transform == null)
                return;
            
            for (int i = 0; i < outlines.Count; i++)
            {
                if (outlines[i].transform.gameObject == currentHit.transform.gameObject)
                    outlines[i].enabled = true;
                else
                    outlines[i].enabled = false;
            }
        }

        #endregion

        #region Grabbing

        private void HoldInteractable(Ray _ray)
        {
            //if (dragUse)
            //{
            //    if (Vector3.Distance(transform.position, interactedPoint) < interactLength &&
            //        XRInputManager.Instance.GetDeviceInteractLength(xrNode) < rayMaxLength)
            //    {
            //        Debug.Log("Object Far");

            //        XRInputManager.Instance.SetDeviceInteractLength(xrNode, 
            //            XRInputManager.Instance.GetDeviceInteractLength(xrNode) + (Time.deltaTime * dragSpeed));
            //    }
            //    else if (Vector3.Distance(transform.position, interactedPoint) > interactLength &&
            //        XRInputManager.Instance.GetDeviceInteractLength(xrNode) > 0f)
            //    {
            //        Debug.Log("Object Close");

            //        XRInputManager.Instance.SetDeviceInteractLength(xrNode, 
            //            XRInputManager.Instance.GetDeviceInteractLength(xrNode) - (Time.deltaTime * dragSpeed));
            //    }

            //    //DragInteractable(_ray);
            //}

            if (dragUse)
            {
                
            }

            if (rotateUse)
            {
                // face

                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.rotation = 
                    Quaternion.LookRotation(XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position - transform.position, Vector3.up);
                //RotateInteractable(_ray);
            }
            myLineVisual.localToGlobalPoint =
                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position -
                XRInputManager.Instance.GetDeviceInteractOffset(xrNode);

            XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position =
                Vector3.MoveTowards(XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position,
                _ray.GetPoint(XRInputManager.Instance.GetDeviceInteractLength(xrNode)) + XRInputManager.Instance.GetDeviceInteractOffset(xrNode),
                0.05f);

            //XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position =
            //    Vector3.Lerp(XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position,
            //    _ray.GetPoint(XRInputManager.Instance.GetDeviceInteractLength(xrNode)) + XRInputManager.Instance.GetDeviceInteractOffset(xrNode),
            //    0.3f);
        }

        private void InteractLerp(Vector3 _start, Vector3 _end)
        {
            Vector3.Lerp(_start, _end, Time.deltaTime);
        }

        // Drag with secondaryAxis2D
        private void DragInteractable(Ray _ray)
        {
            if (secondaryVector.y > 0.3f && XRInputManager.Instance.GetDeviceInteractLength(xrNode) < rayMaxLength)
            {
                XRInputManager.Instance.SetDeviceInteractLength(xrNode, XRInputManager.Instance.GetDeviceInteractLength(xrNode) + (Time.deltaTime * dragSpeed));
            }
            else if (secondaryVector.y < -0.3f && XRInputManager.Instance.GetDeviceInteractLength(xrNode) > 0f)
            {
                XRInputManager.Instance.SetDeviceInteractLength(xrNode, XRInputManager.Instance.GetDeviceInteractLength(xrNode) - (Time.deltaTime * dragSpeed));
            }
        }

        // Rotate with primaryAxis2D
        private void RotateInteractable(Ray _ray)
        {
            if (Mathf.Abs(primaryVector.y) + Mathf.Abs(primaryVector.x) > 0.5f && XRInputManager.Instance.GetDeviceInteractLength(xrNode) < rayMaxLength)
            {
                Vector3 v3 = new Vector3(primaryVector.y, -primaryVector.x, 0);
                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.localEulerAngles +=
                    v3;

                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.RotateAround
                    (XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position,
                    v3, rotateSpeed);
            }
        }

        #endregion

        #region Events

        private void OnHoverEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse)
                return;

            XRLogger.Instance.LogInfo($"Hover Enter : " + _interactable.name);
            
            rayInteractor.GetCurrentRaycastHit(out currentHit);

            hoveringObjects.Add(_interactable.gameObject);
            OutlineActivate();

            ActiveOutlineCurrentHit();

            isHovering = true;

            //lineExtend.ChangeLine(isHovering);
        }

        private void OnHoverExitInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse)
                return;
            XRLogger.Instance.LogInfo($"Hover Exit : " + _interactable.name);

            rayInteractor.GetCurrentRaycastHit(out currentHit);

            ActiveOutlineCurrentHit();
            OutlineDeactivate(_interactable);

            hoveringObjects.Remove(_interactable.gameObject);
            isHovering = false;

            //lineExtend.ChangeLine(isHovering);
        }

        private void OnSelectEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!grabUse || XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
                return;

            XRLogger.Instance.LogInfo($"Select Enter : " + _interactable.name);

            grabbingObject = baseInteractor.selectTarget.gameObject;
            interactedPoint = currentInteractPoint;
            interactorPoint = transform.position;
            interactLength = Vector3.Distance(transform.position, currentInteractPoint);
            isGrabbing = true;

            Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);

            XRInputManager.Instance.SetDeviceInteractingState(xrNode, true);
            XRInputManager.Instance.SetDeviceInteractObject(xrNode, grabbingObject);
            XRInputManager.Instance.SetDeviceInteractLength(xrNode, interactLength);
            XRInputManager.Instance.SetDeviceInteractOffet(xrNode, grabbingObject.transform.position, ray.GetPoint(interactLength));
        }

        private void OnSelectExitInteractable(XRBaseInteractable _interactable)
        {
            if (!grabUse)
                return;

            XRLogger.Instance.LogInfo($"Select Exit : " + _interactable.name);

            grabbingObject = null;
            isGrabbing = false;
            XRInputManager.Instance.SetDeviceInteractingState(xrNode, false);
            XRInputManager.Instance.SetDeviceInteractObject(xrNode, null);
            XRInputManager.Instance.SetDeviceInteractOffet(xrNode, Vector3.zero, Vector3.zero);
            XRInputManager.Instance.SetDeviceInteractLength(xrNode, 0.0f);
        }

        #endregion
    }
}