using Assets.Scripts.Construction;
using Items;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Hud
{
    public class ToolbarController : MonoBehaviour
    {
        [SerializeField] List<ToolbarSlotController> _slots;

        //TODO this should be handled separately later
        [SerializeField] List<ItemData> _toolbarItems;

        ConstructionController _constructionController;

        void Start()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (i >= _toolbarItems.Count)
                {
                    _slots[i].Init(null, null);
                    continue;
                }

                ItemData itemData = _toolbarItems[i];
                _slots[i].Init(() => 
                {
                    OnSlotClicked(itemData);
                }, itemData.Sprite);
            }
        }

        void OnSlotClicked(ItemData itemData)
        {
            _constructionController = DependencyResolver.Instance.Resolve<ConstructionController>();
            _constructionController.EnterConstructionMode(itemData);
        }
    }
}