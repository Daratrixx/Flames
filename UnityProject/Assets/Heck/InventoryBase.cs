using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[Serializable]
public class InventoryBase : UIModelList<InventoryItem> {
    
    public int maxSize = 10;

    public bool StoreItemIntoInventory(ItemData item, int count) {
        var current = this.Where(x => x.item == item);
        if(current.Count() > 0) {
            current.First().count += count;
            return true;
        } else if (this.Count < maxSize) {
            this.Add(new InventoryItem(item, count));
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

    public InventoryItem(ItemData item, int count) {
        this.item = item;
        this.COUNT = count;
    }

    public ItemData item;

    public int count {
        set { COUNT = value; FireUpdate(); }
        get { return COUNT; }
    }

    private int COUNT;
}
