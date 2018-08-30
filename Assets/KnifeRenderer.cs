using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeRenderer : MonoBehaviour {
    private bool isRendered = false;

	// Use this for initialization
	void Start () {
        MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
        meshrenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            if (!isRendered) {
                MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
                meshrenderer.enabled = true;
            }else if (isRendered) {
                MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
                meshrenderer.enabled = false;
            }
        }
	}
}
