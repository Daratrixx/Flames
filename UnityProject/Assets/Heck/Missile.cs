using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class Missile : MonoBehaviour {

        public CombatTeam affects = CombatTeam.enemy;

        public int damage = 30;
        public int heal = 0;

        public float speed = 10;

        public const int obstacleLayer = 10;
        public const int characterLayer = 11;

        private void Update() {
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other) {
            CombatTarget unit;
            /*if (other.gameObject.layer == obstacleLayer) {
                // we hit an obstacle, the missile is consumed
                Destroy(gameObject);
            } else*/
            if ((unit = other.GetComponent<CombatTarget>()) != null) {
                // we hit a unit, damage or heal accordingly and consume the missile
                if (((int)affects & (int)unit.GetTeam()) == 0) return; // unit is not affected
                if (heal > 0) unit.Heal(heal);
                if (damage > 0) unit.Damage(damage);
                Destroy(gameObject);
            } else if(other.gameObject.layer == obstacleLayer) {
                Debug.Log("Missile hit a wall!");
                Destroy(gameObject);
            }
        }

        public static Missile SpawnMissile(Vector3 position, Vector3 direction, string missilePrefab) {
            return Resources.Load<GameObject>("Prefabs/Missile/" + missilePrefab).GetComponent<Missile>();
        }

    }
}