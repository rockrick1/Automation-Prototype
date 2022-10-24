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
        [SerializeField] float _extractionInterval = 1f;

        ResourceNodeController _resourceNode;
        PlaceableItemController _target;
        bool _hasResourceNode;

        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);

            _hasResourceNode = DependencyResolver.Instance.Resolve<WorldGridController>().
                TryGetNodeOn(transform.position, out _resourceNode);
            if (!_hasResourceNode) return;

            var grid = DependencyResolver.Instance.Resolve<FactoryController>();
            TryGetItemAtOrientation(out _target);

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

                yield return new WaitForSeconds(_extractionInterval);
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