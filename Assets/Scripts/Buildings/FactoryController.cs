using Buildings.Assemblers;
using Buildings.Belts;
using Items;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
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
        Dictionary<BeltController, BeltPath> _beltsToPath;

        protected override void Start()
        {
            base.Start();
            _belts = new List<BeltController>();
            _beltPaths = new List<BeltPath>();
            _beltsToPath = new Dictionary<BeltController, BeltPath>();
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
            List<BeltController> belts = new List<BeltController>();
            belts.Add(placed);

            string debug = "b4";
            foreach (var path in _beltPaths)
            {
                debug += $"{path}\n";
            }
            Debug.Log(debug);

            foreach (var neighbour in placed.GetSurroundingItems().Values)
            {
                if (neighbour is not BeltController neighbourBelt) continue;

                belts.AddRange(_beltsToPath[neighbourBelt].Belts);
                _beltPaths.Remove(_beltsToPath[neighbourBelt]);
            }

            while (belts.Count > 0)
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
                        _beltsToPath[belt] = path;
                        foundPath = true;
                        break;
                    }
                }

                if (!foundPath)
                {
                    BeltPath path = new BeltPath();
                    path.AddBelt(belt);
                    _beltsToPath[belt] = path;
                    _beltPaths.Add(path);
                }

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


            debug = "after";
            foreach (var path in _beltPaths)
            {
                debug += $"{path}\n";
            }
            Debug.Log(debug);
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
                        foreach(var belt in pathB.Belts)
                        {
                            _beltsToPath[belt] = pathA;
                        }
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