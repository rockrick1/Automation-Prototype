using Assets.Scripts.Player;
using Items;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Assets.Scripts.Construction
{
    public class ItemPeviewController : MonoBehaviour
    {
        [SerializeField] Image _itemImage;
        [SerializeField] float _opacity = .5f;

        public void Show(ItemData itemData)
        {
            _itemImage.rectTransform.sizeDelta = itemData.Size;
            _itemImage.sprite = itemData.Sprite;
            _itemImage.color = new Color(1, 1, 1, _opacity);
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void MoveTo(Vector2 pos)
        {
            transform.position = pos;
        }

        void Update()
        {
        }
    }
}