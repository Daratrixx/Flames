using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ItemData")]
public class ItemData : ScriptableObject {

    private void Awake() {
        //foreach(var e in itemStatDecription) {
        //    bonusStats.Add(e.statName, e.statValue);
        //}
    }

    public Mesh itemHolderMesh;
    public Material itemHolderMaterial;

    public int itemId;

    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    public int maxStackSize = 0; // >1 is infinite

    public EquipmentSlotPosition equipmentSlot;

    public bool isEquipable {
        get {
            return equipmentSlot != EquipmentSlotPosition.None;
        }
    }

    public Dictionary<BodyAttachmentPoint, GameObject> equipmentVisiblePart = new Dictionary<BodyAttachmentPoint, GameObject>();

    //public Dictionary<string, int> bonusStats = new Dictionary<string, int>();

    public List<ItemStatDescription> itemStatDecription = new List<ItemStatDescription>();

}

[System.Serializable]
public struct ItemStatDescription {
    public string statName;
    public int statValue;
}

public enum BodyAttachmentPoint {
    Overhead, Head, Face, Neck, Shoulders, Chest, Back, Butt,
    RightShoulder, RightElbow, RightWrist, RightHand, RightWeapon,
    LeftShoulder, LeftElbow, LeftWrist, LeftHand, LeftWeapon,
    RightHips, RightKnee, RightAnkle, RightFoot,
    LeftHips, LeftKnee, LeftAnkle, LeftFoot
}