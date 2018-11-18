using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Assets.Scripts {

    public abstract class CombatTarget : MonoBehaviour {

        public abstract CombatTeam GetTeam();

        public abstract bool IsDead();
        public abstract bool IsAlive();

        public abstract void Damage(int damage);

        public abstract void Heal(int heal);

        public abstract void Die();

    }

    public enum CombatTeam : int {
        player = 1 << 0,
        ally = 1 << 1,
        enemy = 1 << 2
    }

}
