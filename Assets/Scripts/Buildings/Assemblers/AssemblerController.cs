using Items;
using System.Collections;

namespace Buildings.Assemblers
{
    public class AssemblerController : PlaceableItemController, IItemTransporter
    {
        Recipe _selectedRecipe;

        ItemSlot _input1;
        ItemSlot _input2;
        ItemSlot _output;

        public override void Init(ItemOrientation oritentation)
        {
            StartCoroutine(ProcessItemsRoutine());
        }

        public void SetRecipe(Recipe recipe)
        {
            _selectedRecipe = recipe;
        }

        IEnumerator ProcessItemsRoutine()
        {
            while (true)
            {
                if (_selectedRecipe == null)
                {
                    yield return null;
                    continue;
                }



                yield return null;
            }
        }

        public void ExecuteTransport()
        {
        }

        public bool IsFree(ItemData item)
        {
            return (_input1.ItemData == null || (item == _input1.ItemData && _input1.Quantity < item.MaxStack)) ||
                (_input2.ItemData == null || (item == _input2.ItemData && _input2.Quantity < item.MaxStack));
        }

        public void OnReceptionFinished(ItemInTransportController item)
        {
        }

        public void ReserveAndExecuteReception(ItemInTransportController item)
        {
        }

        public void ReserveAndInstantReceive(ItemData itemData, ItemInTransportController itemInTransport = null)
        {
            //receive item here
        }

        public bool TryForceTakeItem(out ItemData output)
        {
            output = null;

            if (_output.Quantity > 0)
            {
                output = _output.ItemData;
                _output.Quantity--;

                return true;
            }

            return false;
        }

        struct ItemSlot
        {
            public int Quantity;
            public ItemData ItemData;
        }
    }
}