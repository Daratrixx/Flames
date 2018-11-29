using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Table", menuName = "LootTable")]
public class LootTable : ScriptableObject {
    [SerializeField]
    public LootTableEntry[] entries = new LootTableEntry[0];
    
    private static GameObject prefab = null;

    public static void LootUnit(CombatUnit unit) {
        if (prefab == null) prefab = Resources.Load<GameObject>("Prefabs/ItemHolder");
        if(unit.lootOnDeath != null) {
            foreach(LootTableEntry e in unit.lootOnDeath.entries) {
                if (UnityEngine.Random.value < e.probability) {
                    GameObject go = Instantiate(prefab, unit.transform.position + Vector3.up, Quaternion.identity);
                    PickableItem pi = go.GetComponent<PickableItem>();
                    pi.item = e.item;
                    pi.count = UnityEngine.Random.Range(e.minCount, e.maxCount);
                    pi.InitVisibility(); // update mesh and material
                    float forceIntensity = UnityEngine.Random.Range(100, 500);
                    Vector3 forceDirection = Quaternion.Euler(UnityEngine.Random.Range(0,45), UnityEngine.Random.Range(0,360), 0) * Vector3.forward;
                    go.GetComponent<Rigidbody>().AddForce(forceDirection * forceIntensity);
                }
            }
        }
    }

}

[Serializable]
public class LootTableEntry {
    public ItemData item;
    public int minCount = 1;
    public int maxCount = 0;
    public float probability; // between 0 and 1
}