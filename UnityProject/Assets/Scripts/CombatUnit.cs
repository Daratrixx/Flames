﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

namespace Assets.Scripts {

    public class CombatUnit : CombatTarget, UIModelInterface {

        public override CombatTarget GetRootTarget() {
            return this;
        }

        public CombatTeam team = CombatTeam.player;

        #region stats

        public int currentHealth = 100;

        [SerializeField]
        private int _maxHealth = 100;
        public int maxHealth {
            get { return _maxHealth; }
            set {
                float percent = ((float)currentHealth / (float)maxHealth);
                _maxHealth = value;
                currentHealth = (int)Math.Round(_maxHealth * percent);
            }
        }

        [SerializeField]
        private int _armor = 0;
        public int armor { get { return _armor; } set { _armor = value; } }

        [SerializeField]
        private int _power = 0;
        public int power { get { return _power; } set { _power = value; } }

        #endregion

        public LootTable lootOnDeath;

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
            FireDamaged();
        }

        public override void Damage(int damage, int bonus) {
            currentHealth -= (int)(damage * (100 + bonus - armor) / 100.0f);
            Debug.Log("unit damaged! " + currentHealth + "/" + maxHealth + " health remaining!");
            FireDamaged();
        }

        public override void Damage(int damage, int bonus, int armor) {
            currentHealth -= (int)(damage * (100 + bonus - armor) / 100.0f);
            Debug.Log("unit damaged! " + currentHealth + "/" + maxHealth + " health remaining!");
            FireDamaged();
        }

        public override void Heal(int heal) {
            currentHealth = Mathf.Min(currentHealth + heal, maxHealth);
            Debug.Log("unit healed! " + currentHealth + "/" + maxHealth + " health remaining!");
        }

        public override void Die() {
            base.Die();
            LootTable.LootUnit(this);
        }

        private void LateUpdate() {
            if (IsDead())
                Die();
        }

        public EmptyDelegate OnDamaged;
        public EmptyDelegate OnHealed;
        public EmptyDelegate OnUpdate;
        public EmptyDelegate OnDelete;

        public void FireDamaged() {
            if (OnDamaged != null)
                OnDamaged();
        }
        public void FireHealed() {
            if (OnHealed != null)
                OnHealed();
        }

        public void FireUpdate() {
            if (OnUpdate != null)
                OnUpdate();
        }

        public void FireDelete() {
            if (OnDelete != null)
                OnDelete();
        }

        public void RegisterDamagedListener(EmptyDelegate del) {
            OnDamaged += del;
        }

        public void RegisterHealedListener(EmptyDelegate del) {
            OnHealed += del;
        }

        public void RegisterUpdateListener(EmptyDelegate del) {
            OnUpdate += del;
        }

        public void RegisterDeleteListener(EmptyDelegate del) {
            OnDelete += del;
        }

        public void UnregisterDamagedListener(EmptyDelegate del) {
            OnDamaged -= del;
        }

        public void UnregisterHealedListener(EmptyDelegate del) {
            OnHealed -= del;
        }

        public void UnregisterUpdateListener(EmptyDelegate del) {
            OnUpdate -= del;
        }

        public void UnregisterDeleteListener(EmptyDelegate del) {
            OnDelete -= del;
        }
    }

}
