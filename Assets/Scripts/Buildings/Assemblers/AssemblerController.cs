using Items;

namespace Buildings.Assemblers
{
    public class AssemblerController : PlaceableItemController, IItemReceiver
    {
        public bool IsFree()
        {
            return true;
        }

        public void ReceiveItem(ItemInTransportController item)
        {
        }

        public void ReserveReception(ItemInTransportController item)
        {
        }
    }
}