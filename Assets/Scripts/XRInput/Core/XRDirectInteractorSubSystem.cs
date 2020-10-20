using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRInputManager = XRInput.Core.XRInputStateManager;

namespace XRInput.Core
{
    public class XRDirectInteractorSubSystem : XRInteractorSubSystem
    {
        private XRDirectInteractor directInteractor;
        private Vector3 interactPoint;

        [Header("Hover")]
        [SerializeField] private bool hoverUse;
        private List<GameObject> hoveringObjects = new List<GameObject>();
        private bool isHovering;

        [Header("Grab")]
        [SerializeField] private bool grabUse;
        private GameObject grabbingObject;
        private bool isGrabbing;

        [Header("Outline")]
        [SerializeField] private bool outlineUse;
        [SerializeField] private Color outlineColor;
        [SerializeField] [Range(0, 10)] private float outlineWidth;
        private List<Outline> outlines = new List<Outline>();

        private void Start()
        {
            baseInteractor = GetComponent<XRBaseInteractor>();
            directInteractor = GetComponent<XRDirectInteractor>();

            baseInteractor.onHoverEnter.AddListener(OnHoverEnterInteractable);
            baseInteractor.onHoverExit.AddListener(OnHoverExitInteractable);
            baseInteractor.onSelectEnter.AddListener(OnSelectEnterInteractable);
            baseInteractor.onSelectExit.AddListener(OnSelectExitInteractable);
        }

        private void OutlineActivate()
        {
            if (!outlineUse)
                return;

            int currentIdx = hoveringObjects.Count - 1;

            if (hoveringObjects[currentIdx].GetComponent<Outline>() == null)
                hoveringObjects[currentIdx].gameObject.AddComponent<Outline>();

            outlines.Add(hoveringObjects[currentIdx].GetComponent<Outline>());
            outlines[currentIdx].enabled = true;
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

        #region Events

        private void OnHoverEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse)
                return;

            XRLogger.Instance.LogInfo($"Hover Enter : " + _interactable.name);

            hoveringObjects.Add(_interactable.gameObject);
            OutlineActivate();

            isHovering = true;
        }

        private void OnHoverExitInteractable(XRBaseInteractable _interactable)
        {
            if (!hoverUse)
                return;

            XRLogger.Instance.LogInfo($"Hover Exit : " + _interactable.name);

            OutlineDeactivate(_interactable);
            hoveringObjects.Remove(_interactable.gameObject);

            isHovering = false;
        }

        private void OnSelectEnterInteractable(XRBaseInteractable _interactable)
        {
            if (!grabUse || XRInputManager.Instance.GetDeviceInteractObject(xrNode) != null)
                return;

            XRLogger.Instance.LogInfo($"Select Enter : " + _interactable.name);

            isGrabbing = true;
        }

        private void OnSelectExitInteractable(XRBaseInteractable _interactable)
        {
            if (!grabUse)
                return;

            XRLogger.Instance.LogInfo($"Select Exit : " + _interactable.name);

            isGrabbing = false;
        }

        #endregion
    }
}