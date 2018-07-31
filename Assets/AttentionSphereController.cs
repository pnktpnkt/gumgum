using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionSphereController : MonoBehaviour {
    private Vector3 pos;
    public Transform invisibleAvatar;

	// Use this for initialization
	void Start () {
        MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
        meshrenderer.enabled = false;
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V) || OVRInput.GetDown(OVRInput.RawButton.B)) {
            pos = new Vector3(invisibleAvatar.position.x, invisibleAvatar.position.y, pos.z);
            MeshRenderer meshrenderer = GetComponent<MeshRenderer>();
            meshrenderer.enabled = true;
        }
    }
}
