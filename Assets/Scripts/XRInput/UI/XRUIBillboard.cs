using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Crengine.XRInput.UI
{
    /// <summary>
    /// 
    /// UICanvas must face to user camera
    /// 
    /// </summary>
    /// 
    public class XRUIBillboard : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform titleBar;

        private bool isInteract;
        public bool IsInteract { get { return isInteract; } set { isInteract = value; } }

        private XRBaseInteractor interactor;
        public XRBaseInteractor Interactor { get { return interactor; } set { interactor = value; } }

        private void Update()
        {
            if (isInteract)
            {
                BillboardInteractor();
            }
        }

        Quaternion originalQuaternion;
        private void Start()
        {
            originalQuaternion = titleBar.rotation;
        }

        private void BillboardInteractor()
        {
            titleBar.rotation = Quaternion.LookRotation(titleBar.position - player.transform.position) * originalQuaternion;
            Vector3 angle = titleBar.eulerAngles;
            angle.x = 0;
            titleBar.eulerAngles = angle;
        }
    }
}