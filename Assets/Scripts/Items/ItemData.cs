using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/ItemData", order = 'I')]
    public class ItemData : ScriptableObject
    {
        [SerializeField] string _name;
        [SerializeField] Vector2Int _size = new Vector2Int(1,1);
        [SerializeField] Sprite _sprite;

        public string Name => _name;
        public Vector2Int Size => _size;
        public Sprite Sprite => _sprite;
    }
}