using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ItemData")]
public class ItemData : ScriptableObject {

    public Mesh itemHolderMesh;
    public Material itemHolderMaterial;

    public int itemId;

    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    public EquipmentSlotPosition equipmentSlot;

    public bool isEquipable {
        get {
            return equipmentSlot != EquipmentSlotPosition.None;
        }
    }

    public Dictionary<BodyAttachmentPoint, GameObject> equipmentVisiblePart = new Dictionary<BodyAttachmentPoint, GameObject>();

}

public enum BodyAttachmentPoint {
    Overhead, Head, Face, Neck, Shoulders, Chest, Back, Butt,
    RightShoulder, RightElbow, RightWrist, RightHand, RightWeapon,
    LeftShoulder, LeftElbow, LeftWrist, LeftHand, LeftWeapon,
    RightHips, RightKnee, RightAnkle, RightFoot,
    LeftHips, LeftKnee, LeftAnkle, LeftFoot
}