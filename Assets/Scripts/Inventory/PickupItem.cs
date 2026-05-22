using ThirdPerson.Inventory;
using UnityEngine;

namespace ThirdPerson.Inventory
{
    public class PickupItem : MonoBehaviour
    {
        [field: SerializeField] public InventoryItem Item { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Inventory inventory))
            {
                if (inventory.HasItem(Item.ItemType)) { return; }// check if we already have the item, if so we can skip the pickup

                Item.transform.SetParent(inventory.transform); // detach before destroy
                if (inventory.AddItem(Item))
                {
                    Destroy(gameObject);
                }

            }
        }
    }
}