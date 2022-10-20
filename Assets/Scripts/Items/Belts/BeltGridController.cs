using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Items
{
    public class BeltGridController : Dependable
    {
        [SerializeField] Tilemap _tilemap;
        [SerializeField] Animator _beltSpritesAnimationSynchronizer;

        public Animator AnimSync => _beltSpritesAnimationSynchronizer;

        // Update is called once per frame
        void Update()
        {

        }
    }
}