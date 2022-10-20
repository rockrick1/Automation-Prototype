using Items;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Construction
{
    public class PlaceableItemsProvider
    {
        const string ITEM_PREFABS_PATH = "Assets/Prefabs/Items/PlaceableItems";

        Dictionary<string, BasePlaceableItemController> _items;

        public PlaceableItemsProvider()
        {
            _items = new Dictionary<string, BasePlaceableItemController>();
            PopulateItemsDict();
        }

        void PopulateItemsDict()
        {
            string[] assetsPaths = AssetDatabase.GetAllAssetPaths();
            List<string> prefabsPaths = new List<string>();

            foreach (string assetPath in assetsPaths)
            {
                if (assetPath.Contains(ITEM_PREFABS_PATH) && assetPath.Contains(".prefab"))
                {
                    prefabsPaths.Add(assetPath);
                    BasePlaceableItemController asset = AssetDatabase.LoadAssetAtPath(assetPath, 
                        typeof(BasePlaceableItemController)) as BasePlaceableItemController;
                    if (string.IsNullOrEmpty(asset.ItemData.Name))
                    {
                        Debug.LogError($"Trying to register ItemData with empty name! {asset.ItemData.ToString()}");
                    }
                    _items.Add(asset.ItemData.Name, asset);
                }
            }
        }

        public BasePlaceableItemController GetPlaceableItem(ItemData itemData)
        {
            if (_items.TryGetValue(itemData.Name, out BasePlaceableItemController item))
            {
                return item;
            }

            Debug.LogError($"[{nameof(PlaceableItemsProvider)}] - Could not get item of name {itemData.Name}");
            return null;
        }
    }
}