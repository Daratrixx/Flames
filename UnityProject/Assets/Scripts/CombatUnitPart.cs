using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Assets.Scripts {

    [RequireComponent(typeof(Collider))]
    public class CombatUnitPart : CombatAttackable {

        public CombatUnit rootUnit = null;

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
