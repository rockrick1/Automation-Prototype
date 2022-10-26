using Items;
using System.Collections;
using UnityEngine;
using Utils;

namespace Buildings.Belts
{
    public class BeltController : PlaceableItemController, IItemReceiver
    {
        [SerializeField] Animator _spriteAnimator;
        [SerializeField] float _transportSpeed = .5f;

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

        public void ReserveAndExecuteReception(ItemInTransportController item)
        {
            _heldItem = item;
            _transportRoutine = StartCoroutine(TransportRoutine());
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
                    if (_transportRoutine == null) TryRegisterEvent();
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
            if (_target == null || _target is not IItemReceiver receiver) return;

            receiver.ReserveAndExecuteReception(_heldItem);
            _heldItem = null;
        }

        IEnumerator TransportRoutine()
        {
            Vector3 origin = _heldItem.transform.position;
            Vector3 destination = transform.position;
            float timeElapsed = 0;

            while (timeElapsed < _transportSpeed)
            {
                if (_heldItem == null) yield break;
                _heldItem.transform.position = Vector3.Lerp(origin, destination, timeElapsed / _transportSpeed);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _heldItem.transform.position = destination;
            TryRegisterEvent();
            _transportRoutine = null;
        }

        public bool IsFree()
        {
            return _heldItem == null;
        }
    }
}