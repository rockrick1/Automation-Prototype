using Buildings;
using NaughtyAttributes;
using UnityEngine;

namespace Items
{
    public enum ItemType
    {
        Normal,
        Belt,
        Extractor,
        Assembler,
        Resource,
        Inserter,
    }

    [CreateAssetMenu(menuName = "Items/ItemData", order = 'I')]
    public class ItemData : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] ItemType _type;
        [SerializeField] Sprite _sprite;
        [SerializeField, Min(1)] int _maxStack = 99;
        [SerializeField] bool _placeable;
        [SerializeField, ShowIf("_placeable")] Vector2Int _size = new Vector2Int(1,1);
        [SerializeField, ShowIf("_placeable")] ItemOrientation _orientation;

        public string Name => _name;
        public ItemType Type => _type;
        public Sprite Sprite => _sprite;
        public int MaxStack => _maxStack;
        public bool Placeable => _placeable;
        public Vector2Int Size => _size;
        public ItemOrientation Orientation => _orientation;
    }
}