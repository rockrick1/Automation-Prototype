using System.Collections;
using UnityEngine;
using Utils;

namespace Items
{
    public class BeltController : BasePlaceableItemController
    {
        [SerializeField] Animator _spriteAnimator;

        // the object to which the belt will try to move its item
        BasePlaceableItemController _target;
        Animator _animSync;
        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);
            _animSync = DependencyResolver.Instance.Resolve<BeltGridController>().AnimSync;
        }

        void Update()
        {
            _spriteAnimator.Play(0, -1, _animSync.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }
}