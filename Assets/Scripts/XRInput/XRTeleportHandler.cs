using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Crengine.XRInput.Core;


/// <summary>
/// ********************************************************
///         **Layer Setting**
///         ----------
///         Floor : 'TeleportGround' || somethig else
///                 Floor.Teleportation Area.Interation Layer Mask -> Deselect all layer, add 'TeleportGround' || somethig else
///         
///         Interactor : Interaction Layer Mask, Raycase Mask ->
///                      Deselect all layer, add 'TeleportGround' || somethig else
///         ----------
/// ********************************************************
/// </summary>
namespace Crengine.XRInput
{
    public class XRTeleportHandler : XRInteractorSubSystem
    {
        [SerializeField] Transform player;
        private XRInteractorLineVisual teleportLineVisual;
        private LineRenderer lineRenderer;

        private bool isTeleportReady;

        private void Start()
        {
            baseInteractor = GetComponent<XRBaseInteractor>();
            teleportLineVisual = GetComponent<XRInteractorLineVisual>();
            lineRenderer = GetComponent<LineRenderer>();

            teleportLineVisual.enabled = false;
            baseInteractor.enabled = false;
        }

        private void Update()
        {
            // Get my XRNode
            if (!device.isValid)
                GetDevice();

            // Catch current XRNode button use
            DetectControllerUse();
            CatchEnterSecondary2DAxisInput();
        }

        private void CatchEnterSecondary2DAxisInput()
        {
            if (secondaryVector.y > 0.2f)
            {
                baseInteractor.enabled = true;
                teleportLineVisual.enabled = true;

                isTeleportReady = true;

            }
            else
            {
                ExecuteTeleport();

                teleportLineVisual.enabled = false;
                baseInteractor.enabled = false;

                isTeleportReady = false;
            }
        }

        private void ExecuteTeleport()
        {
            if (isTeleportReady)
            {
                player.position = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            }
        }
    }
}