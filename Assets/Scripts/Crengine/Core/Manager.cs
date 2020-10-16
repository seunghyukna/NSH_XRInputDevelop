using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Crengine.Core
{
    public abstract class Manager : MonoBehaviour
    {
        private void Awake()
        {
            SingletonRegistry.instance.AddRegistry(this);
        }
        Dictionary<Type, Action<EventCore>> eventGroup = new Dictionary<Type, Action<EventCore>>();

        public void RaiseEvent<T>(EventCore _data) where T : EventCore
        {
            if(!eventGroup.ContainsKey(typeof(T)))
            {
                Debug.Log(_data.GetType() + "is not Settings");
                return;
            }
            eventGroup[typeof(T)].Invoke(_data); 
        }
        public void AddListener<T>(Action<EventCore> _event) where T : EventCore
        {
            if(!eventGroup.ContainsKey(typeof(T)))
            {
                eventGroup.Add(typeof(T),new Action<EventCore>((EventCore _data)=> { }));
            }
            eventGroup[typeof(T)] += _event;
        }
        public void RemoveListener<T>(Action<EventCore> _event) where T : EventCore
        {
            if (!eventGroup.ContainsKey(typeof(T)))
            {
                Debug.Log("NotFoundKey");
                return;
            }
            eventGroup[typeof(T)] -= _event;
        }
    }
}
