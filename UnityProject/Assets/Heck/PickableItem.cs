using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class PickableItem : MonoBehaviour {

        private void OnTriggerEnter(Collider other) {
            Character character;
            if ((character = other.GetComponent<Character>()) != null) {
                StartCoroutine(DestroyItem());
            }
        }

        public const float itemDestroyDuration = 1;

        private IEnumerator DestroyItem() {
            ParticleSystem ps;
            Vector3 scale = transform.localScale;
            if ((ps = GetComponent<ParticleSystem>()) != null) {
                ps.Stop();
            }
            float progression = itemDestroyDuration;
            while(progression > 0) {
                yield return null;
                progression -= Time.deltaTime;
                transform.localScale = scale * (progression / itemDestroyDuration); 
            }
            Destroy(this.gameObject);
        }
    }
}