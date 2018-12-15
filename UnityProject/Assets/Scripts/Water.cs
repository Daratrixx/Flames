using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public UVShifterParameter currentParameter = new UVShifterParameter("_MainTex", 1, new Vector2(0.05f,0.05f));
    public UVShifterParameter noiseParameter = new UVShifterParameter("_DetailAlbedoMap", 5, new Vector2(-0.1f,-0.1f));

    private Material material;

    // Use this for initialization
    void Start() {
        try {
            material = GetComponent<MeshRenderer>().material;
            if (material == null)
                Destroy(this);
            else {
                currentParameter.SetScale(material, transform.lossyScale.x, transform.lossyScale.z);
                noiseParameter.SetScale(material, transform.lossyScale.x, transform.lossyScale.z);
            }
        } catch {
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update() {
        currentParameter.Shift(material);
        noiseParameter.Shift(material);
    }
}
