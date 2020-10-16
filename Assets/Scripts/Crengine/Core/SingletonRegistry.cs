using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Crengine.Core
{

    public class SingletonRegistry : MonoBehaviour
    {

        public static SingletonRegistry instance;
        public Dictionary<System.Type, Manager> managers = new Dictionary<System.Type, Manager>();
        public void Awake()
        {
            Debug.Log("SingletonSetup");
            Debug.Log(instance);
            if (instance != null)
            {
                Debug.Log(gameObject.name);
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void GetManagers()
        {
            Manager[] managers = GetComponents<Manager>();
            for (int i = 0; i < managers.Length; i++)
            {
                AddRegistry(managers[i]);
            }
        }
        public void AddRegistry(Manager _manager)
        {
            Debug.Log(_manager + "  is Setup");
            instance.managers.Add(_manager.GetType(), _manager);
        }
        public void RemoveRegistry(Manager _manager)
        {
            instance.managers.Remove(_manager.GetType());
        }
        private void OnDestroy()
        {
            if (instance == this)
                instance = null;

        }
        public T FindManagersOfType<T>() where T : Manager
        {

            return managers[typeof(T)] as T;
        }
    }
}
