using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts {
    
    [RequireComponent(typeof(MeshRenderer))]
    public class BoxPuzzlePiece : MonoBehaviour {

        public BoxPuzzleGoalIndex goalIndex;
        
        private new MeshRenderer renderer;
        
        private Color targetColor;

        private void Start() {
            renderer = GetComponent<MeshRenderer>();
            renderer.material.color = targetColor = BoxPuzzlePositionGoal.BoxPuzzleGoalColor[(int)goalIndex];
        }

        public void LightOn() {
            targetColor = Color.white;
        }

        public void LightOff() {
            targetColor = BoxPuzzlePositionGoal.BoxPuzzleGoalColor[(int)goalIndex];
        }

        private void FixedUpdate() {
            renderer.material.color = Color.Lerp(renderer.material.color, targetColor, Time.fixedDeltaTime * 2);
        }

        private void OnDrawGizmos() {
            Gizmos.color = BoxPuzzlePositionGoal.BoxPuzzleGoalColor[(int)goalIndex];
            Gizmos.DrawWireSphere(transform.position, 1);
        }

    }
}
