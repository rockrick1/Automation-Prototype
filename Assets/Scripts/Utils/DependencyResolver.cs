using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class DependencyResolver : MonoBehaviour
    {
        public static DependencyResolver Instance;

        List<object> _dependables = new List<object>();

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public T TryResolve<T>() where T : Dependable
        {
            T output = (T) _dependables.Find(dependable => dependable is T);

            if (output == null)
            {
                Debug.LogError($"[{nameof(DependencyResolver)}] - Could not resolve dependency of type {nameof(T)}");
                return null;
            }

            return output;
        }


        public void Register<T>(T dependable) where T : Dependable
        {
            _dependables.Add(dependable);
        }

        internal void Unregister(Dependable dependable)
        {
            _dependables.Remove(dependable);
        }
    }
}