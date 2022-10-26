using Buildings.Assemblers;
using Buildings.Belts;
using Items;
using System;
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

        HashSet<BeltTransportEvent> _beltTransportEvents;
        HashSet<BeltController> _beltsWithEvents;

        protected override void Start()
        {
            base.Start();
            _beltTransportEvents = new HashSet<BeltTransportEvent>();
            _beltsWithEvents = new HashSet<BeltController>();
        }

        public void FeedItemToBelt(BeltController belt, ItemData item)
        {
            var itemInTransport = Instantiate(_itemInTransportPrefab);
            itemInTransport.Init(item);
            itemInTransport.transform.position = belt.transform.position;
            belt.ReserveAndRegisterEvent(itemInTransport);
        }

        public void FeedItemToAssembler(AssemblerController assembler, ItemData item)
        {
            throw new NotImplementedException();
        }

        public void RegisterEvent(BeltTransportEvent beltTransportEvent)
        {
            _beltTransportEvents.Add(beltTransportEvent);
            _beltsWithEvents.Add(beltTransportEvent.Source);
        }

        public void RemoveEventsBySource(IItemReceiver receiver)
        {
            _beltTransportEvents.RemoveWhere(e => e.Source == receiver as BeltController);
            if (receiver is BeltController belt)
            {
                _beltsWithEvents.Remove(belt);
            }
        }

        public void RemoveEventsByTarget(IItemReceiver receiver)
        {
            _beltTransportEvents.RemoveWhere(e => e.Target == receiver);
            if (receiver is BeltController belt)
            {
                _beltsWithEvents.Remove(belt);
            }
        }

        void OnDrawGizmos()
        {
            if (_beltTransportEvents == null) return;

            foreach(var e in _beltTransportEvents)
            {
                if (e.Target == null || e.Source == null || e.Target is not PlaceableItemController other) continue;
                DrawArrow(e.Source.transform.position, other.transform.position - e.Source.transform.position, .5f);
            }
        }

        void Update()
        {
            foreach (BeltTransportEvent e in _beltTransportEvents.ToList())
            {
                if (e.Source == null || e.Target == null)
                {
                    _beltTransportEvents.Remove(e);
                }

                if (_beltsWithEvents.Contains(e.Target) || !e.Target.IsFree()) continue;

                e.Source.ExecuteTransport();
                _beltTransportEvents.Remove(e);
                _beltsWithEvents.Remove(e.Source);
            }
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 45f)
        {
            Gizmos.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}