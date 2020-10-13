using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using XRLogger = XRInput.Core.XRDebugLogger;

public class XRRaySelect : XRRayInteractorSubSystem
{
    [Header("XR Ray Hover Input")]
    [SerializeField] private bool dragUse;
    [SerializeField] private InputHelpers.Button dragUsage;
    [SerializeField] private bool rotateUse;
    [SerializeField] private InputHelpers.Button rotateUsage;

    [SerializeField] private float rayLength;

    private Vector3 interactOffset;

    private bool isSelecting;

    private bool isTriggerPress;

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
        if (!device.isValid)
        {
            GetDevice();
        }

        if (isSelecting)
        {
            InteractSelect();
        }
    }

    private void InteractSelect()
    {
        Ray ray = new Ray(transform.position, transform.forward * rayLength);
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.blue);

        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out isTriggerPress))
        {
            baseInteractable.transform.position = ray.GetPoint(interactLength) + interactOffset;
        }

        if (dragUse)
        {
            DragInteractable(ray);
        }
        if (rotateUse)
        {
            RotateInteractable(ray);
        }
    }

    private void DragInteractable(Ray _ray)
    {

    }

    private void RotateInteractable(Ray _ray)
    {

    }

    private void MatchLineLength()
    {
        rayInteractor.maxRaycastDistance = rayLength;
        lineVisual.lineLength = rayLength;
    }

    private void OnSelectEnterSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Select Enter : " + _interactable.name);

        baseInteractable = baseInteractor.selectTarget;
        GetInteractPosition();

        interactLength = Vector3.Distance(transform.position, interactPoint);
        Ray ray = new Ray(transform.position, transform.forward * rayLength);
        //interactOffset = ray.GetPoint(interactLength) - baseInteractable.transform.position;
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
