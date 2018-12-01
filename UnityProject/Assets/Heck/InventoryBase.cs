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
        foreach (var c in current) {
            if (c.item.maxStackSize < 1) {
                c.count += count; // no stack limit, stack everything at once
                return true; // the proper amount of items was added
            } else if (c.item.maxStackSize - c.count == count) { // we exactly fill the item stack
                c.count = c.item.maxStackSize;
                return true; // the proper amount of items was added
            } else if (c.item.maxStackSize - c.count > count) { // we can add everything that is left here
                c.count += count;
                return true; // the proper amount of items was added
            } else if (c.item.maxStackSize - c.count < count) { // we add the item to the stack and we keep adding
                count -= (c.item.maxStackSize - c.count);
                c.count = c.item.maxStackSize;
            }
        }
        while (this.Count < maxSize || count > 0) {
            if (item.maxStackSize < 1 || item.maxStackSize >= count) {
                this.Add(new InventoryItem(this, item, count));
                return true;
            } else {
                this.Add(new InventoryItem(this, item, item.maxStackSize));
                count -= item.maxStackSize;
            }
        }
        return false;
    }

    public bool UnstoreItemFromInventory(ItemData item, int count) {
        if (count == 0) return true; // just in case...
        var current = this.Where(x => x.item.itemId == item.itemId).ToList();
        foreach (var c in current) {
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
        return false; // there was nothing to remove/we couldn't remove enough
    }

    public bool EquipItem(InventoryItem item) {
        if (item == null || item.item == null) return false;
        if (!UnstoreItemFromInventory(item.item, 1)) return false;
        equipment.equipment.EquipItem(item.item);
        return true;
    }

    public int GetItemCount(ItemData item) {
        return this.Where(x => x.item == item).Select(x => x.count).Sum();
    }
}

public class InventoryItem : UIModel {

    private InventoryBase inventory { get; set; }

    public void OnTrigger() {
        if (item.isEquipable)
            inventory.EquipItem(this);
    }

    public InventoryItem(InventoryBase inventory, ItemData item, int count) {
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
