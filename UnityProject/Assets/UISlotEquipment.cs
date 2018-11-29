using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlotEquipment : UI.UIView<EquipmentSlot> {

    public Text itemSlot;

    public Text itemName;
    public Text itemDescription;

    public Image itemIcon;

    public Button elementButton;

    public EmptyDelegate trigger;

    public void FireTrigger() {
        if(trigger != null)
            trigger();
    }

    public override void CreateView(EquipmentSlot dataSource) {
        base.CreateView(dataSource);
        trigger += dataSource.OnTrigger;
        itemSlot.text = dataSource.slot.ToString();
    }

    public override void UpdateView() {
        if (dataSource.item != null) {
            if(itemName.text != dataSource.item.itemName)itemName.text = dataSource.item.itemName;
            if(itemDescription.text != dataSource.item.itemDescription) itemDescription.text = dataSource.item.itemDescription;
            if(itemIcon.sprite != dataSource.item.itemIcon) itemIcon.sprite = dataSource.item.itemIcon;
            elementButton.interactable = true;
        } else {
            itemName.text = "No item";
            itemDescription.text = "Maybe this shouldn't be displayed?";
            itemIcon.sprite = null;
            elementButton.interactable = false;
        }
    }

}
