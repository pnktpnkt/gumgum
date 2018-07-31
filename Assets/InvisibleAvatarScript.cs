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

    private AvatarCalibrator avatarCalibrator;
    public Transform invisibleAvatar;
    public Transform shoulder_right;
    public Transform upperarm;
    public Transform forearm;
    private bool firstCalibration = true;
    private bool secondCalibration = false;

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
        avatarCalibrator = new AvatarCalibrator(invisibleAvatar, shoulder_right, upperarm, forearm);
        vrik.enabled = false;
        GameObject rightWrist = GameObject.FindGameObjectWithTag("RightHand");
        GameObject rightShoulder = GameObject.FindGameObjectWithTag("InvisibleRightShoulder");
        float armLength = (rightWrist.transform.position - rightShoulder.transform.position).sqrMagnitude;
        Debug.Log(armLength);
	}

	// Update is called once per frame
	void Update () {
        Vector3 headPos = headTarget.position;
        Vector3 handPos = rightHandTarget.position;
        headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		if (!leftController) {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
		} else {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.LTouch);
		}
        
        if (OVRInput.GetDown(OVRInput.RawButton.A)) {
            Debug.Log("A button is pushed");
            maxHeadToHandDist = headToHandDist;
            //Debug.Log(maxHeadToHandDist);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B)) {
            Debug.Log("B button is pushed");
            maxHeadTargetPosition = headTarget.position;
            //Debug.Log(maxHeadTargetPosition);
            GameObject rightWrist = GameObject.FindGameObjectWithTag("RightHand");
            GameObject rightShoulder = GameObject.FindGameObjectWithTag("InvisibleRightShoulder");
            float armLength = (rightWrist.transform.position - rightShoulder.transform.position).sqrMagnitude;
            Debug.Log(armLength);
            float realLength = (handPos - rightShoulder.transform.position).sqrMagnitude;
            Debug.Log(realLength);
            //Debug.Log(headTarget.position);
            //Debug.Log(rightHandTarget.position);
        }

       
        if (Input.GetKeyDown(KeyCode.C) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (firstCalibration) { // calibratioin for position and scale of avatar
                avatarCalibrator.calibrateAvatarPosition(headPos);
                avatarCalibrator.calibrateAvatarScale(headPos);
            }else if (!firstCalibration && !secondCalibration) {
                if (!vrik.enabled) {
                    vrik.enabled = true;
                }
                secondCalibration = true;
            }else if (secondCalibration) { // calibratioin for shoulder position and arm length of avatar
                avatarCalibrator.calibrateShoulderPosition(headPos, handPos);
                avatarCalibrator.calibrateArmLength(headPos, handPos);
            }
           
        }

        if (Input.GetKeyDown(KeyCode.V) || OVRInput.GetDown(OVRInput.RawButton.B)) {
            if (firstCalibration) {
                firstCalibration = false;
            }
            if (secondCalibration) {
                secondCalibration = false;
            }
        }
    }

	public Vector3 getHandAccel(){
		return handAccel;
	}

	public float getHeadToHandDist(){
		return headToHandDist;
	}
}

