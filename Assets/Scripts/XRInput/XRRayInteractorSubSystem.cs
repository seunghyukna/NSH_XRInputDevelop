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
        [SerializeField] Transform player;
        private XRRayInteractor rayInteractor;
        private LineVisual lineVisual;
        [SerializeField] private float rayMaxLength;
        private Vector3 currentInteractPoint;
        private RaycastHit currentHit;
        private Vector3 interactorPoint;
        private float interactLength;
        

        [Header("Hover")]
        [SerializeField] private bool hoverUse;
        private List<GameObject> hoveringObjects = new List<GameObject>();
        private bool isHovering;

        [Header("Select")]
        [SerializeField] private bool selectUse;
        private GameObject selectedObject;
        private bool isSelected;

        [Header("Outline")]
        [SerializeField] private bool outlineUse;
        [SerializeField] private Color outlineColor;
        [SerializeField] [Range(0,10)] private float outlineWidth;
        private List<Outline> outlines = new List<Outline>();

        [Header("Grab Drag")]
        [SerializeField] private bool dragUse;
        [SerializeField] private float dragSpeed = 0.1f;
        [SerializeField] private float pullingAmount = 50f;

        //[Header("Grab Raoate")]
        //[SerializeField] private bool rotateUse;
        //[SerializeField] private float rotateSpeed = 10f;

        XRControlTypeManager controlTypeManager;
        private void Awake()
        {
            controlTypeManager = player.GetComponent<XRControlTypeManager>();
            if (controlTypeManager.Interactor != InteractorType.Ray)
            {
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            baseInteractor = GetComponent<XRBaseInteractor>();
            rayInteractor = GetComponent<XRRayInteractor>();
            lineVisual = GetComponent<LineVisual>();

            baseInteractor.onHoverEnter.AddListener(OnHoverEnterInteractable);
            baseInteractor.onHoverExit.AddListener(OnHoverExitInteractable);
            baseInteractor.onSelectEnter.AddListener(OnSelectEnterInteractable);
            baseInteractor.onSelectExit.AddListener(OnSelectExitInteractable);

            MatchLineLength();
        }

        private void Update()
        {
            // Get my XRNode
            if (!device.isValid)
                GetDevice();

            // Catch current XRNode button use
            DetectControllerUse();

            Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);

            // Control ray length when interact
            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                ray = new Ray(transform.position, transform.forward * XRInputManager.Instance.GetDeviceInteractLength(xrNode));
            }

            // Get ray end point sequentially
            GetInteractPosition(ray);
            XRInputManager.Instance.SetDeviceHitPosition(xrNode, currentInteractPoint);

            // hold & drag interacted obejct
            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                HoldInteractable(ray);   
            }

            // Update LineRenderer
            lineVisual.LineVisualize(isSelected);
        }

        #region Initialization

        private void GetInteractPosition(Ray _ray)
        {
            RaycastHit hit;
            Physics.Raycast(_ray, out hit);

            // hit object
            if (hit.transform != null)
            {
                currentInteractPoint = transform.position + _ray.direction * Vector3.Distance(transform.position, hit.transform.position);
                lineVisual.FollowPoint = currentInteractPoint;

                return;
            }
            
            // grab object
            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {                
                currentInteractPoint = transform.position + _ray.direction * 
                    XRInputManager.Instance.GetDeviceInteractLength(xrNode);
                lineVisual.FollowPoint = currentInteractPoint;

                return;
            }

            // no hit object
            currentInteractPoint = transform.position + _ray.direction * rayMaxLength;
            lineVisual.FollowPoint = currentInteractPoint;
        }

        private void MatchLineLength()
        {
            rayInteractor.maxRaycastDistance = rayMaxLength;
        }

        #endregion

        #region Hovering

        private void OutlineActivate(XRBaseInteractable _interactable)
        {
            if (!outlineUse)
                return;

            hoveringObjects.Add(_interactable.gameObject);
            int currentIdx = hoveringObjects.Count - 1;
             
            // Attach Outline component
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
            hoveringObjects.Remove(_interactable.gameObject);
        }

        private void ManageOutlineActivate()
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
            // Move interactable obejct 
            if (dragUse)
            {
                float playerInteractorDistance = Vector3.Distance(player.position, interactorPoint);
                float currentDistance = Vector3.Distance(player.position, transform.position);
                float distanceOffset = currentDistance - playerInteractorDistance;
                float newLength = interactLength + distanceOffset * pullingAmount;

                XRInputManager.Instance.SetDeviceInteractLength(xrNode, Mathf.Clamp(newLength, 1, rayMaxLength));

                // Manipulate interactable object
                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position =
                    Vector3.Lerp(XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position,
                    _ray.GetPoint(XRInputManager.Instance.GetDeviceInteractLength(xrNode)) + XRInputManager.Instance.GetDeviceInteractOffset(xrNode),
                    dragSpeed);
            }

            // Rotate interactable object
            //if (rotateUse)
            //{

            //}

            // Fix linerenderer end point to first interacted point
            lineVisual.LocalToGlobalPoint =
                XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position - XRInputManager.Instance.GetDeviceInteractOffset(xrNode);
        }

        #endregion

        #region Events

        private void OnHoverEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse || isSelected)
                return;

            XRLogger.Instance.LogInfo($"Hover Enter : " + _interactable.name);

            // Interactable object is UI
            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIHoverEnter(_interactable.gameObject, baseInteractor);
            }
            
            rayInteractor.GetCurrentRaycastHit(out currentHit);
            OutlineActivate(_interactable);
            ManageOutlineActivate();
            isHovering = true;
        }

        private void OnHoverExitInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse || isSelected)
                return;

            XRLogger.Instance.LogInfo($"Hover Exit : " + _interactable.name);

            // Interactable object is UI
            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIHoverExit(_interactable.gameObject, baseInteractor);
            }

            rayInteractor.GetCurrentRaycastHit(out currentHit);
            ManageOutlineActivate();
            OutlineDeactivate(_interactable);
            isHovering = false;
        }

        private void OnSelectEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!selectUse || XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
                return;

            XRLogger.Instance.LogInfo($"Select Enter : " + _interactable.name);

            // Interactable object is UI
            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIManipulateStart(_interactable.gameObject, baseInteractor);
            }

            selectedObject = baseInteractor.selectTarget.gameObject;
            interactLength = Vector3.Distance(transform.position, currentInteractPoint);
            interactorPoint = transform.position;
            isSelected = true;

            Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);

            XRInputManager.Instance.SetDeviceInteractingState(xrNode, true);
            XRInputManager.Instance.SetDeviceInteractObject(xrNode, selectedObject);
            XRInputManager.Instance.SetDeviceInteractLength(xrNode, interactLength);
            XRInputManager.Instance.SetDeviceInteractOffet(xrNode, selectedObject.transform.position, ray.GetPoint(interactLength));
        }

        private void OnSelectExitInteractable(XRBaseInteractable _interactable)
        {
            if (!selectUse)
                return;

            XRLogger.Instance.LogInfo($"Select Exit : " + _interactable.name);

            // Interactable object is UI
            if (_interactable.gameObject.layer == 5)
            {
                XRUIManipulateProvider.InvokeUIManipulateEnd(_interactable.gameObject, baseInteractor);
            }

            selectedObject = null;
            interactLength = 0.0f;
            interactorPoint = Vector3.zero;
            isSelected = false;

            XRInputManager.Instance.SetDeviceInteractingState(xrNode, false);
            XRInputManager.Instance.SetDeviceInteractObject(xrNode, null);
            XRInputManager.Instance.SetDeviceInteractLength(xrNode, 0.0f);
            XRInputManager.Instance.SetDeviceInteractOffet(xrNode, Vector3.zero, Vector3.zero);
        }

        #endregion
    }
}