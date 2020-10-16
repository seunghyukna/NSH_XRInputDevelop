using UnityEngine;
using UnityEngine.XR;
using XRLogger = XRInput.Core.XRDebugLogger;
using XRInputManager = XRInput.Core.XRInputStateManager;

namespace XRInput.Core
{
    public class XRFloatingUI : MonoBehaviour
    {
        [SerializeField] private Transform canvas;
        [SerializeField] private Transform offset;
        [SerializeField] private Transform cam;
        [SerializeField] private bool isActive;

        public CanvasGroup mainPage;
        public CanvasGroup page1;
        public CanvasGroup page2;
        public CanvasGroup page3;

        private void Update()
        {
            CatchMenuClicked();

            if (isActive)
            {
                FollowingOffset();
            }
        }

        private void CatchMenuClicked()
        {
            if (XRInputManager.Instance.GetDeviceMenuState(XRNode.LeftHand))
            {
                if (!isActive)
                {
                    ActiveFloatingUI();
                }
                else
                {
                    InactiveFloatingUI();
                }
            }
        }

        private void FollowingOffset()
        {
            transform.position = offset.position;
            // canvas must face cam
            // add....
            
        }

        private void ActiveFloatingUI()
        {
            mainPage.alpha = 1;
            mainPage.interactable = true;
            mainPage.blocksRaycasts = true;

            isActive = true;
        }

        private void InactiveFloatingUI()
        {
            mainPage.alpha = 0;
            mainPage.interactable = false;
            mainPage.blocksRaycasts = false;

            isActive = false;
        }

        public void ActiveCanvasGroup(CanvasGroup _canvasGroup)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void InactiveCanvasGroup(CanvasGroup _canvasGroup)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}