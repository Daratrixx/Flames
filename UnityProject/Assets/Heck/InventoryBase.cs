using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryBase : MonoBehaviour {

    public List<InventoryItem> items;
    public int size;

    public bool StoreItemIntoInventory(ItemData item, int count) {
        var current = items.Where(x => x.item == item);
        if(current.Count() > 0) {
            current.First().count += count;
            return true;
        } else if (items.Count < size) {
            items.Add(new InventoryItem { item = item, count= count });
            return true;
        }
        return false;
    }

    public int GetItemCount(ItemData item) {
        return items.Where(x => x.item == item).Select(x => x.count).Sum();
    }
}

[Serializable]
public class InventoryItem {
    public ItemData item;
    public int count;
}
