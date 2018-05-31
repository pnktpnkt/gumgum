using UnityEngine;
using System.Collections;

public class VisibleAvatarScript : MonoBehaviour {
	private GameObject invisibleAvatar;
	private StretchEventEmitterDebug superStretchManVRIKScript;
	private InvisibleAvatarScript invisibleAvatarScript;
	private ArmStretchController armStretchController;
	private InvisibleAvatarMotionImitator imitator;
	private ArmState armState = ArmState.Idle;
	private ArmStateReducer armStateReducer;

	// Use this for initialization
	void Start () {
		invisibleAvatar = GameObject.FindGameObjectWithTag ("Player");
		superStretchManVRIKScript = invisibleAvatar.GetComponent<StretchEventEmitterDebug> ();
		invisibleAvatarScript = invisibleAvatar.GetComponent<InvisibleAvatarScript> ();

		//must
		armStretchController = new ArmStretchController ();

		// setup InvisibleAvatarMotionImitator
		GameObject visibleAvatar = GameObject.FindGameObjectWithTag ("VisibleAvatar");
		GameObject translatedParentObject = GameObject.FindGameObjectWithTag ("StretchingJoint");
		imitator = new InvisibleAvatarMotionImitator (invisibleAvatar, visibleAvatar, translatedParentObject);
		imitator.Imitate ();

		// setup ArmStateReducer
		armStateReducer = new ArmStateReducer();
	}

	// Update is called once per frame
	void Update () {
		// for ArmStateReducer
		Vector3 handAccel = invisibleAvatarScript.getHandAccel ();
		float headToHandDist = invisibleAvatarScript.getHeadToHandDist ();
		Debug.Log ("handAccel : " + handAccel);
		Debug.Log ("headToHandDist : " + headToHandDist);
		armState = armStateReducer.getNewArmState (armState, handAccel, headToHandDist);
		Debug.Log ("armState : " + armState);

		//if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Idle) {
		if (armState == ArmState.Idle) {
			imitator.Imitate ();
		} else {
			imitator.ImitateExceptHandPosition ();
		}

		//if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Stretching) {
		if (armState == ArmState.Stretching) {
			armStretchController.Stretch ();
		} else if (armState == ArmState.Shrinking){//superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Shrinking) {
			armStretchController.Shrink ();
		}
	}
}