using System.Collections.Generic;
using UnityEditor;
using Utils;

namespace Items
{
    public class ItemService : Dependable
    {
        const string ITEM_DATAS_PATH = "Assets/ItemDatas";

        Dictionary<string, ItemData> _items;

        protected override void Start()
        {
            base.Start();
            PopulateItemsDict();
        }

        void PopulateItemsDict()
        {
            string[] assetsPaths = AssetDatabase.GetAllAssetPaths();
            List<string> dataPaths = new List<string>();

            foreach (string assetPath in assetsPaths)
            {
                if (assetPath.Contains(ITEM_DATAS_PATH) && assetPath.Contains(".prefab"))
                {
                    dataPaths.Add(assetPath);
                    ItemData asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ItemData)) as ItemData;
                    _items.Add(asset.Name, asset);
                }
            }
        }

        public ItemData GetItemData(string name)
        {
            return _items[name];
        }
    }
}