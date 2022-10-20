using UnityEngine;

namespace Utils
{
    public abstract class Dependable : MonoBehaviour
    {
        protected virtual void Start()
        {
            DependencyResolver.Instance.Register(this);
        }

        protected virtual void OnDestroy()
        {
            DependencyResolver.Instance.Unregister(this);
        }
    }
}