using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hud
{
    public class ToolbarSlotController : MonoBehaviour
    {
        [SerializeField] Button _button;
        [SerializeField] Image _itemImage;

        public void Init(Action onClick, Sprite itemSprite)
        {
            if (onClick == null)
            {
                _itemImage.gameObject.SetActive(false);
                return;
            }

            _button.onClick.AddListener(onClick.Invoke);
            _itemImage.sprite = itemSprite;
        }
    }
}