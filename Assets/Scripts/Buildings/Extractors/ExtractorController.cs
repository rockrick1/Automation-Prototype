using Assets.Scripts.Construction;
using Assets.Scripts.Resources;
using Items;
using System.Collections;
using UnityEngine;
using Utils;

namespace Buildings.Extractors
{
    public class ExtractorController : PlaceableItemController
    {
        [SerializeField] float _extractionInterval = 1f;

        ResourceNodeController _resourceNode;
        IItemTransporter _target;
        bool _hasResourceNode;

        public override void Init(ItemOrientation oritentation)
        {
            base.Init(oritentation);

            _hasResourceNode = DependencyResolver.Instance.Resolve<WorldGridController>().
                TryGetNodeOn(transform.position, out _resourceNode);
            if (!_hasResourceNode) return;

            TryGetItemAtOrientation(out PlaceableItemController output);
            _target = output is IItemTransporter receiver ? receiver : null;

            StartCoroutine(ItemGenerationRoutine());
        }

        public override void OnItemPlacedAtOrientation(PlaceableItemController other)
        {
            if (other is not IItemTransporter receiver) return;
            switch(other.ItemData.Type)
            {
                case ItemType.Assembler:
                case ItemType.Belt:
                    _target = receiver;
                    break;
            }
        }

        public override void OnItemRemovedAtOrientation(PlaceableItemController other)
        {
            _target = null;
        }

        IEnumerator ItemGenerationRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_extractionInterval);

                while (_target == null || !_target.IsFree(null)) yield return null;

                DependencyResolver.Instance.Resolve<FactoryController>().
                    FeedItemToReceiver(_target, _resourceNode.YieldingItem);
            }
        }
    }
}