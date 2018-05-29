using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InVisibleAvatarScript : MonoBehaviour
{
	private ArmState rightArmState = ArmState.Idle;
	private ArmState leftArmState = ArmState.Idle;
	ArmStateReducer reducer = new ArmStateReducer();
	public RootMotion.FinalIK.VRIK vrik;
	private Transform headTarget;
	private Transform rightHandTarget;

	// Use this for initialization
	void Start () {
		headTarget = vrik.solver.spine.headTarget.transform;
		rightHandTarget = vrik.solver.rightArm.target.transform;
	}

	// Update is called once per frame
	void Update () {
		Vector3 handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
		float headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		rightArmState = reducer.getNewArmState(rightArmState, handAccel, headToHandDist);
	}

}

