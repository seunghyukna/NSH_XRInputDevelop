using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRInputManager = XRInput.Core.XRInputStateManager;

namespace XRInput.Core
{
    public class XRRayInteractorSubSystem : XRInteractorSubSystem
    {
        private XRRayInteractor rayInteractor;
        private LineRenderer lineRenderer;
        private XRInteractorLineVisual lineVisual;
        private Vector3 interactPoint;
        private RaycastHit currentHit;
        [SerializeField] private float rayMaxLength;

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

            //if (GetComponent<XRLinerendererExtend>() != null)
            //    lineExtend = GetComponent<XRLinerendererExtend>();

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
            GetInteractPosition();
            XRInputManager.Instance.SetDeviceHitPosition(xrNode, interactPoint);

            Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);
            Debug.DrawRay(transform.position, transform.forward * rayMaxLength, Color.blue);
            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                ray = new Ray(transform.position, transform.forward * XRInputManager.Instance.GetDeviceInteractLength(xrNode));
                Debug.DrawRay(transform.position, transform.forward * XRInputManager.Instance.GetDeviceInteractLength(xrNode), Color.green);
            }

            DetectControllerUse();

            if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            {
                rayInteractor.lineType = XRRayInteractor.LineType.ProjectileCurve;
                HoldInteractable(ray);
            }
            else
            {
                //rayInteractor.lineType = XRRayInteractor.LineType.StraightLine;
            }
        }

        #region Initialization

        private void GetInteractPosition()
        {
            if (lineRenderer.positionCount <= 1)
                return;

            interactPoint = lineRenderer.GetPosition(1);
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
            XRInputManager.Instance.GetDeviceInteractObject(xrNode).transform.position =
                _ray.GetPoint(XRInputManager.Instance.GetDeviceInteractLength(xrNode)) + XRInputManager.Instance.GetDeviceInteractOffset(xrNode);

            if (dragUse)
            {
                DragInteractable(_ray);
            }
            if (rotateUse)
            {
                RotateInteractable(_ray);
            }
        }

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
            
            isGrabbing = true;

            Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);

            XRInputManager.Instance.SetDeviceInteractingState(xrNode, true);
            XRInputManager.Instance.SetDeviceInteractObject(xrNode, grabbingObject);
            XRInputManager.Instance.SetDeviceInteractLength(xrNode, Vector3.Distance(transform.position, interactPoint));
            XRInputManager.Instance.SetDeviceInteractOffet(xrNode, grabbingObject.transform.position,
                ray.GetPoint(XRInputManager.Instance.GetDeviceInteractLength(xrNode)));
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