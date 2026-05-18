using System.Collections.Generic;
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

        bool AddItem(InventoryItem item)
        {
            if (_items.Count >= _maxSize) return false;
            _items.Add(item);
            _onItemAdded.Invoke(item);
            return true;
        }
        public void RemoveItem(InventoryItem item)
        {
            _items.Remove(item);
            _onItemRemoved.Invoke(item);
        }
        public InventoryItem GetItem(InventoryItem inventoryItem)
        {
            if (HasItem(inventoryItem))
            {
                return _items.Find(item => item == inventoryItem);
            }
            return null;
        }

        bool HasItem(InventoryItem inventoryItem)
        {
            return _items.Contains(inventoryItem);
        }

    }
}