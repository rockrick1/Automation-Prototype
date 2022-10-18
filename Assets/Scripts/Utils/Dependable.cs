using UnityEngine;

namespace Utils
{
    public abstract class Dependable : MonoBehaviour
    {
        void Start()
        {
            DependencyResolver.Instance.Register(this);
        }

        void OnDestroy()
        {
            DependencyResolver.Instance.Unregister(this);
        }
    }
}