using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIControllerUpgraded : AIController {

    public IdlePattern idlePattern = IdlePattern.Stand;

    public List<Vector3> path = new List<Vector3>();
    private int pathProgression = 0;
    private int pathProgressionDirection = 1;
    public float idleMovementStepDelay; // for all but IdlePattern.Stand, delay between two movements of a sequence
    public float idleMovementResetDelay; // for IdlePattern.Patrol and IdlePattern.Loop, delay before changing direction/go back to the start
    public float idleRoamRadius; // for IdlePattern.RoamCenter, the maximum distance from the original position a roam point can be
    public float idleRoamMaxDistance; // for IdlePattern.RoamFree, the maximum distance for each destination

    public AttackPattern attackPattern = AttackPattern.Rush;
    public float salvoAttackPreparationDuration = 1;
    public int salvoAttackCount = 3;
    public float salvoAttackInterval = 0.2f;
    private float salvoAttackPreparationTimer = -1;
    private int salvoAttackProgression = 0;
    private float salvoAttackIntervalTimer = -1;


    private void Start() {
        RaycastHit info;
        if (Physics.Raycast(character.transform.position, Vector3.down, out info, 1, 1 << 10))
            character.transform.position = info.point;
        originalPosition = character.transform.position;

        unit.RegisterDeathListener(Die);

        StartCoroutine(targetUpdateCoroutine = TargetUpdate());

        path.Insert(0, roamGoal = originalPosition); // ensure the start of the path is the initial position
    }

    private void Update() {
        if (unit == null) return;
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
    
    protected override bool AttackTarget() {
        if (missilePrefab == null) return false;
        switch(attackPattern) {
            case AttackPattern.Rush:
                return RushBehaviour();
            case AttackPattern.Salvo:
                return SalvoBehaviour();
            default:
                return false;
        }
    }

    protected virtual bool RushBehaviour() {
        if (distanceToTarget > attackDistance) return false;
        isAtacking = true;
        directionToTarget.y = 0;
        // handle attacking target behaviour
        character.Walk(directionToTarget.normalized);
        if (attackCooldown <= 0) {
            Missile m = GameObject.Instantiate(missilePrefab, transform.position + Vector3.up, transform.rotation).GetComponent<Missile>();
            m.bonus = unit.power;
            attackCooldown = attackRate;
        }

        return true;
    }

    protected virtual bool SalvoBehaviour() {
        if(salvoAttackProgression != 0) {
            if (salvoAttackPreparationTimer > 0) {
                salvoAttackPreparationTimer -= Time.deltaTime;
                character.Rotate(directionToTarget.normalized);
                return true;
            }
            if (salvoAttackIntervalTimer > 0) { // between two shots
                salvoAttackIntervalTimer -= Time.deltaTime;
                character.Rotate(directionToTarget.normalized);
                return true;
            }
            if (salvoAttackProgression > 0) { // actual shot
                character.Rotate(directionToTarget.normalized); // rotate
                salvoAttackProgression--; // progress salvo
                salvoAttackIntervalTimer = salvoAttackInterval; // start timer for next shot
                Missile m = GameObject.Instantiate(missilePrefab, transform.position + Vector3.up, transform.rotation).GetComponent<Missile>(); // shoot
                m.bonus = unit.power; // apply shooter bonuses
                return true;
            }
        }
        if (attackCooldown > 0) { // between 2 salvo
            attackCooldown -= Time.deltaTime;
            return false; // reloading, will just follow
        }
        if (distanceToTarget > attackDistance) // too far, just follow
            return false;
        // ready to initate salvo!!!
        salvoAttackProgression = salvoAttackCount;
        salvoAttackPreparationTimer = salvoAttackPreparationDuration;
        attackCooldown = attackRate;
        return true;
    }

    #endregion

    #region idle

    protected override void IdleBehaviour() {
        switch (idlePattern) {
            case IdlePattern.Patrol:
                PatrolBehaviour();
                break;
            case IdlePattern.Loop:
                LoopBehaviour();
                break;
            case IdlePattern.RoamCenter:
                RoamCenterBehaviour();
                break;
            case IdlePattern.RoamFree:
                RoamFreeBehaviour();
                break;
        }
    }

    protected virtual void StandBehaviour() {
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

    protected float idleMovementStepTimer = 0;
    protected float idleMovementResetTimer = 0;

    protected virtual void PatrolBehaviour() {
        if (idleMovementStepTimer > 0) {
            character.NoMove();
            idleMovementStepTimer -= Time.deltaTime;
            return;
        } else if (idleMovementResetTimer > 0) {
            character.NoMove();
            idleMovementResetTimer -= Time.deltaTime;
            return;
        }
        Vector3 goal = path[pathProgression];
        Vector3 directionToGoal = goal - transform.position;
        float distanceToGoal = directionToGoal.magnitude;
        directionToGoal.y = 0;
        directionToGoal.Normalize();

        if (distanceToGoal > character.walkSpeed * 0.25f) {
            character.Walk(directionToGoal);
            isRoaming = true;
        } else {
            pathProgression += pathProgressionDirection;
            if (pathProgression == -1 || pathProgression == path.Count) { // reached the end, wait then go back
                idleMovementResetTimer = idleMovementResetDelay; // start reset timer
                pathProgressionDirection = -pathProgressionDirection; // change direction
                pathProgression += 2 * pathProgressionDirection; // next target is current target -1 (*2 is to compensate the overflow
            } else { // just wait the regular pause
                idleMovementStepTimer = idleMovementStepDelay; // start step timer
            }
            if (idleMovementResetTimer > 0 || idleMovementStepTimer > 0) // if there's a pause, interupt walking cycle
                character.NoMove();
            isRoaming = true;
        }
    }

    protected virtual void LoopBehaviour() {
        if (idleMovementStepTimer > 0) {
            character.NoMove();
            idleMovementStepTimer -= Time.deltaTime;
            return;
        } else if (idleMovementResetTimer > 0) {
            character.NoMove();
            idleMovementResetTimer -= Time.deltaTime;
            return;
        }
        Vector3 goal = path[pathProgression];
        Vector3 directionToGoal = goal - transform.position;
        float distanceToGoal = directionToGoal.magnitude;
        directionToGoal.y = 0;
        directionToGoal.Normalize();

        if (distanceToGoal > character.walkSpeed * 0.25f) {
            character.Walk(directionToGoal);
            isRoaming = true;
        } else {
            pathProgression += 1;
            if (pathProgression == path.Count) { // reached the end, wait then go back
                idleMovementResetTimer = idleMovementResetDelay; // start reset timer
                pathProgression = 0; // next target is original position
            } else { // just wait the regular pause
                idleMovementStepTimer = idleMovementStepDelay; // start step timer
            }
            if (idleMovementResetTimer > 0 || idleMovementStepTimer > 0) // if there's a pause, interupt walking cycle
                character.NoMove();
            isRoaming = true;
        }
    }

    protected Vector3 roamGoal;

    protected virtual void RoamCenterBehaviour() {
        if (idleMovementStepTimer > 0) {
            character.NoMove();
            idleMovementStepTimer -= Time.deltaTime;
            return;
        }

        Vector3 directionToGoal = roamGoal - transform.position;
        float distanceToGoal = directionToGoal.magnitude;
        directionToGoal.y = 0;
        directionToGoal.Normalize();

        if (distanceToGoal > character.walkSpeed * 0.25f) {
            character.Walk(directionToGoal);
            isRoaming = true;
        } else {
            float angle = UnityEngine.Random.value * 360;
            float distance = UnityEngine.Random.value * idleRoamRadius;
            roamGoal = originalPosition + Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
            idleMovementStepTimer = idleMovementStepDelay; // start step timer
            if (idleMovementResetTimer > 0 || idleMovementStepTimer > 0) // if there's a pause, interupt walking cycle
                character.NoMove();
            isRoaming = true;
        }
    }

    protected virtual void RoamFreeBehaviour() {
        if (idleMovementStepTimer > 0) {
            character.NoMove();
            idleMovementStepTimer -= Time.deltaTime;
            return;
        }

        Vector3 directionToGoal = roamGoal - transform.position;
        float distanceToGoal = directionToGoal.magnitude;
        directionToGoal.y = 0;
        directionToGoal.Normalize();

        if (distanceToGoal > character.walkSpeed * 0.25f) {
            character.Walk(directionToGoal);
            isRoaming = true;
        } else {
            float angle = UnityEngine.Random.value * 360;
            float distance = UnityEngine.Random.value * idleRoamRadius;
            roamGoal = roamGoal + Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;
            idleMovementStepTimer = idleMovementStepDelay; // start step timer
            if (idleMovementResetTimer > 0 || idleMovementStepTimer > 0) // if there's a pause, interupt walking cycle
                character.NoMove();
            isRoaming = true;
        }
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
                //Gizmos.DrawWireSphere(originalPosition, roamDistance);
                Gizmos.DrawWireSphere(transform.position, roamDistance);
            }
        }
        switch (idlePattern) {
            case IdlePattern.Loop:
            case IdlePattern.Patrol:
                if (path.Count > 1) {
                    Gizmos.color = Color.white;
                    for (int i = 0; i < path.Count; ++i) {
                        Gizmos.DrawLine(path[i], path[(i + 1) % path.Count]);
                        Gizmos.DrawSphere(path[i], 0.1f);
                    }
                    if (Application.isEditor) {
                        Gizmos.DrawLine(path[0], transform.position);
                        Gizmos.DrawLine(path[path.Count - 1], transform.position);
                        Gizmos.DrawSphere(transform.position, 0.1f);
                    }
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(path[pathProgression], transform.position);
                    Gizmos.DrawSphere(path[pathProgression], 0.1f);
                }
                break;
            case IdlePattern.RoamCenter:
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(Application.isPlaying ? roamGoal : transform.position, transform.position);
                Gizmos.DrawSphere(path[pathProgression], 0.1f);
                Gizmos.DrawWireSphere(Application.isPlaying ? originalPosition : transform.position, idleRoamRadius);
                break;
            case IdlePattern.RoamFree:
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(roamGoal, transform.position);
                Gizmos.DrawSphere(path[pathProgression], 0.1f);
                Gizmos.DrawWireSphere(transform.position, idleRoamMaxDistance);
                break;
        }
    }

}


public enum IdlePattern {
    Stand, // just stand at spawn point
    Patrol, // patrol back and forth along a path
    Loop, // walk along a path, going back to the begining once the end is reached
    RoamCenter, // roam between random position in a circle around the spawn point
    RoamFree // roam in random directions with no limits
}

public enum AttackPattern {
    Rush,
    Salvo
}