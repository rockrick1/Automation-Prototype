using UnityEngine;

namespace Utils
{
    public abstract class Dependable : MonoBehaviour
    {
        protected virtual void Start()
        {
            Debug.Log($"Dependable registration: {ToString()}");
            DependencyResolver.Instance.Register(this);
        }

        protected virtual void OnDestroy()
        {
            Debug.Log($"Dependable destruction: {ToString()}");
            DependencyResolver.Instance.Unregister(this);
        }
    }
}