using Assets.Scripts.Construction;
using Assets.Scripts.Resources;
using Buildings.Assemblers;
using Buildings.Belts;
using Items;
using System.Collections;
using UnityEngine;
using Utils;

namespace Buildings.Extractors
{
    public class Extractor : PlaceableItemController
    {
        ResourceNodeController _resourceNode;
        PlaceableItemController _target;
        bool _hasResourceNode;

        public override void Init(ItemOrientation oritentation)
        {
            _hasResourceNode = DependencyResolver.Instance.Resolve<WorldGridController>().
                TryGetNodeOn(transform.position, out _resourceNode);
            base.Init(oritentation);
            StartCoroutine(ItemGenerationRoutine());
        }

        public override void OnItemPlacedAtOrientation(PlaceableItemController other)
        {
            switch(other.ItemData.Type)
            {
                case ItemType.Assembler:
                case ItemType.Belt:
                    _target = other;
                    break;
            }
        }

        IEnumerator ItemGenerationRoutine()
        {
            while (true)
            {
                if (_hasResourceNode && _target != null && _target is IItemReceiver receiver && receiver.IsFree())
                {
                    switch (_target.ItemData.Type)
                    {
                        case ItemType.Assembler:
                            FeedResourceToAssembler(_target as AssemblerController, _resourceNode.YieldingItem);
                            break;
                        case ItemType.Belt:
                            FeedResourceToBelt(_target as BeltController, _resourceNode.YieldingItem);
                            break;
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }

        void FeedResourceToBelt(BeltController belt, ItemData resource)
        {
            DependencyResolver.Instance.Resolve<FactoryController>().FeedItemToBelt(belt, resource);
        }

        void FeedResourceToAssembler(AssemblerController assembler, ItemData resource)
        {
            DependencyResolver.Instance.Resolve<FactoryController>().FeedItemToAssembler(assembler, resource);
        }
    }
}