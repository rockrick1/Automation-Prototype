using Assets.Scripts.Construction;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Items
{
    public enum ItemOrientation
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3
    }

    public class BasePlaceableItemController : MonoBehaviour
    {
        [SerializeField] protected ItemData _itemData;
        [SerializeField] protected SpriteRenderer _spriteRenderer;

        public ItemData ItemData => _itemData;

        public ItemOrientation Orientation = ItemOrientation.Right;

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
                    _spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.forward * 0);
                    break;
                case ItemOrientation.Down:
                    _spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.forward * 270);
                    break;
                case ItemOrientation.Left:
                    _spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                    break;
                case ItemOrientation.Up:
                    _spriteRenderer.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
                    break;
            }
        }

        public List<BasePlaceableItemController> GetSurroundingItems()
        {
            var ret = new List<BasePlaceableItemController>();
            return ret;
        }

        public BasePlaceableItemController GetItemAtOrientation()
        {
            return null;
            //_worldGridController.
        }
    }
}