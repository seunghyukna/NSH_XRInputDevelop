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
    public class HoverEvent : UnityEvent { }

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
        public HoverEvent HoverEnter;
        public HoverEvent HoverExit;

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


        public static void InvokeUIHoverEnter(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIHoverEnter != null)
            {
                UIHoverEnter(_object, _interactor);
            }
        }

        public static void InvokeUIHoverExit(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIHoverExit != null)
            {
                UIHoverExit(_object, _interactor);
            }
        }

        public static void InvokeUIManipulateStart(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIManipulateStart != null)
            {
                UIManipulateStart(_object, _interactor);
            }
        }

        public static void InvokeUIManipulateEnd(GameObject _object, XRBaseInteractor _interactor)
        {
            if (UIManipulateEnd != null)
            {
                UIManipulateEnd(_object, _interactor);
            }
        }

        #region events

        private void HoverEnterEvent(GameObject _obj, XRBaseInteractor _interactor)
        {
            if (_obj == titleBar)
            {
                this.HoverEnter.Invoke();
            }
        }

        private void HoverExitEvent(GameObject _obj, XRBaseInteractor _interactor)
        {
            if (_obj == titleBar)
            {
                this.HoverExit.Invoke();
            }
        }

        private void ManipulateStartEvent(GameObject _obj, XRBaseInteractor _interactor)
        {
            if (_obj == titleBar)
            {
                this.ManipulateStart.Invoke();
                billboard.Interactor = _interactor;
                billboard.IsInteract = true;
            }
        }

        private void ManipulateEndEvent(GameObject _obj, XRBaseInteractor _interactor)
        {
            if (_obj == titleBar)
            {
                this.ManipulateEnd.Invoke();
                billboard.Interactor = null;
                billboard.IsInteract = false;
            }
        }

        #endregion
    }
}