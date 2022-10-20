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

        void Start()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (i >= _toolbarItems.Count || _toolbarItems[i] == null)
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
            DependencyResolver.Instance.Resolve<ConstructionController>().EnterConstructionMode(itemData);
        }
    }
}