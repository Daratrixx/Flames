using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {


    [RequireComponent(typeof(Light))]
    [RequireComponent(typeof(MeshRenderer))]
    public class BoxPuzzlePositionGoal : TriggerCondition {

        public BoxPuzzleGoalIndex goalIndex = BoxPuzzleGoalIndex.none;

        private BoxPuzzlePiece currentPiece = null;
        private new Light light;
        private new MeshRenderer renderer;

        private float targetIntensity;
        private Color targetColor;
        
        private bool TRIGGERED = false;

        public override bool IsTriggered() {
            return TRIGGERED;
        }

        private void Start() {
            light = GetComponent<Light>();
            targetColor = light.color = Color.white;
            targetIntensity = light.intensity = 0;
            renderer = GetComponent<MeshRenderer>();
            renderer.material.color = BoxPuzzleGoalColor[(int)goalIndex];
        }

        public void LightOn() {
            TRIGGERED = true;
            targetIntensity = 2;
            targetColor = BoxPuzzleGoalColor[(int)goalIndex];
        }

        public void LightOff() {
            TRIGGERED = false;
            targetIntensity = 0;
            targetColor = Color.white;
        }

        private void OnTriggerEnter(Collider other) {
            BoxPuzzlePiece piece = other.GetComponent<BoxPuzzlePiece>();
            if (piece != null) {
                if (currentPiece != null && currentPiece.goalIndex == goalIndex) currentPiece.LightOff();
                currentPiece = piece;
                if(piece.goalIndex == goalIndex) {
                    LightOn();
                    currentPiece.LightOn();
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (currentPiece!= null && other.transform == currentPiece.transform) {
                LightOff();
                currentPiece.LightOff();
                currentPiece = null;
            }
        }

        private void FixedUpdate() {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, Time.fixedDeltaTime * 2);
            light.color = Color.Lerp(light.color, targetColor, Time.fixedDeltaTime * 2);
        }

        public static Color[] BoxPuzzleGoalColor = { Color.white, Color.blue, Color.red, Color.yellow, Color.green, Color.cyan, Color.magenta };

        private void OnDrawGizmos() {
            Gizmos.color = BoxPuzzleGoalColor[(int)goalIndex];
            Gizmos.DrawWireCube(transform.position, new Vector3(2, 2, 2));
        }

    }


    public enum BoxPuzzleGoalIndex : int {
        none, blue, red, yellow, green, cyan, magenta
    }

}
