using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Assets.Scripts {

    public class CombatUnit : CombatTarget {

        public CombatTeam team = CombatTeam.player;

        public int currentHealth;
        public int maxHealth;

        public override CombatTeam GetTeam() {
            return team;
        }

        public override bool IsDead() {
            return currentHealth <= 0;
        }
        public override bool IsAlive() {
            return currentHealth > 0;
        }

        public override void Damage(int damage) {
            currentHealth -= damage;
            Debug.Log("unit damaged! " + currentHealth + "/" + maxHealth + " health remaining!");
        }

        public override void Heal(int heal) {
            currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
            Debug.Log("unit healed! " + currentHealth + "/" + maxHealth + " health remaining!");
        }

        public override void Die() {

        }

    }

}
