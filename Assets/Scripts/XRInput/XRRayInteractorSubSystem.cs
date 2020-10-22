using System.Collections.Generic;
using UnityEngine;
using Crengine.XRInput.Core;
using Crengine.XRInput.UI;
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
        [SerializeField] private float dragSpeed = 0.1f;

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

            // no hit object
            currentInteractPoint = transform.position + _ray.direction * rayMaxLength;

            // hit object
            if (hit.transform != null)
            {
                currentInteractPoint = transform.position + _ray.direction * Vector3.Distance(transform.position, hit.transform.position);
            }
            
            // grab object
            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {                
                currentInteractPoint = transform.position + _ray.direction * 
                    XRInputManager.Instance.GetDeviceInteractLength(xrNode);
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
            if (dragUse)
            {
                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position =
                    Vector3.Lerp(XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position,
                    _ray.GetPoint(XRInputManager.Instance.GetDeviceInteractLength(xrNode)) + XRInputManager.Instance.GetDeviceInteractOffset(xrNode),
                    dragSpeed);
            }

            if (rotateUse)
            {

            }

            myLineVisual.localToGlobalPoint =
                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position - XRInputManager.Instance.GetDeviceInteractOffset(xrNode);
        }

        #endregion

        #region Events

        private void OnHoverEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse || isGrabbing)
                return;

            XRLogger.Instance.LogInfo($"Hover Enter : " + _interactable.name);

            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIHoverEnter(_interactable.gameObject, baseInteractor);
            }
            
            rayInteractor.GetCurrentRaycastHit(out currentHit);

            hoveringObjects.Add(_interactable.gameObject);
            OutlineActivate();

            ActiveOutlineCurrentHit();

            isHovering = true;

            //lineExtend.ChangeLine(isHovering);
        }

        private void OnHoverExitInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse || isGrabbing)
                return;

            XRLogger.Instance.LogInfo($"Hover Exit : " + _interactable.name);

            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIHoverExit(_interactable.gameObject, baseInteractor);
            }

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

            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIManipulateStart(_interactable.gameObject, baseInteractor);
            }

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

            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIManipulateEnd(_interactable.gameObject, baseInteractor);
            }

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