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

        public float maxDistance = 20;
        public float currentDistance = 0;

        private Vector3 origin;

        private void Start() {
            origin = transform.position;
        }

        private void Update() {
            transform.position += transform.forward * speed * Time.deltaTime;
            if(Vector3.Distance(origin,transform.position) >= maxDistance) {
                Debug.Log("Missile expired");
                Destroy(gameObject);
            }
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
                if (affectedTargets.Contains(unit.GetRootTarget())) return; // unit already affected
                affectedTargets.Add(unit.GetRootTarget());
                if (heal > 0) unit.Heal(heal);
                if (damage > 0) unit.Damage(damage);
                Destroy(gameObject);
            } else if(other.gameObject.layer == obstacleLayer) {
                Debug.Log("Missile hit a wall!");
                Destroy(gameObject);
            }
        }

        private List<CombatTarget> affectedTargets = new List<CombatTarget>();

        public static Missile SpawnMissile(Vector3 position, Vector3 direction, string missilePrefab) {
            return Resources.Load<GameObject>("Prefabs/Missile/" + missilePrefab).GetComponent<Missile>();
        }

    }
}