using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crengine.Core
{
    public abstract class EventCore
    {
        public T GetEventOfType<T>() where T : EventCore
        {
            return (T)this;
        }
    }
}
