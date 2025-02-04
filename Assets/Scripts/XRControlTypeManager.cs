﻿using UnityEngine;

namespace Crengine.XRInput.Core
{
    public enum InteractorType
    {
        Direct,
        Ray
    }

    public enum LocomotionType
    {
        Teleport,
        Movement
    }

    public class XRControlTypeManager : MonoBehaviour
    {
        [SerializeField] private InteractorType interactor;
        public InteractorType Interactor { get { return interactor; } set { interactor = value; } }

        [SerializeField] private LocomotionType locomotion;
        public LocomotionType Locomotion { get { return locomotion; } set { locomotion = value; } }

        private void Start()
        {
            SetInteractorType();
            SetLocomotionType();
        }

        public void SetInteractorType()
        {
            switch (interactor)
            {
                case InteractorType.Direct:

                    break;
                case InteractorType.Ray:

                    break;
                default:
                    break;
            }
        }

        public void SetLocomotionType()
        {
            switch (locomotion)
            {
                case LocomotionType.Teleport:

                    break;
                case LocomotionType.Movement:

                    break;
                default:
                    break;
            }
        }
    }
}