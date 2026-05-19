using UnityEngine;
namespace ThirdPerson.Inventory
{

    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Sprite _icon;

        public string ItemName => _itemName;
        public Sprite Icon => _icon;
        
    }
}
