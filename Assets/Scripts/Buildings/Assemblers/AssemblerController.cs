using Items;

namespace Buildings.Assemblers
{
    public class AssemblerController : PlaceableItemController//, IItemTransporter
    {
        public bool IsFree()
        {
            return true;
        }

        public void OnReceptionFinished(ItemInTransportController item)
        {
        }

        public void ReserveAndExecuteReception(ItemInTransportController item)
        {
        }

        public void ReserveAndInstantReceive(ItemInTransportController item)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteTransport()
        {
            throw new System.NotImplementedException();
        }
    }
}