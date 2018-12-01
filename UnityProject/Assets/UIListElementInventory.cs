using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListElementInventory : UI.UIListElementBase<InventoryItem> {

    public Text itemName;
    public Text itemDescription;
    public Text itemCount;

    public Image itemIcon;

    public Button elementButton;

    public EmptyDelegate trigger;

    public void FireTrigger() {
        if (trigger != null)
            trigger();
    }

    public override void CreateView(InventoryItem dataSource) {
        base.CreateView(dataSource);
        trigger += dataSource.OnTrigger;
    }

    public override void UpdateView() {
        if (dataSource.item != null) {
            if(itemName.text != dataSource.item.itemName)itemName.text = dataSource.item.itemName;
            if(itemDescription.text != dataSource.item.itemDescription) itemDescription.text = dataSource.item.itemDescription;
            if(itemCount.text != "x" + dataSource.count)itemCount.text = "x"+dataSource.count;
            if(itemIcon.sprite != dataSource.item.itemIcon) itemIcon.sprite = dataSource.item.itemIcon;
            elementButton.interactable = true;
        } else {
            itemName.text = "No item";
            itemDescription.text = "Maybe this shouldn't be displayed?";
            itemCount.text = "x" + 0;
            itemIcon.sprite = null;
            elementButton.interactable = false;
        }
    }

}
