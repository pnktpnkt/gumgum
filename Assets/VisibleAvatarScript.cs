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

    public GameObject invisibleAvatar;
    public float stretchDegree;
	public float stretchTotalTime;

	//private GameObject invisibleAvatar;
	private InvisibleAvatarScript invisibleAvatarScript;
	private ArmStretchController armStretchController;
	private InvisibleAvatarMotionImitator imitator;
	private ArmState armState = ArmState.Idle;
	private ArmState previousArmState = ArmState.Idle;
	private ArmStateReducer armStateReducer;
    private ControllerVelocityStorage cvs;
    private bool stretchFlag = false;
    private bool Flag = false;
    private int updateCount = 0;
    private int stretchingStateCount = 0;

	// Use this for initialization
	void Start () {
		//invisibleAvatar = GameObject.FindGameObjectWithTag ("Player");
		invisibleAvatarScript = invisibleAvatar.GetComponent<InvisibleAvatarScript> ();

		//must
		armStretchController = new ArmStretchController (stretchDegree, stretchTotalTime);

		// setup InvisibleAvatarMotionImitator
		GameObject visibleAvatar = GameObject.FindGameObjectWithTag ("VisibleAvatar");
		GameObject translatedParentObject = GameObject.FindGameObjectWithTag ("StretchingJoint");
        //GameObject translatedParentObject = GameObject.FindGameObjectWithTag("RightShoulder");
        GameObject attackingObject = GameObject.FindGameObjectWithTag("AttackingObject");
		imitator = new InvisibleAvatarMotionImitator (invisibleAvatar, visibleAvatar, translatedParentObject, attackingObject);
		imitator.Imitate ();

		// setup ArmStateReducer
		armStateReducer = new ArmStateReducer();

        cvs = new ControllerVelocityStorage();
	}

	// Update is called once per frame
	void Update () {
		// for ArmStateReducer
		Vector3 handAccel = invisibleAvatarScript.getHandAccel ();
        Vector3 handVelocity = invisibleAvatarScript.getHandVelocity();
        Vector3 handPos = invisibleAvatarScript.getHandPosition();
        cvs.add(handVelocity);
        //cvs.add(handPos);
        float headToHandDist = invisibleAvatarScript.getHeadToHandDist ();
		//Debug.Log ("handAccel : " + handAccel);
		//Debug.Log ("headToHandDist : " + headToHandDist);
		previousArmState = armState;
		armState = armStateReducer.getNewArmState (armState, handAccel, headToHandDist);
        //Debug.Log ("armState : " + armState);

        if (armState == ArmState.Idle) {
			imitator.Imitate ();
		} else {
			imitator.ImitateWhileArmStretching ();
        }

		if (armState == ArmState.Stretching) {
            //if (!stretchFlag) armStretchController.setStretchDirectionVec(cvs.getPunchDirection());
            stretchFlag = false;
            stretchingStateCount++;
            if (stretchingStateCount >= 1) {
                armStretchController.Stretch();
                //if (previousArmState == ArmState.Idle || previousArmState == ArmState.Shrinking) {
                if (stretchingStateCount == 1) { 
                    onStretchStart.Invoke(handAccel, handAccel);
                }
                stretchFlag = true;
            }		
		} else if (armState == ArmState.Shrinking) {
            //if(stretchFlag) armStretchController.setStretchDirectionVec(cvs.getPunchDirection());
                armStretchController.Shrink();
                if (previousArmState == ArmState.Stretching)
                    onShrinkStart.Invoke(handAccel, handAccel);
            //stretchFlag = false;
            stretchingStateCount = 0;
        }
	}
}