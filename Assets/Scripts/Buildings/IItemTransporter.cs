using Items;

namespace Buildings
{
    public interface IItemTransporter
    {
        public bool IsFree();
        public void ReserveAndExecuteReception(ItemInTransportController item);
        public void ReserveAndInstantReceive(ItemInTransportController item);
        public void OnReceptionFinished(ItemInTransportController item);
        public void ExecuteTransport();
        public bool TryForceTakeItem(out ItemData output);
    }
}