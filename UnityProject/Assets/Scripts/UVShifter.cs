using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVShifter : MonoBehaviour {

    public UVShifterParameter[] parameters = new UVShifterParameter[0];

    public Vector2 shiftSpeed;
    public string textureName = "PLZ SET!!";
    public float scaleFactor = 1;

    private Material material;

    // Use this for initialization
    void Start() {
        try {
            material = GetComponent<MeshRenderer>().material;
            if (material == null)
                Destroy(this);
            else {
                foreach (var p in parameters) {
                   p.SetScale(material, transform.lossyScale.x, transform.lossyScale.z);
                }
            }
        } catch {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        foreach (var p in parameters) {
            p.Shift(material);
        }
    }
}

[System.Serializable]
public class UVShifterParameter {

    public UVShifterParameter(string name, float scale, Vector2 speed) {
        textureName = name;
        scaleFactor = scale;
        shiftSpeed = speed;
    }

    public string textureName = "PLZ SET!!";
    public float scaleFactor = 1;
    public Vector2 shiftSpeed;

    public void SetScale(Material m, float w, float h) {
        m.SetTextureScale(textureName, new Vector2(w * scaleFactor, h * scaleFactor));
    }

    public void Shift(Material m) {
        m.SetTextureOffset(textureName, shiftSpeed * Time.time);
    }

}
