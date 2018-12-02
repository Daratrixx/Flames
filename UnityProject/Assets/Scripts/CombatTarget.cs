using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Assets.Scripts {

    public abstract class CombatTarget : MonoBehaviour {

        public abstract CombatTarget GetRootTarget();

        public abstract CombatTeam GetTeam();

        public abstract bool IsDead();
        public abstract bool IsAlive();

        public abstract void Damage(int damage);
        public abstract void Damage(int damage, int bonus);
        public abstract void Damage(int damage, int bonus, int armor);

        public abstract void Heal(int heal);

        public virtual void Die() {
            FireDeath();
            Destroy(this);
        }

        private DeathDelegate onDeath;

        public void FireDeath() {
            onDeath();
        }

        public void RegisterDeathListener(DeathDelegate d) {
            onDeath += d;
        }

        public void UnregisterDeathListener(DeathDelegate d) {
            onDeath -= d;
        }

    }

    public enum CombatTeam : int {
        player = 1 << 0,
        ally = 1 << 1,
        enemy = 1 << 2
    }

}

public delegate void DeathDelegate();
