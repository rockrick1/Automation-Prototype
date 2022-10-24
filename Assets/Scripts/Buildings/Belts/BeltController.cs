using Items;
using UnityEngine;
using Utils;

namespace Buildings.Belts
{
    public class BeltController : PlaceableItemController, IItemReceiver
    {
        [SerializeField] Animator _spriteAnimator;

        FactoryController _gridController;
        ItemInTransportController _heldItem;
        // the object to which the belt will try to move its item
        PlaceableItemController _target;
        Animator _animSync;

        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);
            _gridController = DependencyResolver.Instance.Resolve<FactoryController>();
            _animSync = _gridController.AnimSync;

            if (!TryGetItemAtOrientation(out PlaceableItemController other)) return;
            _target = other;
        }

        public void ReceiveItem(ItemInTransportController item)
        {
            _heldItem = item;
            //move item
            item.transform.position = transform.position;

            TryRegisterEvent();
        }

        public override void OnItemPlacedAtOrientation(PlaceableItemController other)
        {
            switch (other.ItemData.Type)
            {
                case ItemType.Inserter:
                case ItemType.Assembler:
                case ItemType.Belt:
                    _target = other;
                    TryRegisterEvent();
                    break;
            }
        }

        void Update()
        {
            _spriteAnimator.Play(0, -1, _animSync.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

        void TryRegisterEvent()
        {
            if (_heldItem == null || _target == null) return;

            var beltTransportEvent = new BeltTransportEvent
            {
                Source = this,
                Target = _target as IItemReceiver
            };
            _gridController.RegisterEvent(beltTransportEvent);
        }

        public void ExecuteTransport()
        {
            if (_target != null && _target is IItemReceiver receiver)
            {
                receiver.ReceiveItem(_heldItem);
                _heldItem = null;
            }
        }

        public bool IsFree()
        {
            return _heldItem == null;
        }
    }
}