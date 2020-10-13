using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRRayHit = XRInput.Core.XRRayHitPosition;

public class XRRaySelect : XRRayInteractorSubSystem
{
    [Header("XR Select Input")]
    [SerializeField] private float rayMaxLength;

    [Header("XR Interact Drag")]
    //[SerializeField] private InputHelpers.Button dragUsage;
    [SerializeField] private bool dragUse;
    [SerializeField] private float dragSpeed = 5f;

    [Header("XR Interact Raoate")]
    //[SerializeField] private InputHelpers.Button rotateUsage;
    [SerializeField] private bool rotateUse;
    [SerializeField] private float rotateSpeed = 10f;


    private Vector3 interactOffset;
    private Vector2 dragInput;
    

    private bool isSelecting;

    private bool isTriggerPress;
    private bool isDragPress;

    private void Start()
    {
        baseInteractor = GetComponent<XRRayInteractor>();
        rayInteractor = GetComponent<XRRayInteractor>();
        lineRenderer = GetComponent<LineRenderer>();
        lineVisual = GetComponent<XRInteractorLineVisual>();

        baseInteractor.onSelectEnter.AddListener(OnSelectEnterSubSystem);
        baseInteractor.onSelectExit.AddListener(OnSelectExitSubSystem);

        MatchLineLength();
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);
        Debug.DrawRay(transform.position, transform.forward * rayMaxLength, Color.blue);

        if (!device.isValid)
        {
            GetDevice();
        }

        if (isSelecting)
        {
            HoldInteractable(ray);
        }
    }

    private void HoldInteractable(Ray _ray)
    {
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPress))
        {
            baseInteractable.transform.position = _ray.GetPoint(interactLength) + interactOffset;
        }

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
        if (device.TryGetFeatureValue(CommonUsages.secondary2DAxis, out dragInput))
        {
            
            if (dragInput.y > 0.3f && interactLength < rayMaxLength)
            {
                interactLength += (Time.deltaTime * dragSpeed);
            }
            else if (dragInput.y < -0.3f && interactLength > 0f)
            {
                interactLength -= (Time.deltaTime * dragSpeed);
            }
        }
    }

    private void RotateInteractable(Ray _ray)
    {

    }

    private void MatchLineLength()
    {
        rayInteractor.maxRaycastDistance = rayMaxLength;
        lineVisual.lineLength = rayMaxLength;
    }

    private void OnSelectEnterSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Select Enter : " + _interactable.name);

        baseInteractable = baseInteractor.selectTarget;
        GetInteractPosition();
        XRRayHit.Instance.HitPosition = interactPoint;
        interactLength = Vector3.Distance(transform.position, interactPoint);
        Ray ray = new Ray(transform.position, transform.forward * rayMaxLength);
        interactOffset = baseInteractable.transform.position - ray.GetPoint(interactLength);

        isSelecting = true;
    }

    private void OnSelectExitSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Select Exit");

        baseInteractable = null;
        interactLength = 0.0f;
        isSelecting = false;
    }
}
