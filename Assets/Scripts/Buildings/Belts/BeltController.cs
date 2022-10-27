using Items;
using System.Collections;
using UnityEngine;
using Utils;

namespace Buildings.Belts
{
    public class BeltController : PlaceableItemController, IItemTransporter
    {
        [SerializeField] Animator _spriteAnimator;
        [SerializeField] float _transportSpeed = .5f;

        FactoryController _factoryController;
        Animator _animSync;

        IItemTransporter _target;
        ItemInTransportController _heldItem;
        Coroutine _transportRoutine;

        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);
            _factoryController = DependencyResolver.Instance.Resolve<FactoryController>();
            _animSync = _factoryController.AnimSync;

            if (!TryGetItemAtOrientation(out PlaceableItemController other) ||
                other is not IItemTransporter receiver ||
                other is not BeltController) return;
            _target = receiver;
        }

        void Update()
        {
            _spriteAnimator.Play(0, -1, _animSync.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

        public void ReserveAndExecuteReception(ItemInTransportController item)
        {
            _heldItem = item;
            _transportRoutine = StartCoroutine(TransportRoutine());
        }

        public void ReserveAndInstantReceive(ItemInTransportController item)
        {
            _heldItem = item;
            TryRegisterEvent();
        }

        public void OnReceptionFinished(ItemInTransportController _)
        {
            TryRegisterEvent();
        }

        public override void OnItemPlacedAtOrientation(PlaceableItemController other)
        {
            if (other is not IItemTransporter receiver) return;

            switch (other.ItemData.Type)
            {
                case ItemType.Inserter:
                case ItemType.Assembler:
                case ItemType.Belt:
                    _target = receiver;
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

            if (_transportRoutine != null) StopCoroutine(_transportRoutine);

            Destroy(_heldItem.gameObject);
        }

        void TryRegisterEvent()
        {
            if (_heldItem == null) return;

            var beltTransportEvent = new ItemTransportEvent
            {
                Source = this,
                Target = _target
            };
            _factoryController.RegisterEvent(beltTransportEvent);
        }

        public void ExecuteTransport()
        {
            if (!(_target as Object)) return;

            _target.ReserveAndExecuteReception(_heldItem);
            _heldItem = null;
        }

        IEnumerator TransportRoutine()
        {
            if (_heldItem == null) yield break;

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
            OnReceptionFinished(_heldItem);
            _transportRoutine = null;
        }

        public bool IsFree()
        {
            return _heldItem == null;
        }

        public bool TryForceTakeItem(out ItemData output)
        {
            output = null;
            if (_heldItem == null || _transportRoutine != null) return false;

            output = _heldItem.ItemData;

            Destroy(_heldItem.gameObject);
            _heldItem = null;

            _factoryController.RemoveEventsBySource(this);

            return true;
        }
    }
}