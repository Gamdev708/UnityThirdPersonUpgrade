using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ThirdPerson.Inventory
{
    public class Inventory : MonoBehaviour
    {

        [SerializeField] private int _maxSize = 20;
        [SerializeField] List<InventoryItem> _items = new List<InventoryItem>();
        UnityEvent<InventoryItem> _onItemAdded = new UnityEvent<InventoryItem>();
        UnityEvent<InventoryItem> _onItemRemoved = new UnityEvent<InventoryItem>();

        public bool AddItem(InventoryItem item)
        {
            if (_items.Count >= _maxSize)
            {
                Debug.Log("Inventory is full. Cannot add item.");
                return false;
            }
            _items.Add(item);
            _onItemAdded.Invoke(item);
            Debug.Log($"Added {item.ItemName} to inventory.");
            return true;
        }

        public void RemoveItem(InventoryItem item)
        {
            _items.Remove(item);
            _onItemRemoved.Invoke(item);
        }
        public InventoryItem GetItem(ItemType itemType)
        {
            if (HasItem(itemType))
            {
                InventoryItem foundItem = _items.Where(item => item.ItemType == itemType).FirstOrDefault();
                return foundItem; 
            }
            return null;
        }

        public bool HasItem(InventoryItem inventoryItem)
        {
            return _items.Contains(inventoryItem);
        }

        public bool HasItem(ItemType itemType)
        {
            return _items.Any(item => item.ItemType == itemType);
        }
    }
}