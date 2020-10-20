using UnityEngine;
using UnityEngine.XR;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRInputManager = XRInput.Core.XRInputStateManager;

namespace XRInput.Core
{
    public class XRFloatingUI : MonoBehaviour
    {
        [SerializeField] private XRNode xrNode;
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform offset;
        [SerializeField] private Camera cam;
        [SerializeField] private bool isActive;
        private bool buttonTrigger;

        public CanvasGroup mainPage;
        public CanvasGroup page1;
        public CanvasGroup page2;
        public CanvasGroup page3;

        private void Update()
        {
            if (isActive)
            {
                FollowingOffset();
            }

            FloatingUIActivate();
        }


        private void FloatingUIActivate()
        {
            if (XRInputManager.Instance.GetDeviceMenuState(xrNode) && !buttonTrigger)
            {
                if (!isActive)
                {
                    ActiveCanvasGroup(mainPage);

                    isActive = true;
                }
                else
                {
                    InactiveCanvasGroup(mainPage);

                    isActive = false;
                }
            }

            if (XRInputManager.Instance.GetDeviceMenuState(xrNode))
            {
                buttonTrigger = true;
            }
            else if (!XRInputManager.Instance.GetDeviceMenuState(xrNode))
            {
                buttonTrigger = false;
            }
        }

        private void FollowingOffset()
        {
            canvas.position = offset.position;
            canvas.rotation = Quaternion.LookRotation(canvas.position - cam.transform.position, Vector3.up);
        }

        public void ActiveCanvasGroup(CanvasGroup _canvasGroup)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void InactiveCanvasGroup(CanvasGroup _canvasGroup)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}