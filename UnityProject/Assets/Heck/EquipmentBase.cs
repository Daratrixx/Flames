using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBase : MonoBehaviour {

    public Dictionary<EquipmentSlot, InventorySlot> slots = new Dictionary<EquipmentSlot, InventorySlot>();
    public Dictionary<BodyAttachmentPoint, Transform> bones = new Dictionary<BodyAttachmentPoint, Transform>();

    public void EquipItem(ItemData item) {
        if (item == null) return;
        InventorySlot slot = slots[item.equipmentSlot];
        if (slot.currentItem == item) return;
        // remove current item, if any
        UnequipInventorySlot(slot);
        // apply stats
        //TODO
        // add visible object
        foreach (var pair in item.equipmentVisiblePart) {
            GameObject obj;
            if ((obj = AddObjectToAttachmentPoint(pair.Value, pair.Key)) != null)
                slot.equipedObject.Add(obj);
        }
    }

    public void UnequipInventorySlot(InventorySlot slot) {
        if (slot == null || slot.currentItem == null) return;
        // remove stats
        //TODO
        // remove visible parts
        foreach (GameObject go in slot.equipedObject)
            Destroy(go);
        // unbind item data
        slot.currentItem = null;
    }

    public GameObject AddObjectToAttachmentPoint(GameObject go, BodyAttachmentPoint ap) {
        Transform anchor = bones[ap];
        if (anchor) return Instantiate(go, anchor);
        return null;
    }

}

[Serializable]
public class InventorySlot {
    public Transform bone;
    public ItemData currentItem;
    public List<GameObject> equipedObject;

    public bool isUsed {
        get { return currentItem != null; }
    }
}

public enum EquipmentSlot {
    None, Head, Chest, Back, Legs, Weapon
}
