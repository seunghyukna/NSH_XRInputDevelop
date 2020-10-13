using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;
using XRLogger = XRInput.Core.XRDebugLogger;

public class XRRayHover : XRRayInteractorSubSystem
{
    [Header("XR Ray Hover Input")]
    private bool isHovering;

    // Add OutLines
    // Add Linerenderer Transition

    private void Start()
    {
        baseInteractor = GetComponent<XRRayInteractor>();
        rayInteractor = GetComponent<XRRayInteractor>();
        lineRenderer = GetComponent<LineRenderer>();

        baseInteractor.onHoverEnter.AddListener(OnHoverEnterSubSystem);
        baseInteractor.onHoverExit.AddListener(OnHoverExitSubSystem);
    }

    private void Update()
    {
        if (isHovering)
        {
            //GetInteractPosition();
        }
    }

    private void OnHoverEnterSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Hover Enter : " + _interactable.name);

        isHovering = true;
    }

    private void OnHoverExitSubSystem(XRBaseInteractable _interactable)
    {
        XRLogger.Instance.LogInfo($"Hover Exit");

        isHovering = false;
    }
}
