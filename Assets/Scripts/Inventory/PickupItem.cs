using ThirdPerson.Inventory;
using UnityEngine;

namespace ThirdPerson.Inventory
{
    public class PickupItem : MonoBehaviour
    {
        [field: SerializeField] public InventoryItem Item { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided with " + other.gameObject.name);
            if (other.TryGetComponent(out Inventory inventory))
            {
                if (inventory.AddItem(GetComponent<PickupItem>().Item))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}