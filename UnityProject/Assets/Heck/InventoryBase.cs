using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[Serializable]
public class InventoryBase : UIModelList<InventoryItem> {
    
    public int maxSize = 10;

    public bool StoreItemIntoInventory(ItemData item, int count) {
        var current = this.Where(x => x.item.itemId == item.itemId);
        if(current.Count() > 0) {
            current.First().count += count;
            current.First().FireUpdate();
            return true;
        } else if (this.Count < maxSize) {
            this.Add(new InventoryItem { item = item, count = count });
            FireUpdate();
            return true;
        }
        return false;
    }

    public int GetItemCount(ItemData item) {
        return this.Where(x => x.item == item).Select(x => x.count).Sum();
    }
}

[Serializable]
public class InventoryItem : UIModel {
    public ItemData item;
    public int count;
}
