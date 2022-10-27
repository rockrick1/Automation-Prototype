using Buildings.Belts;
using Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Buildings
{
    public class FactoryController : Dependable
    {
        [SerializeField] Tilemap _tilemap;
        [SerializeField] Animator _beltSpritesAnimationSynchronizer;
        [SerializeField] ItemInTransportController _itemInTransportPrefab;

        public Animator AnimSync => _beltSpritesAnimationSynchronizer;

        HashSet<ItemTransportEvent> _beltTransportEvents;
        HashSet<IItemTransporter> _itemsWithEvents;

        protected override void Start()
        {
            base.Start();
            _beltTransportEvents = new HashSet<ItemTransportEvent>();
            _itemsWithEvents = new HashSet<IItemTransporter>();
        }

        public void FeedItemToReceiver(IItemTransporter receiver, ItemData item)
        {
            if (receiver is not PlaceableItemController placeable) return;
            if (receiver is BeltController)
            {
                var itemInTransport = Instantiate(_itemInTransportPrefab);
                itemInTransport.Init(item);
                itemInTransport.transform.position = placeable.transform.position;
                receiver.ReserveAndInstantReceive(item, itemInTransport);
            }
            else
            {
                receiver.ReserveAndInstantReceive(item);
            }
        }

        public void RegisterEvent(ItemTransportEvent beltTransportEvent)
        {
            _beltTransportEvents.Add(beltTransportEvent);
            _itemsWithEvents.Add(beltTransportEvent.Source);
        }

        public void RemoveEventsBySource(IItemTransporter receiver)
        {
            _beltTransportEvents.RemoveWhere(e => e.Source == receiver);
            if (receiver is BeltController belt)
            {
                _itemsWithEvents.Remove(belt);
            }
        }

        public void RemoveEventsByTarget(IItemTransporter receiver)
        {
            _beltTransportEvents.RemoveWhere(e => e.Target == receiver);
            if (receiver is BeltController belt)
            {
                _itemsWithEvents.Remove(belt);
            }
        }

        void Update()
        {
            foreach (ItemTransportEvent e in _beltTransportEvents.ToList())
            {
                if (e.Source == null || !(e.Target as Object))
                {
                    _beltTransportEvents.Remove(e);
                    continue;
                }

                if ((e.Target is IItemTransporter target && _itemsWithEvents.Contains(target)) ||
                    !e.Target.IsFree(e.ItemData))
                {
                    continue;
                }

                e.Source.ExecuteTransport();
                _beltTransportEvents.Remove(e);
                _itemsWithEvents.Remove(e.Source);
            }
        }

        void OnDrawGizmos()
        {
            if (_beltTransportEvents == null) return;

            foreach (var e in _beltTransportEvents)
            {
                if (!(e.Target as Object) ||
                    !(e.Source as Object) ||
                    (e.Target is not PlaceableItemController target) ||
                    (e.Source is not PlaceableItemController source))
                {
                    continue;
                }

                DrawArrow(source.transform.position, target.transform.position - source.transform.position, .5f);
            }
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 45f)
        {
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(10, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(-10, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}