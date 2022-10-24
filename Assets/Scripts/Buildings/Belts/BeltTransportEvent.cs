using UnityEngine;

namespace Buildings.Belts
{
    public struct BeltTransportEvent
    {
        public BeltController Source;
        public IItemReceiver Target;
    }
}