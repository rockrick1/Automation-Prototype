﻿using Items;

namespace Buildings
{
    public interface IItemReceiver
    {
        public bool IsFree();
        public void ReceiveItem(ItemInTransportController item);
    }
}