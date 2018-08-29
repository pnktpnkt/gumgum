using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class InvisibleAvatarScript : MonoBehaviour
{
	public RootMotion.FinalIK.VRIK vrik;
	private Transform headTarget;
	private Transform rightHandTarget;
	private Vector3 handAccel;
    private Vector3 handVelocity;
    private Vector3 handPos;
    private float headToHandDist;
    private float maxHeadToHandDist;
    private Vector3 maxHeadTargetPosition;
	public bool leftController = false;

    private AvatarCalibrator avatarCalibrator;
    public Transform invisibleAvatar;
    public GameObject visibleAvatar;
    public Transform shoulder_right;
    public Transform upperarm;
    public Transform forearm;
    public Transform hand;
    public Transform touch_right;
    private bool firstCalibration = true;
    private bool secondCalibration = false;

    public Transform target;

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
        avatarCalibrator = new AvatarCalibrator(invisibleAvatar, shoulder_right, upperarm, forearm, hand, touch_right);
        vrik.enabled = false;
        GameObject rightWrist = GameObject.FindGameObjectWithTag("RightHand");
        GameObject rightShoulder = GameObject.FindGameObjectWithTag("InvisibleRightShoulder");
        float armLength = (rightWrist.transform.position - rightShoulder.transform.position).sqrMagnitude;
        Debug.Log(armLength);
	}

	// Update is called once per frame
	void Update () {
        Vector3 headPos = headTarget.position;
        handPos = rightHandTarget.position;
        headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		if (!leftController) {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
            handVelocity = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        } else {
			handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.LTouch);
            handVelocity = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.LTouch);
        }
        
        if (OVRInput.GetDown(OVRInput.RawButton.A)) {
            Debug.Log("A button is pushed");
            Debug.Log(touch_right.localEulerAngles);
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
                visibleAvatar.SetActive(true);
                avatarCalibrator.calibrateAvatarPosition(headPos);
                avatarCalibrator.calibrateAvatarScale(headPos);
                Debug.Log("First Calibration");
            } else if (!firstCalibration && !secondCalibration) {
                if (!vrik.enabled) {
                    vrik.enabled = true;
                    Debug.Log("VRIK ON");
                }
                secondCalibration = true;
            }else if (secondCalibration && avatarCalibrator.isArmParallel()) { // calibratioin for shoulder position and arm length of avatar
                avatarCalibrator.calibrateShoulderPosition(headPos, handPos);
                //avatarCalibrator.calibrateArmLength(headPos, handPos);
                Debug.Log("Second Calibration");
            }else if (secondCalibration) {
                //avatarCalibrator.calibrateShoulderPosition2(target.position, handPos);
                avatarCalibrator.calibrateShoulderPosition2(target.position, hand.position);
                Debug.Log("Second Calibration2");
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

    public Vector3 getHandVelocity() {
        return handVelocity;
    }

    public Vector3 getHandPosition() {
        return handPos;
    }

    public float getHeadToHandDist(){
		return headToHandDist;
	}
}

