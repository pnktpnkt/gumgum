using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    public Transform avatar;
    private Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C) || OVRInput.GetDown(OVRInput.RawButton.A)) {
            Vector3 newPos = new Vector3(pos.x+avatar.position.x, pos.y, pos.z);
            this.transform.position = newPos;
        }

    }
}
