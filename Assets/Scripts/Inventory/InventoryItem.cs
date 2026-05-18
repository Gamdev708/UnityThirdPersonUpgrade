using UnityEngine;
namespace ThirdPerson.Inventory
{

    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _quantity = 1;
        
        public string ItemName => _itemName;
        public Sprite Icon => _icon;
        public int Quantity => _quantity;
    }
}
