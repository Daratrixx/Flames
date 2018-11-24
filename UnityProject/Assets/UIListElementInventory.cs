using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListElementInventory : UI.UIListElementBase<InventoryItem> {

    public Text itemName;
    public Text itemDescription;
    public Text itemCount;

    public Image itemIcon;

    public override void CreateView(InventoryItem dataSource) {
        base.CreateView(dataSource);
    }

    public override void UpdateView() {
        if (dataSource.item != null) {
            itemName.text = dataSource.item.itemName;
            itemDescription.text = dataSource.item.itemDescription;
            itemCount.text = "x"+dataSource.count;
            itemIcon.sprite = dataSource.item.itemIcon;
        } else {
            itemName.text = "No item";
            itemDescription.text = "Maybe this shouldn't be displayed?";
            itemCount.text = "x" + 0;
            itemIcon.sprite = null;
        }
    }

}
