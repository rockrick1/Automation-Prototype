using Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    public class ResourceNodeController : MonoBehaviour
    {

        [SerializeField] ItemData _yieldingItem;

        public ItemData YieldingItem => _yieldingItem;
    }
}