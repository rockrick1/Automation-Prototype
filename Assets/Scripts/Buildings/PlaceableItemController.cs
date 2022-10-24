using Assets.Scripts.Construction;
using Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Buildings
{
    public enum ItemOrientation
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3
    }

    [DisallowMultipleComponent]
    public class PlaceableItemController : MonoBehaviour
    {
        [SerializeField] protected ItemData _itemData;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        public ItemData ItemData => _itemData;

        [HideInInspector]
        public ItemOrientation Orientation;

        WorldGridController _worldGridController;

        public virtual void Init(ItemOrientation oritentation)
        {
            _worldGridController = DependencyResolver.Instance.Resolve<WorldGridController>();
            _spriteRenderer.sprite = ItemData.Sprite;
            Orientation = oritentation;
            OnRotated();
        }

        public ItemOrientation RotateCw()
        {
            Orientation++;
            if ((int) Orientation == 4) Orientation = ItemOrientation.Right;
            OnRotated();
            return Orientation;
        }

        public ItemOrientation RotateCcw()
        {
            Orientation--;
            if ((int) Orientation == -1) Orientation = ItemOrientation.Up;
            OnRotated();
            return Orientation;
        }

        void OnRotated()
        {
            switch (Orientation)
            {
                case ItemOrientation.Right:
                    transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                    break;
                case ItemOrientation.Down:
                    transform.rotation = Quaternion.Euler(Vector3.forward * 270);
                    break;
                case ItemOrientation.Left:
                    transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                    break;
                case ItemOrientation.Up:
                    transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                    break;
            }
        }

        public Dictionary<Vector3, PlaceableItemController> GetSurroundingItems()
        {
            if (_worldGridController == null)
            {
                _worldGridController = DependencyResolver.Instance.Resolve<WorldGridController>();
            }

            var ret = new Dictionary<Vector3, PlaceableItemController>();
            PlaceableItemController toInsert;
            Vector3 pos = transform.position;
            Vector3 other;
            int outerOffset = (_itemData.Size.x / 2) + 1;

            for (int i = -(_itemData.Size.x / 2); i <= _itemData.Size.x / 2; i++)
            {
                //size x
                other = pos + new Vector3(outerOffset, i);
                if (_worldGridController.TryGetItemOn(other, out toInsert))
                {
                    ret.Add(other, toInsert);
                }
                //size -x
                other = pos + new Vector3(-outerOffset, i);
                if (_worldGridController.TryGetItemOn(other, out toInsert))
                {
                    ret.Add(other, toInsert);
                }
                //size y
                other = pos + new Vector3(i, outerOffset);
                if (_worldGridController.TryGetItemOn(other, out toInsert))
                {
                    ret.Add(other, toInsert);
                }
                //size -y
                other = pos + new Vector3(i, -outerOffset);
                if (_worldGridController.TryGetItemOn(other, out toInsert))
                {
                    ret.Add(other, toInsert);
                }
            }

            return ret;
        }

        public bool TryGetItemAtOrientation(out PlaceableItemController output)
        {
            output = null;
            if (_itemData.Size.x != 1 || _itemData.Size.y != 1) return false;

            switch (Orientation)
            {
                case ItemOrientation.Right:
                    return _worldGridController.TryGetItemOn(transform.position + new Vector3(1, 0), out output);
                case ItemOrientation.Down:
                    return _worldGridController.TryGetItemOn(transform.position + new Vector3(0, -1), out output);
                case ItemOrientation.Left:
                    return _worldGridController.TryGetItemOn(transform.position + new Vector3(-1, 0), out output);
                case ItemOrientation.Up:
                    return _worldGridController.TryGetItemOn(transform.position + new Vector3(0, 1), out output);
                default:
                    return false;
            }
        }

        public virtual void OnDestroyed() { }
        public virtual void OnItemPlacedAdjacent(PlaceableItemController other, Vector3 pos) { }
        public virtual void OnItemPlacedAtOrientation(PlaceableItemController other) { }
        public virtual void OnItemRemovedAdjacent(PlaceableItemController other, Vector3 pos) { }
        public virtual void OnItemRemovedAtOrientation(PlaceableItemController other) { }
    }
}