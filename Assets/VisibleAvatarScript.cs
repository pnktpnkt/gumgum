using UnityEngine;
using System.Collections;

public class VisibleAvatarScript : MonoBehaviour {
	private GameObject invisibleAvatar;
	private StretchEventEmitterDebug superStretchManVRIKScript;
	private ArmStretchController armStretchController;
	private InvisibleAvatarMotionImitator imitator;

	// Use this for initialization
	void Start () {
		invisibleAvatar = GameObject.FindGameObjectWithTag ("Player");
		superStretchManVRIKScript = invisibleAvatar.GetComponent<StretchEventEmitterDebug> ();

		//must
		armStretchController = new ArmStretchController ();

		imitator = new InvisibleAvatarMotionImitator ();
		imitator.setInvisibleAvatar (invisibleAvatar);
		imitator.setVisibleAvatar (GameObject.FindGameObjectWithTag ("VisibleAvatar"));
	}

	// Update is called once per frame
	void Update () {
		Debug.Log ("State : " + superStretchManVRIKScript.state);
		if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Idle) {
			imitator.Imitate ();
		} else {
			imitator.ImitateExceptHandPosition ();
		}

		if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Stretching) {
			armStretchController.Stretch ();
		} else if (superStretchManVRIKScript.state == StretchEventEmitterDebug.State.Shrinking) {
			armStretchController.Shrink ();
		}
	}
}