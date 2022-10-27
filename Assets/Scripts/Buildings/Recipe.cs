using Items;
using System;
using UnityEngine;

namespace Buildings
{
    [CreateAssetMenu(menuName = "Recipes/Recipe", order = 'R')]
    public class Recipe : ScriptableObject
    {
        [SerializeField] ItemType _craftedOn;
        [SerializeField] Slot _input1;
        [SerializeField] Slot _input2;
        [SerializeField] Slot _output;

        [Serializable]
        struct Slot
        {
            public ItemData ItemData;
            public int Quantity;
        }
    }
}