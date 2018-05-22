using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperStretchManScript : MonoBehaviour {
	private GameObject player;
	private StretchEventEmitterDebug superStretchManVRIKScript;
	private ArmStretchController armStretchController;
	private PlayerMotionImitator playerMotionImitator;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		superStretchManVRIKScript = player.GetComponent<StretchEventEmitterDebug> ();
		armStretchController = new ArmStretchController ();
		playerMotionImitator = new PlayerMotionImitator ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Stete : " + superStretchManVRIKScript.state);
		if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Idle) {
			playerMotionImitator.Imitate ();
		} else {
			playerMotionImitator.ImitateExceptHandPosition ();
		}

		if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Stretching) {
			armStretchController.Stretch ();
		} else if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Shrinking) {
			armStretchController.Shrink ();
		}
	}
}
