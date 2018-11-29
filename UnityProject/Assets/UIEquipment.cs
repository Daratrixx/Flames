using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipment : UI.UIView<EquipmentBase> {

    public UISlotEquipment Head;
    public UISlotEquipment Chest;
    public UISlotEquipment Back;
    public UISlotEquipment Legs;
    public UISlotEquipment Weapon;

    public Equiper characterSource;

    private void Start() {
        if(characterSource != null) {
            CreateView(characterSource.equipment);
        }
        HideView();
    }

    public override void CreateView(EquipmentBase dataSource) {
        base.CreateView(dataSource);
        Head.CreateView(dataSource.slots[EquipmentSlotPosition.Head]);
        Chest.CreateView(dataSource.slots[EquipmentSlotPosition.Chest]);
        Back.CreateView(dataSource.slots[EquipmentSlotPosition.Back]);
        Legs.CreateView(dataSource.slots[EquipmentSlotPosition.Legs]);
        Weapon.CreateView(dataSource.slots[EquipmentSlotPosition.Weapon]);
    }

    public override void UpdateView() {

    }

}
