using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XRInput.Core.Singleton;

namespace XRInput.Core
{
    public class XRRayHitPosition : XRInputSingleton<XRRayHitPosition>
    {
        public Vector3 HitPosition;
    }
}