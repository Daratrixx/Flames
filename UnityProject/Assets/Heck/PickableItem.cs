using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class PickableItem : MonoBehaviour {

        public ItemData item = null;
        public int count = 1;
        private void Start() {
            if (item != null) {
                GetComponentInChildren<MeshFilter>().mesh = item.itemHolderMesh; // renderer
                GetComponentInChildren<MeshRenderer>().material = item.itemHolderMaterial; // renderer
            }
        }

        private void OnTriggerEnter(Collider other) {
            Character character;
            if ((character = other.GetComponent<Character>()) != null) {
                // add the item to the inventory
                if (character.inventory.StoreItemIntoInventory(item, count))
                    // shrink and remove the item
                    StartCoroutine(DestroyItem());
            }
        }

        public const float itemDestroyDuration = 2;

        private IEnumerator DestroyItem() {
            ParticleSystem ps;
            Vector3 scale = transform.localScale;
            if ((ps = GetComponent<ParticleSystem>()) != null) {
                ps.Stop();
            }
            float progression = itemDestroyDuration;
            while (progression > 0) {
                yield return null;
                progression -= Time.deltaTime;
                transform.localScale = scale * (progression / itemDestroyDuration);
            }
            Destroy(this.gameObject);
        }
    }
}