using Items;
using System.Collections;
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
        Coroutine _transportRoutine;

        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);
            _gridController = DependencyResolver.Instance.Resolve<FactoryController>();
            _animSync = _gridController.AnimSync;

            if (!TryGetItemAtOrientation(out PlaceableItemController other)) return;
            _target = other;
        }

        void Update()
        {
            _spriteAnimator.Play(0, -1, _animSync.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

        public void ReserveAndRegisterEvent(ItemInTransportController item)
        {
            _heldItem = item;
            TryRegisterEvent();
        }

        public void ReserveReception(ItemInTransportController item)
        {
            _heldItem = item;
        }

        public void ReceiveItem(ItemInTransportController item)
        {
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

        public override void OnItemRemovedAtOrientation(PlaceableItemController other)
        {
            DependencyResolver.Instance.Resolve<FactoryController>().RemoveEventsBySource(this);
        }

        public override void OnDestroyed()
        {
            DependencyResolver.Instance.Resolve<FactoryController>().RemoveEventsByTarget(this);

            if (_heldItem == null) return;

            if (_transportRoutine!= null) StopCoroutine(_transportRoutine);

            Destroy(_heldItem.gameObject);
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
            if (_transportRoutine != null) return;
            _transportRoutine = StartCoroutine(TransportRoutine());
        }

        IEnumerator TransportRoutine()
        {
            if (_target == null || _target is not IItemReceiver receiver)
            {
                _transportRoutine = null;
                yield break;
            }

            var item = _heldItem;
            float timeElapsed = 0;
            float duration = .1f;

            receiver.ReserveReception(_heldItem);
            _heldItem = null;

            while (timeElapsed < duration)
            {
                item.transform.position = Vector3.Lerp(transform.position, _target.transform.position, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            item.transform.position = _target.transform.position;
            receiver.ReceiveItem(item);

            _transportRoutine = null;
        }

        public bool IsFree()
        {
            return _heldItem == null;
        }
    }
}