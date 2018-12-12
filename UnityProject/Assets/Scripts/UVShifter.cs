using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVShifter : MonoBehaviour {

    public Vector2 shiftSpeed;
    public string textureName = "PLZ SET!!";
    public float scaleFactor = 1;

    private Material material;
    private Vector2 offset;


    // Use this for initialization
    void Start() {
        try {
            material = GetComponent<MeshRenderer>().material;
            if (material == null)
                Destroy(this);
            else {
                material.SetTextureScale(textureName, new Vector2(transform.lossyScale.x * scaleFactor, transform.lossyScale.z * scaleFactor));
                offset = material.GetTextureOffset(textureName);
                foreach (string s in material.shaderKeywords) {
                    Debug.Log(s);
                }
                Debug.Log(material.ToString());
            }
        } catch {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        offset += shiftSpeed * Time.deltaTime;
        material.SetTextureOffset(textureName, offset);
    }
}
