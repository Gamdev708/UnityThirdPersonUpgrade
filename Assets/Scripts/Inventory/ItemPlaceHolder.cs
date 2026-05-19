using ThirdPerson.Inventory;
using UnityEngine;

public class ItemPlaceHolder : MonoBehaviour
{
    [SerializeField] InventoryItem item;
    [SerializeField] GameObject Placeholder;
    [SerializeField] ItemType requiredItem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Inventory inventory))
        {
            if (item == null)
            {
                InventoryItem inventoryItem = inventory.GetItem(requiredItem);
                if (inventoryItem != null)
                {
                    item = inventoryItem;
                    inventory.RemoveItem(inventoryItem);

                    item.transform.SetParent(Placeholder.transform);

                    item.transform.SetPositionAndRotation(Placeholder.transform.position, Placeholder.transform.rotation);
                    Vector3 parentScale = Placeholder.transform.lossyScale;
                    item.transform.localScale = new Vector3(
                        1f / parentScale.x,
                        1f / parentScale.y,
                        1f / parentScale.z);
                }
                else
                {
                    Debug.Log("Player does not have the required item.");
                }
            }
        }
    }
}
