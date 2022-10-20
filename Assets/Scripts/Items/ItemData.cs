using NaughtyAttributes;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/ItemData", order = 'I')]
    public class ItemData : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] Sprite _sprite;
        [SerializeField, Min(1)] int _maxStack = 99;
        [SerializeField] bool _placeable;
        [SerializeField, ShowIf("_placeable")] Vector2Int _size = new Vector2Int(1,1);

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public int MaxStack => _maxStack;
        public bool Placeable => _placeable;
        public Vector2Int Size => _size;
    }
}