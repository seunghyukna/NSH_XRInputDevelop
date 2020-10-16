using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRInputManager = XRInput.Core.XRInputStateManager;

public class XRRayInteractorSubSystem : MonoBehaviour
{
    [Header("XR Setting")]
    private XRBaseInteractor baseInteractor;
    private XRBaseInteractable baseInteractable;
    private XRRayInteractor rayInteractor;
    private LineRenderer lineRenderer;
    private XRInteractorLineVisual lineVisual;
    [SerializeField] private XRNode xrNode;
    [SerializeField] private List<InputDevice> devices = new List<InputDevice>();
    [SerializeField] private InputDevice device;
    [SerializeField] private float rayMaxLength;

    [Header("XR Ray Hover")]
    [SerializeField] private bool isHovering;

    [Header("XR Ray Select Interact Drag")]
    //[SerializeField] private InputHelpers.Button dragUsage;
    [SerializeField] private bool dragUse;
    [SerializeField] private float dragSpeed = 5f;

    [Header("XR Ray Select Interact Raoate")]
    //[SerializeField] private InputHelpers.Button rotateUsage;
    [SerializeField] private bool rotateUse;
    [SerializeField] private float rotateSpeed = 10f;

    // Device interact information
    private GameObject interactObject;
    private Vector3 interactPoint;
    private Vector3 interactOffset;
    private float interactLength;

    // Device button states
    private bool isTriggerPress;
    private bool isGripPress;
    private Vector2 primaryVector;
    private Vector2 secondaryVector;
    private bool isMenuPress;

    private void Start()
    {
        baseInteractor = GetComponent<XRRayInteractor>();
        rayInteractor = GetComponent<XRRayInteractor>();
        lineRenderer = GetComponent<LineRenderer>();
        lineVisual = GetComponent<XRInteractorLineVisual>();

        baseInteractor.onHoverEnter.AddListener(OnHoverEnterSubSystem);
        baseInteractor.onHoverExit.AddListener(OnHoverExitSubSystem);
        baseInteractor.onSelectEnter.AddListener(OnSelectEnterSubSystem);
        baseInteractor.onSelectExit.AddListener(OnSelectExitSubSystem);

        MatchLineLength();
    }

    private void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        Debug.DrawRay(transform.position, transform.forward * rayMaxLength, Color.blue);
        GetInteractPosition();
        XRInputManager.Instance.SetDeviceHitPosition(xrNode, interactPoint);
        Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);

        DetectButtonUse();

        if (interactObject != null)
        {
            HoldInteractable(ray);
        }
    }

    private void GetInteractPosition()
    {
        if (lineRenderer.positionCount <= 1)
            return;

        interactPoint = lineRenderer.GetPosition(1);
    }

    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    private void MatchLineLength()
    {
        rayInteractor.maxRaycastDistance = rayMaxLength;
        lineVisual.lineLength = rayMaxLength;
    }

    private void HoldInteractable(Ray _ray)
    {
        interactObject.transform.position = _ray.GetPoint(interactLength) + interactOffset;
        
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
        if (secondaryVector.y > 0.3f && interactLength < rayMaxLength)
        {
            interactLength += (Time.deltaTime * dragSpeed);
        }
        else if (secondaryVector.y < -0.3f && interactLength > 0f)
        {
            interactLength -= (Time.deltaTime * dragSpeed);
        }
    }

    private void RotateInteractable(Ray _ray)
    {

    }

    private void DetectButtonUse()
    {
        device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPress);
        XRInputManager.Instance.SetDeviceTriggerState(xrNode, isTriggerPress);

        device.TryGetFeatureValue(CommonUsages.gripButton, out isGripPress);
        XRInputManager.Instance.SetDeviceGripState(xrNode, isGripPress);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out primaryVector);
        XRInputManager.Instance.SetDevicePrimaryState(xrNode, primaryVector);

        device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out secondaryVector);
        XRInputManager.Instance.SetDeviceSecondaryState(xrNode, secondaryVector);

        device.TryGetFeatureValue(CommonUsages.menuButton, out isMenuPress);
        XRInputManager.Instance.SetDeviceMenuState(xrNode, isMenuPress);
    }

    private void OnHoverEnterSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Hover Enter : " + _interactable.name);

        isHovering = true;
    }

    private void OnHoverExitSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Hover Exit : " + _interactable.name);

        isHovering = false;
    }

    private void OnSelectEnterSubSystem(XRBaseInteractable _interactable)
    {
        if (XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
            return;

        XRLogger.Instance.LogInfo($"Select Enter : " + _interactable.name);

        interactObject = baseInteractor.selectTarget.gameObject;
        interactLength = Vector3.Distance(transform.position, interactPoint);

        Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);
        interactOffset = interactObject.transform.position - ray.GetPoint(interactLength);

        XRInputManager.Instance.SetDeviceInteractingState(xrNode, true);
        XRInputManager.Instance.SetDeviceInteractObject(xrNode, interactObject);
    }

    private void OnSelectExitSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Select Exit");

        interactObject = null;
        interactLength = 0.0f;
        XRInputManager.Instance.SetDeviceInteractingState(xrNode, false);
        XRInputManager.Instance.SetDeviceInteractObject(xrNode, null);
    }

}
