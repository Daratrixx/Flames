using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Assets.Scripts {

    [RequireComponent(typeof(Collider))]
    public class CombatUnitPart : CombatTarget {

        public CombatUnit rootUnit = null;

        private void Start() {
            if(rootUnit == null) {
                Debug.LogError("A unit part was set without a root unit!");
                Destroy(this);
            }
        }

        public override CombatTarget GetRootTarget() {
            return rootUnit;
        }

        public override CombatTeam GetTeam() {
            return rootUnit.GetTeam();
        }

        public override bool IsDead() {
            return rootUnit.IsAlive();
        }
        public override bool IsAlive() {
            return rootUnit.IsAlive();
        }

        public override void Damage(int damage) {
            rootUnit.Damage(damage);
        }

        public override void Heal(int heal) {
            rootUnit.Heal(heal);
        }

        public override void Die() {
            rootUnit.Die();
        }

    }

}
