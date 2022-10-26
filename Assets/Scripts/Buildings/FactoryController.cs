using Buildings.Assemblers;
using Buildings.Belts;
using Items;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

        List<BeltController> _belts;
        List<BeltPath> _beltPaths;

        protected override void Start()
        {
            base.Start();
            _belts = new List<BeltController>();
            _beltPaths = new List<BeltPath>();
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

        void Update()
        {
            foreach(var path in _beltPaths)
            {
                //path.ExecuteTransport();
            }
        }

        public void OnBeltCreated(BeltController belt)
        {
            _belts.Add(belt);
            RefreshPathsList(belt);
        }

        public void OnBeltDestroyed(BeltController belt)
        {
            _belts.Remove(belt);
            RefreshPathsList(belt);
        }

        void RefreshPathsList(BeltController placed)
        {
            _beltPaths.Clear();

            List<BeltController> belts = new List<BeltController>(_belts);

            while(belts.Count > 0)
            {
                BeltController belt = belts[0];
                belts.RemoveAt(0);

                bool foundPath = false;
                if (belt.TryGetItemAtOrientation(out PlaceableItemController next) && next is BeltController)
                {
                    bool hasPrevious = belt.TryGetItemAtOppositeOrientation(out PlaceableItemController previous) &&
                            previous is BeltController;

                    foreach(BeltPath path in _beltPaths)
                    {
                        if (!path.ContainsBelt(belt)) continue;
                        if (hasPrevious && path.ContainsBelt(previous as BeltController)) continue;

                        path.AddBelt(belt);
                        foundPath = true;
                        break;
                    }
                }

                if (!foundPath)
                {
                    BeltPath path = new BeltPath();
                    path.AddBelt(belt);
                    _beltPaths.Add(path);
                }
            }
            {
                int iterations = 0;
                while(TryMergeBeltPaths())
                {
                    if (++iterations > 1000) 
                    {
                        Debug.LogError("BREAK!!!!");
                        break;
                    }
                }
            }
        }

        bool TryMergeBeltPaths()
        {
            for(int i = 0; i < _beltPaths.Count; i++)
            {
                BeltPath pathA = _beltPaths[i];
                for(int j = 0; j < _beltPaths.Count; j++)
                {
                    if (i == j) continue;
                    BeltPath pathB = _beltPaths[j];

                    BeltController firstA = pathA.GetFristBelt();
                    BeltController lastA = pathA.GetLastBelt();
                    BeltController firstB = pathB.GetFristBelt();
                    BeltController lastB = pathB.GetLastBelt();

                    lastA.TryGetItemAtOrientation(out PlaceableItemController nextLastA);
                    lastB.TryGetItemAtOrientation(out PlaceableItemController nextLastB);

                    // A will connect with B
                    if (nextLastA == firstB && nextLastB != firstA)
                    {
                        pathA.MergePathWith(pathB);
                        _beltPaths.Remove(pathB);
                        return true;
                    }
                }
            }

            return false;
        }

        void OnDrawGizmos()
        {
            if (_beltPaths == null) return;

            foreach(var path in _beltPaths)
            {
                int i = 0;
                foreach(var belt in path.Belts)
                {
                    Handles.Label(belt.transform.position, $"{i++}");
                }
            }
        }
    }
}