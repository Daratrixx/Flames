using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIListInventory : UI.UIListBase<InventoryItem> {

    [SerializeField]
    public Looter characterSource;

    private void Start() {
        if (characterSource != null && characterSource.inventory != null)
            CreateView(characterSource.inventory);
        HideView();
    }

}
