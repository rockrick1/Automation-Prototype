using Buildings.Belts;
using Items;
using System.Collections;
using UnityEngine;
using Utils;

namespace Buildings.Inserters
{
    public class InserterController : PlaceableItemController, IItemTransporter
    {
        [SerializeField] float _insertionRate = 1f;

        FactoryController _factoryController;

        PlaceableItemController _feeder;
        IItemTransporter _target;
        ItemData _heldItem;

        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);

            _factoryController = DependencyResolver.Instance.Resolve<FactoryController>();

            TryGetItemAtOrientation(out PlaceableItemController atOrientation);
            TryGetItemAtOppositeOrientation(out _feeder);

            if (atOrientation != null && atOrientation is IItemTransporter receiver && receiver is not InserterController)
            {
                _target = receiver;
            }

            StartCoroutine(GetItemFromFeederRoutine());
        }

        public override void OnItemPlacedAtOrientation(PlaceableItemController other)
        {
            if (other is not IItemTransporter transporter) return;
            _target = transporter;
        }

        IEnumerator GetItemFromFeederRoutine()
        {
            while (true)
            {
                if (_heldItem != null || _feeder == null || _feeder is not IItemTransporter feeder ||
                    !(_target as UnityEngine.Object))
                {
                    yield return null;
                    continue;
                }

                if (feeder.TryForceTakeItem(out _heldItem))
                {
                    yield return new WaitForSeconds(_insertionRate);
                    OnReceptionFinished(null);
                }

                yield return null;
            }
        }

        public bool IsFree()
        {
            return true;
        }

        public void ReserveAndInstantReceive(ItemInTransportController item)
        {
            throw new System.NotImplementedException();
        }

        public void ReserveAndExecuteReception(ItemInTransportController item)
        {
            throw new System.NotImplementedException();
            //_heldItem = item.ItemData;
            //Destroy(item);
            //OnReceptionFinished(null);
        }

        public void OnReceptionFinished(ItemInTransportController item)
        {
            TryRegisterEvent();
        }

        void TryRegisterEvent()
        {
            if (_heldItem == null || _target == null) return;

            var beltTransportEvent = new ItemTransportEvent
            {
                Source = this,
                Target = _target
            };
            _factoryController.RegisterEvent(beltTransportEvent);
        }

        public void ExecuteTransport()
        {
            if (!(_target as UnityEngine.Object)) return;

            DependencyResolver.Instance.Resolve<FactoryController>().
                FeedItemToReceiver(_target, _heldItem);

            _heldItem = null;
        }

        public bool TryForceTakeItem(out ItemData output)
        {
            output = null;
            return false;
        }
    }
}