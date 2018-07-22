﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour {
    private GameObject[] stretchingJoints;
    private Vector3 localVector = new Vector3(-0.2f, 0.2f, 0f);
    private Vector3 tempVector;
    private Quaternion tempRotation;
    bool attackStart = false;
    // Use this for initialization
    void Start () {
        stretchingJoints = GameObject.FindGameObjectsWithTag("StretchingJoint");
        this.transform.localPosition = localVector;
        tempVector = localVector;
        //tempRotation = this.transform.localRotation;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition = localVector;
        if (Input.GetKeyDown(KeyCode.S) || OVRInput.GetDown(OVRInput.RawButton.A)) attackStart = true;
        if(attackStart) attack();
        if (Input.GetKeyDown(KeyCode.D) || OVRInput.GetDown(OVRInput.RawButton.B)) {
            this.transform.localPosition = tempVector;
            localVector = tempVector;
            //this.transform.localRotation = tempRotation;
            attackStart = false;
        }
	}

    void attack() {
        //Quaternion rot = Quaternion.AngleAxis(-90f * Time.deltaTime, Vector3.up);
        //transform.localRotation = rot * transform.localRotation;
        if(localVector.x < 0.2f) {
            localVector = localVector + new Vector3(1f * Time.deltaTime, 0, 0);
        }
    }
}