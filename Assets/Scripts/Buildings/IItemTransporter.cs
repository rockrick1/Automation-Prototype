using Items;

namespace Buildings
{
    public interface IItemTransporter
    {
        public bool IsFree(ItemData item);
        public void ReserveAndExecuteReception(ItemInTransportController item);
        public void ReserveAndInstantReceive(ItemData itemData, ItemInTransportController itemInTransport = null);
        public void OnReceptionFinished(ItemInTransportController item);
        public void ExecuteTransport();
        public bool TryForceTakeItem(out ItemData output);
    }
}