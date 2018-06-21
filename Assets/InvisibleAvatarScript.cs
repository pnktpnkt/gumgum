﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InvisibleAvatarScript : MonoBehaviour
{
	public RootMotion.FinalIK.VRIK vrik;
	private Transform headTarget;
	private Transform rightHandTarget;
	private Vector3 handAccel;
	private float headToHandDist;
	public bool leftController = false;

	// Use this for initialization
	void Start () {
		headTarget = vrik.solver.spine.headTarget.transform;
		rightHandTarget = vrik.solver.rightArm.target.transform;
		headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		if (!leftController) {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
		} else {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.LTouch);
		}
	}

	// Update is called once per frame
	void Update () {
		headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		if (!leftController) {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
		} else {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.LTouch);
		}
	}

	public Vector3 getHandAccel(){
		return handAccel;
	}

	public float getHeadToHandDist(){
		return headToHandDist;
	}
}

