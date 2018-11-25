using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class PickableItem : MonoBehaviour {

        public ItemData item = null;
        public int count = 1;

        private bool consumed = false;

        private void Start() {
            InitVisibility();
        }

        public void InitVisibility() {
            if (item != null) {
                GetComponentInChildren<MeshFilter>().mesh = item.itemHolderMesh; // renderer
                GetComponentInChildren<MeshRenderer>().material = item.itemHolderMaterial; // renderer
            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (consumed) return;
            if(collision.collider.gameObject.layer == 10) {
                Debug.Log("Obstacle hit!");
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (consumed) return;
            if (other.gameObject.layer == 10) {
                Debug.Log("Obstacle hit!");
                GetComponent<Rigidbody>().isKinematic = true;
            }
            Looter looter;
            if ((looter = other.GetComponent<Looter>()) != null) {
                // add the item to the inventory
                if (looter.inventory.StoreItemIntoInventory(item, count)) {
                    consumed = true;
                    // shrink and remove the item
                    StartCoroutine(DestroyItem());
                }
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