using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "ItemData")]
public class ItemData : ScriptableObject {

    public Mesh itemHolderMesh;
    public Material itemHolderMaterial;

}
