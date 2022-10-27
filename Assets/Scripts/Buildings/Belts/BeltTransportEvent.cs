using UnityEngine;

namespace Buildings.Belts
{
    public struct ItemTransportEvent
    {
        public IItemTransporter Source;
        public IItemTransporter Target;
    }
}