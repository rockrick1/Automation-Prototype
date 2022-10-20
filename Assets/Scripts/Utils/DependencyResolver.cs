using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class DependencyResolver : MonoBehaviour
    {
        public static DependencyResolver Instance;

        Dictionary<Type, object> _dependables = new Dictionary<Type, object>();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public T Resolve<T>() where T : Dependable
        {
            return (T) _dependables[typeof(T)];
        }

        public bool TryResolve<T>(out T output) where T : Dependable
        {
            if (_dependables.TryGetValue(typeof(T), out object dependable))
            {
                output = dependable as T;
                return true;
            }

            output = null;
            return false;
        }

        public void Register<T>(T dependable) where T : Dependable
        {
            Debug.Log($"Dependency resolver registration: {dependable.GetType()}");
            _dependables[dependable.GetType()] = dependable as T;
        }

        internal void Unregister<T>(T dependable) where T : Dependable
        {
            Debug.Log($"Dependency resolver removal: {dependable.GetType()}");
            _dependables.Remove(dependable.GetType());
        }
    }
}