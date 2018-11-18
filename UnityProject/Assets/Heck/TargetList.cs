using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TargetList : MonoBehaviour {

    public CombatTarget[] units = new CombatTarget[0];

    public CombatTarget GetClosestTarget(Vector3 position) {
        if (units.Length == 0) return null;
        try {
            return units.Where(x => x.IsAlive()).Select(x => new { target = x, distance = Vector3.Distance(x.transform.position, position) }).OrderByDescending(x => x.distance).First().target;
        } catch {

        }
        return null;
    }

    public CombatTarget GetClosestTarget(Vector3 position, float maxDistance) {
        if (units.Length == 0) return null;
        try {
            return units.Where(x => x.IsAlive()).Select(x => new { target = x, distance = Vector3.Distance(x.transform.position, position) }).Where(x => x.distance < maxDistance).OrderByDescending(x => x.distance).First().target;
        } catch {

        }
        return null;
    }

}