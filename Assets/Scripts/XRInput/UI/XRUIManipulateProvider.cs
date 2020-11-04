using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Crengine.XRInput.UI
{
    /// <summary>
    /// 
    /// User can interact with UI canvas
    /// 
    /// </summary>

    [System.Serializable]
    public class UIHoverEvent : UnityEvent { }

    [System.Serializable]
    public class ManipulateEvent : UnityEvent { }

    public class XRUIManipulateProvider : MonoBehaviour
    {
        public delegate void ManipulateUIEvent(GameObject _object, XRBaseInteractor _interactor);
        public static event ManipulateUIEvent UIHoverEnter;
        public static event ManipulateUIEvent UIHoverExit;
        public static event ManipulateUIEvent UIManipulateStart;
        public static event ManipulateUIEvent UIManipulateEnd;

        [SerializeField] private GameObject titleBar;
        private XRUIBillboard billboard;

        [Header("Hover Events")]
        public UIHoverEvent HoverEnter;
        public UIHoverEvent HoverExit;

        [Header("Select Events")]
        public ManipulateEvent ManipulateStart;
        public ManipulateEvent ManipulateEnd;

        private void Start()
        {
            billboard = GetComponent<XRUIBillboard>();

            UIHoverEnter += HoverEnterEvent;
            UIHoverExit += HoverExitEvent;
            UIManipulateStart += ManipulateStartEvent;
            UIManipulateEnd += ManipulateEndEvent;
        }

        #region Invoker

        public static void InvokeUIHoverEnterEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIHoverEnter != null)
            {
                UIHoverEnter(_object, _interactor);
            }
        }

        public static void InvokeUIHoverExitEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIHoverExit != null)
            {
                UIHoverExit(_object, _interactor);
            }
        }

        public static void InvokeUIManipulateStartEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIManipulateStart != null)
            {
                UIManipulateStart(_object, _interactor);
            }
        }

        public static void InvokeUIManipulateEndEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIManipulateEnd != null)
            {
                UIManipulateEnd(_object, _interactor);
            }
        }

        #endregion

        #region events

        private void HoverEnterEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (_object == titleBar)
            {
                this.HoverEnter.Invoke();
            }
        }

        private void HoverExitEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (_object == titleBar)
            {
                this.HoverExit.Invoke();
            }
        }

        private void ManipulateStartEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (_object == titleBar)
            {
                this.ManipulateStart.Invoke();
                billboard.Interactor = _interactor;
                billboard.IsInteract = true;
            }
        }

        private void ManipulateEndEvent(GameObject _object, XRBaseInteractor _interactor)
        {
            if (_object == titleBar)
            {
                this.ManipulateEnd.Invoke();
                billboard.Interactor = null;
                billboard.IsInteract = false;
            }
        }

        #endregion
    }
}