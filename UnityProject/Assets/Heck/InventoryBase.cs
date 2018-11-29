using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[Serializable]
public class InventoryBase : UIModelList<InventoryItem> {

    public Equiper equipment;
    public int maxSize = 10;

    public bool StoreItemIntoInventory(ItemData item, int count) {
        if (item == null) return true;
        if (count == 0) return true; // just in case...
        var current = this.Where(x => x.item.itemId == item.itemId);
        if(current.Count() > 0) {
            current.First().count += count;
            return true;
        } else if (this.Count < maxSize) {
            this.Add(new InventoryItem(this, item, count));
            return true;
        }
        return false;
    }

    public bool UnstoreItemFromInventory(ItemData item, int count) {
        if (count == 0) return true; // just in case...
        var current = this.Where(x => x.item.itemId == item.itemId).ToList();
        if (current.Count > 0) {
            foreach(var c in current) {
                if (c.count == count) { // we exactly remove the item stack
                    this.Remove(c);
                    c.FireDelete();
                    return true; // the proper amount of items was deleted
                } else if (c.count > count) { // we can remove everything that is left here
                    c.count -= count;
                    return true; // the proper amount of items was deleted
                } else { // we remove the item and we keep deleting
                    this.Remove(c);
                    c.FireDelete();
                    count -= c.count;
                }
            }
            return false; // we wanted to remove more than there was
        }
        return false; // there was nothing to remove
    }

    public bool EquipItem(InventoryItem item) {
        if(item == null || item.item == null) return false;
        if (!UnstoreItemFromInventory(item.item, 1)) return false;
        ItemData pivot = equipment.equipment.EquipItem(item.item);
        //if (pivot != null)
        //    return StoreItemIntoInventory(pivot, 1);
        return true;
    }

    public int GetItemCount(ItemData item) {
        return this.Where(x => x.item == item).Select(x => x.count).Sum();
    }
}

[Serializable]
public class InventoryItem : UIModel {

    public InventoryBase inventory;

    public void OnTrigger() {
        Debug.Log("Trigger on item " + item.itemName);
        if(item.isEquipable)
            inventory.EquipItem(this);
    }

    public InventoryItem(InventoryBase inventory,ItemData item, int count) {
        this.inventory = inventory;
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
