using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]public class ArmStretchEvent: UnityEvent<Vector3, Vector3>{

}

[System.Serializable]public class ArmShrinkEvent: UnityEvent<Vector3, Vector3>{

}

public class VisibleAvatarScript : MonoBehaviour {
	[SerializeField] ArmStretchEvent onStretchStart = new ArmStretchEvent();
	[SerializeField] ArmShrinkEvent onShrinkStart = new ArmShrinkEvent();

	public float stretchDegree;
	public float stretchTotalTime;

	private GameObject invisibleAvatar;
	private StretchEventEmitterDebug superStretchManVRIKScript;
	private InvisibleAvatarScript invisibleAvatarScript;
	private ArmStretchController armStretchController;
	private InvisibleAvatarMotionImitator imitator;
	private ArmState armState = ArmState.Idle;
	private ArmState previousArmState = ArmState.Idle;
	private ArmStateReducer armStateReducer;

	// Use this for initialization
	void Start () {
		invisibleAvatar = GameObject.FindGameObjectWithTag ("Player");
		superStretchManVRIKScript = invisibleAvatar.GetComponent<StretchEventEmitterDebug> ();
		invisibleAvatarScript = invisibleAvatar.GetComponent<InvisibleAvatarScript> ();

		//must
		armStretchController = new ArmStretchController (stretchDegree, stretchTotalTime);

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
		//Debug.Log ("handAccel : " + handAccel);
		//Debug.Log ("headToHandDist : " + headToHandDist);
		previousArmState = armState;
		armState = armStateReducer.getNewArmState (armState, handAccel, headToHandDist);
		//Debug.Log ("armState : " + armState);

		//if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Idle) {
		if (armState == ArmState.Idle) {
			imitator.Imitate ();
		} else {
			imitator.ImitateExceptHandPosition ();
		}

		//if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Stretching) {
		if (armState == ArmState.Stretching) {
			armStretchController.Stretch ();
			if(previousArmState == ArmState.Idle || previousArmState == ArmState.Shrinking)
				onStretchStart.Invoke (handAccel, handAccel);
		} else if (armState == ArmState.Shrinking){//superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Shrinking) {
			armStretchController.Shrink ();
			if(previousArmState == ArmState.Stretching)
				onShrinkStart.Invoke (handAccel, handAccel);
		}
	}
}