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
            belt.ReserveReception(itemInTransport);
            belt.ReceiveItem(itemInTransport);
        }

        public void FeedItemToAssembler(AssemblerController assembler, ItemData item)
        {
            throw new NotImplementedException();
        }

        void Update()
        {
            foreach(BeltTransportEvent e in _beltTransportEvents.ToList())
            {
                _beltsWithEvents.Add(e.Source);
            }
            foreach(BeltTransportEvent e in _beltTransportEvents.ToList())
            {
                if (_beltsWithEvents.Contains(e.Target) || !e.Target.IsFree()) continue;
                
                e.Source.ExecuteTransport();
                _beltTransportEvents.Remove(e);
                _beltsWithEvents.Remove(e.Source);
            }
        }

        public void RegisterEvent(BeltTransportEvent beltTransportEvent)
        {
            _beltTransportEvents.Add(beltTransportEvent);
        }
    }
}