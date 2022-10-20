using Items;
using System;
using System.Collections.Generic;
using Utils;

namespace Assets.Scripts.Player
{
    public class PlayerInventoryController
    {
        List<InventoryItem> _inventory;

        public PlayerInventoryController()
        {
            _inventory = new List<InventoryItem>();
            LoadListFromSave();
        }

        private void LoadListFromSave()
        {
            var itemService = DependencyResolver.Instance.Resolve<ItemService>();
            //TODO this should load a list of items from the player's save file
            _inventory.Add(new InventoryItem
            {
                Quantity = 1,
                ItemData = itemService.GetItemData("BeltMk1")
            });
            _inventory.Add(new InventoryItem
            {
                Quantity = 1,
                ItemData = itemService.GetItemData("AssemblerMk1")
            });
        }

        [Serializable]
        struct InventoryItem
        {
            public int Quantity;
            public ItemData ItemData;
        }
    }
}