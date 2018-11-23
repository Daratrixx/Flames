using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIController : MonoBehaviour {


    public Character character;
    public CombatUnit unit;
    public GameObject missilePrefab = null;
    protected Vector3 originalPosition;


    public TargetList possibleTargets = null;
    public CombatTarget target;
    protected Vector3 previousTargetPosition;
    public float aggroRange = -1;

    public float followDistance = 30; // maximum distance to target for chasing behaviour
    public float attackDistance = 10; // minimum distance to target for combat behaviour
    public float roamDistance = 5;

    protected float distanceToTarget;
    protected Vector3 directionToTarget;
    protected float distanceToOrigin;

    protected bool isAtacking = false;
    protected bool isFollowing = false;
    protected bool isRoaming = true;

    protected IEnumerator targetUpdateCoroutine;

    private void Start() {
        RaycastHit info;
        if (Physics.Raycast(character.transform.position, Vector3.down, out info, 1, 1 << 10))
            character.transform.position = info.point;
        originalPosition = character.transform.position;

        StartCoroutine(targetUpdateCoroutine = TargetUpdate());
    }

    private void Update() {
        if (unit == null) return;
        if (unit.IsDead()) {
            character.Die();
            character = null;
            Destroy(unit);
            unit = null;
            Destroy(this);
            return;
        }
        isAtacking = false;
        isFollowing = false;
        isRoaming = false;
        if (attackCooldown > 0)
            attackCooldown -= Time.deltaTime;
        if (target != null) {
            CombatBehaviour();
        } else {
            IdleBehaviour();
        }
    }

    #region combat

    protected void CombatBehaviour() {
        directionToTarget = target.transform.position - transform.position;
        distanceToTarget = directionToTarget.magnitude;
        if (!AttackTarget()) // if target isn't in attack range, try to catch up
            if (!FollowTarget()) // if target isn't in follow range, stop chasing
                target = null;
    }


    public float attackRate = 1;
    protected float attackCooldown = 0;

    protected bool AttackTarget() {
        if (distanceToTarget > attackDistance) return false;
        isAtacking = true;
        directionToTarget.y = 0;
        // handle attacking target behaviour
        character.Walk(directionToTarget.normalized);
        if (attackCooldown <= 0 && missilePrefab != null) {
            GameObject.Instantiate(missilePrefab, transform.position + Vector3.up, transform.rotation);
            attackCooldown = attackRate;
        }

        return true;
    }

    protected bool FollowTarget() {
        if (distanceToTarget > followDistance) return false;
        isFollowing = true;
        directionToTarget.y = 0;
        // handle following target behaviour
        character.Run(directionToTarget.normalized);

        return true;
    }

    #endregion

    #region idle

    protected void IdleBehaviour() {
        Vector3 directionToOrigin = originalPosition - transform.position;
        distanceToOrigin = directionToOrigin.magnitude;
        directionToOrigin.y = 0;
        directionToOrigin.Normalize();

        if (distanceToOrigin > roamDistance) {
            character.Run(directionToOrigin);
        } else if (distanceToOrigin > 1) {
            character.Walk(directionToOrigin);
            isRoaming = true;
        } else {
            character.NoMove();
            isRoaming = true;
        }
    }

    #endregion

    #region target

    public int targetUpdateInterval = 25; // count of FixedUpdate call between each target check
    public IEnumerator TargetUpdate() {
        while (true) {
            for (int i = 0; i < targetUpdateInterval; ++i)
                yield return new WaitForFixedUpdate();
            CheckForTarget();
        }
    }
    public bool CheckForTarget() {
        if (target != null)
            if (target.IsAlive())
                return true;
            else
                target = null;
        if (possibleTargets == null) return false;
        if (aggroRange > 0)
            target = possibleTargets.GetClosestTarget(transform.position, aggroRange);
        else
            target = possibleTargets.GetClosestTarget(transform.position);

        if (target != null)
            previousTargetPosition = target.transform.position;
        else
            return false;

        return true;
    }

    #endregion

    private void OnDrawGizmos() {
        if (unit.IsDead()) return;
        if (target != null) {
            if (isAtacking) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, attackDistance);
            } else if (isFollowing) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(transform.position, followDistance);
            }
        } else {
            if (isRoaming) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, aggroRange);
            } else {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(originalPosition, roamDistance);
            }
        }
    }
}
