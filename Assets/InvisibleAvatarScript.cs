using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InvisibleAvatarScript : MonoBehaviour
{
	public RootMotion.FinalIK.VRIK vrik;
	private Transform headTarget;
	private Transform rightHandTarget;
	private Vector3 handAccel;
	private float headToHandDist;
    private float maxHeadToHandDist;
    private Vector3 maxHeadTargetPosition;
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
        
        if (OVRInput.GetDown(OVRInput.RawButton.A)) {
            Debug.Log("A button is pushed");
            maxHeadToHandDist = headToHandDist;
            Debug.Log(maxHeadToHandDist);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B)) {
            Debug.Log("B button is pushed");
            maxHeadTargetPosition = headTarget.position;
            Debug.Log(maxHeadTargetPosition);
        }
        
    }

	public Vector3 getHandAccel(){
		return handAccel;
	}

	public float getHeadToHandDist(){
		return headToHandDist;
	}
}

