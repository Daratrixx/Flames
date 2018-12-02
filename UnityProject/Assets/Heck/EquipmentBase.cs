using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class EquipmentBase : UIModel {
    [SerializeField]
    public Dictionary<EquipmentSlotPosition, EquipmentSlot> slots = new Dictionary<EquipmentSlotPosition, EquipmentSlot>();
    [SerializeField]
    public Dictionary<BodyAttachmentPoint, Transform> bones = new Dictionary<BodyAttachmentPoint, Transform>();

    public Looter inventory;

    public CombatUnit unit;

    public ItemData EquipItem(ItemData item) {
        ItemData output = null;
        if (item == null) return null;
        EquipmentSlot slot = slots[item.equipmentSlot];
        // remove current item, if any
        if (slot.item != null) {
            output = slot.item;
            // remove stats
            UnapplyItemStat(slot.item);
            // remove visible parts
            foreach (GameObject go in slot.equipedObject)
                GameObject.Destroy(go);
            slot.equipedObject.Clear();
            // put item back inside the inventory
            if (!inventory.inventory.StoreItemIntoInventory(output, 1)) return null;
        }
        // apply stats
        ApplyItemStat(item);
        // add visible object
        foreach (var pair in item.equipmentVisiblePart) {
            GameObject obj;
            if ((obj = AddObjectToAttachmentPoint(pair.Value, pair.Key)) != null)
                slot.equipedObject.Add(obj);
        }
        slot.item = item;
        return output;
    }
    public ItemData UnequipInventorySlot(EquipmentSlot slot) {
        ItemData output = null;
        if (slot.item == null) return null;
        output = slot.item;
        if (!inventory.inventory.StoreItemIntoInventory(output, 1)) // check if there is room for the item to be unequipped
            return null;
        // remove stats
        UnapplyItemStat(slot.item);
        // remove visible parts
        foreach (GameObject go in slot.equipedObject)
            GameObject.Destroy(go);
        slot.equipedObject.Clear();
        // unbind item data
        slot.item = null;
        return output;
    }

    public GameObject AddObjectToAttachmentPoint(GameObject go, BodyAttachmentPoint ap) {
        Transform anchor = bones[ap];
        if (anchor != null) return GameObject.Instantiate(go, anchor);
        return null;
    }

    private void ApplyItemStat(ItemData item) {
        StatShiftDirection(item.itemStatDecription, 1);
    }

    private void UnapplyItemStat(ItemData item) {
        StatShiftDirection(item.itemStatDecription, -1);
    }

    private void StatShiftDirection(ICollection<ItemStatDescription> stats, int direction) {
        var type = unit.GetType();
        //var flag = System.Reflection.BindingFlags.Public;
        if (direction > 0) {
            foreach (var p in stats) {
                var propertyInfo = type.GetProperty(p.statName);
                if (propertyInfo != null) propertyInfo.SetValue(unit, (int)propertyInfo.GetValue(unit, null) + p.statValue, null);
                else Debug.Log("Unknown property: " + p.statName);
            }
        } else {
            foreach (var p in stats) {
                var propertyInfo = type.GetProperty(p.statName);
                if (propertyInfo != null) propertyInfo.SetValue(unit, (int)propertyInfo.GetValue(unit, null) - p.statValue, null);
                else Debug.Log("Unknown property: " + p.statName);
            }
        }
    }

    public EquipmentBase() {
        for (EquipmentSlotPosition s = EquipmentSlotPosition.None; s < EquipmentSlotPosition.__LIMIT_DONT_USE; ++s) {
            slots.Add(s, new EquipmentSlot(this, s));
        }
    }

}

public class EquipmentSlot : UIModel {

    private EquipmentBase equipment { get; set; }

    public EquipmentSlot(EquipmentBase equipment, EquipmentSlotPosition slot) {
        this.equipment = equipment;
        this.slot = slot;
    }

    public void OnTrigger() {
        equipment.UnequipInventorySlot(this);
    }

    public EquipmentSlotPosition slot { get; private set; }

    public Transform bone;

    public ItemData item {
        set { CURRENT_ITEM = value; FireUpdate(); }
        get { return CURRENT_ITEM; }
    }

    private ItemData CURRENT_ITEM = null;

    // ref to graphical instance of the item
    public List<GameObject> equipedObject = new List<GameObject>();

    public bool isUsed {
        get { return item != null; }
    }
}

public enum EquipmentSlotPosition {
    None, Head, Chest, Back, Legs, Weapon, __LIMIT_DONT_USE
}
