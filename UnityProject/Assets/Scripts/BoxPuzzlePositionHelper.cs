using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    //[RequireComponent(typeof(Rigidbody))]
    public class BoxPuzzlePositionHelper : MonoBehaviour {

        private Transform currentPiece = null;
        private float remainingDrag = 1;

        private float scale = 1;

        private void Start() {
            scale = Mathf.Max(0.01f,transform.localScale.magnitude);
        }

        private void OnTriggerEnter(Collider collider) {
            if (collider.GetComponent<BoxPuzzlePiece>() != null) {
                currentPiece = collider.transform;
                remainingDrag = 0;
            }
        }

        private void Update() {
            if (currentPiece != null) {
                remainingDrag += Time.deltaTime / scale;
                if(remainingDrag < 1) {
                    currentPiece.position = Vector3.Slerp(currentPiece.position, transform.position, remainingDrag);
                } else {
                    currentPiece.position = transform.position;
                    currentPiece = null;
                }
            }
        }
    }

}
