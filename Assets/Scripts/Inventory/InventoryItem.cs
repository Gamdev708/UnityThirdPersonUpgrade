using UnityEngine;
namespace ThirdPerson.Inventory
{
    public enum ItemType
    {
        Battery,
        Capacitor,
        Fuses
    }
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemType _itemType;

        public string ItemName => _itemName;
        public Sprite Icon => _icon;
        public ItemType ItemType => _itemType;

    }
}
