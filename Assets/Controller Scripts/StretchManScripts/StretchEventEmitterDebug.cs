using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]public class StretchEvent: UnityEvent<Vector3, Vector3>{

//}

public class StretchEventEmitterDebug : MonoBehaviour {

	[SerializeField] StretchEvent onStretchStart = new StretchEvent();
	[SerializeField] UnityEvent onStretching = new UnityEvent();
	[SerializeField] UnityEvent onStretchEnd = new UnityEvent();
	[SerializeField] UnityEvent onShrinkStart = new UnityEvent();
	[SerializeField] UnityEvent onShrinking = new UnityEvent();
	[SerializeField] UnityEvent onShrinkEnd = new UnityEvent();
	[SerializeField] UnityEvent onCollide = new UnityEvent();

	public Transform stretchTarget;
	public Transform rightHandAnchor;
	public RootMotion.FinalIK.VRIK vrik;
	public GameObject rightWrist;

	private Transform headTarget;
	private Transform rightHandTarget;
	private Transform rightShoulder;

	private GameObject[] rightArmJoints;
	private RhandCollsionDetection collideDetectScript;
	private RhandStretchController rightHandStretcher;

	private float maxAccel = 0.0f;
	private float currentTime = 0.0f;
	private float prevStretchDist = 0.0f;

	public Transform tempParentOfTarget; 

	public enum State{
		Idle, 
		Stretching,
		StretchEnd,
		Shrinking,
	};

	public State state = State.Idle;
	private State nextState;

	void Start () {

		headTarget = vrik.solver.spine.headTarget.transform;
		rightHandTarget = vrik.solver.rightArm.target.transform;
		rightShoulder = vrik.references.rightShoulder.transform;

		rightArmJoints = GameObject.FindGameObjectsWithTag ("Joint");
		collideDetectScript = rightWrist.GetComponent<RhandCollsionDetection>();
		this.rightHandStretcher = new RhandStretchController (this.rightArmJoints, this.stretchTarget);
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("stretchTarget position : " + stretchTarget.position);
		//Debug.Log ("rightArmJoint : " + rightArmJoints [0].transform.localPosition);
		//Debug.Log (state);

		nextState = Reducer (state);

		if (state == nextState) {

			switch(state){
			case State.Idle:
				break;
			case State.Stretching:
				currentTime += Time.fixedDeltaTime;
				//this.rightHandStretcher.Stretch (currentTime);	
				onStretching.Invoke ();
				break;
			case State.StretchEnd:
				if (Input.GetKey (KeyCode.LeftArrow)) {
					Vector3 tempVector = stretchTarget.position;
					tempVector.x += -0.1f;
					stretchTarget.position = tempVector;
				}
				if (Input.GetKey (KeyCode.RightArrow)) {
					Vector3 tempVector = stretchTarget.position;
					tempVector.x += 0.1f;
					stretchTarget.position = tempVector;
				}
				//this.rightHandStretcher.UpdatePrevTargetPos (stretchTarget.position);
				//this.rightHandStretcher.UpdatePrevJointsStretch (0, rightArmJoints [0].transform.localPosition);
				//this.rightHandStretcher.SaveForwardVec (stretchTarget.position, rightShoulder.position);
				onStretchEnd.Invoke ();
				break;
			case State.Shrinking:
				currentTime -= Time.fixedDeltaTime;
				//this.rightHandStretcher.Stretch (currentTime);
				onShrinking.Invoke ();
				break;
			}

		}else{

			if (state == State.Idle && nextState == State.Stretching) {

				// Get the data for stretching simulation
				float handMass = 0.2f*maxAccel;    //0.2f is basic mass.        
				this.rightHandStretcher.GetAmplitude (handMass, maxAccel, 1, 0.4f);    //0.4f is accelBuffer   
				this.rightHandStretcher.GetAngularFreq (handMass, 1);// 1 is stiffness K
				this.rightHandStretcher.SaveForwardVec (stretchTarget.position, rightShoulder.position);

				/*tempParentOfTarget.position = stretchTarget.position;
				stretchTarget.parent = tempParentOfTarget;
				vrik.solver.rightArm.target = stretchTarget;     // VRIK target changes from the object of rightHandTarget to StretchingTarget 
				*/

				//Vector3 handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
				Vector3	handAccel = new Vector3 (5, 0, 0);	// Set the fixed values for debug
				onStretchStart.Invoke (stretchTarget.position, handAccel);

			} else if (state == State.Stretching && nextState == State.Shrinking || state == State.StretchEnd && nextState == State.Shrinking) {

				float headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
				float stretchingDist = this.rightHandStretcher.getStretchDist();

				if (stretchingDist < prevStretchDist) {     // In the max stretching distance case, CurrentTime needs to be fixed because stretching distance overpasses amplitude by 1 unit 				
					currentTime -= Time.fixedDeltaTime*2;
				}
				else{       // in the case of objecet collision and shrinking on the way, it needs to shrink once for the direction reverses
					currentTime -= Time.fixedDeltaTime;
					//this.rightHandStretcher.Stretch (currentTime);
					prevStretchDist = stretchingDist;
				}

				onShrinkStart.Invoke ();

			} else if (state == State.Shrinking && nextState == State.Idle) {
				RecoverParameters ();
			}

		}

		state = nextState;
	}

	State Reducer(State state){

		switch(state) {

		case State.Idle: 
			//Vector3 handAccel = OVRInput.GetLocalControllerAcceleration (OVRInput.Controller.RTouch);
			Vector3 handAccel = new Vector3 (0, 0, 0);

			if (Input.GetKey (KeyCode.UpArrow)) {
				handAccel = new Vector3 (5, 0, 0);	// Set the fixed values for debug
			}
			//float headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
			float headToHandDist = 0.40f;	// Set the fixed value for debug

			if (handAccel.magnitude >= 2) {      		        // Get the max acceleration
				if (maxAccel < handAccel.magnitude) {
					maxAccel = handAccel.magnitude;            
				}
			}

			if (headToHandDist > 0.30 && maxAccel > 4) {     // Gomugomu pistol starts when max acceleration is over 15 and usr's arm stretches out
				//Debug.Log(maxAccel);
				//Debug.Log (state);
				return State.Stretching;
			} 
			return State.Idle;

		case State.Stretching:
			stretchTarget.localRotation = rightHandTarget.rotation; 
			//headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
			headToHandDist = 0.40f;	// Set the fixed value for debug

			if (Input.GetKey (KeyCode.DownArrow)) {
				headToHandDist = 0f;
			}
			//float stretchingDist = this.rightHandStretcher.getStretchDist ();
			//Debug.Log (stretchingDist);
			/*if (stretchingDist >= prevStretchDist && !collideDetectScript.GetCollisionState()) {    // Stretching if the arm hasn't reached the maximum distance and it isn't blocked by another object
				//currentTime += Time.fixedDeltaTime;
				//this.rightHandStretcher.Stretch (currentTime);	
				prevStretchDist = stretchingDist;
			} else {
				onStretchEnd.Invoke ();
				return State.StretchEnd;
			}*/

			if (headToHandDist < 0.3) {  // Check whether user's hand has come back
				print(this.rightHandStretcher.getStretchVelocity());
				return State.Shrinking;
			}
			return State.Stretching;

		case State.StretchEnd:
			stretchTarget.localRotation = rightHandTarget.rotation; 
			//headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
			headToHandDist = 0.40f;	// Set the fixed value for debug
			if (Input.GetKey (KeyCode.DownArrow)) {
				headToHandDist = 0f;
				print (this.rightHandStretcher.getStretchVelocity ());
				return State.Shrinking;
			}

			if (headToHandDist < 0.3) {  // Check whether user's hand has come back
				print (this.rightHandStretcher.getStretchVelocity ());
				return State.Shrinking;
			}
			return State.StretchEnd;

		case State.Shrinking:
			stretchTarget.localRotation = rightHandTarget.rotation; 
			/*stretchingDist = this.rightHandStretcher.getStretchDist();
			if(stretchingDist <= prevStretchDist){      // Shrinking until it attempts to stretch again
				//currentTime -= Time.fixedDeltaTime;
				//this.rightHandStretcher.Stretch (currentTime);
				prevStretchDist = stretchingDist;
			}
			else{           
				onShrinkEnd.Invoke ();
				return State.Idle;
			}*/
			return State.Shrinking;

		default:
			return state;

		}
	}

	void RecoverParameters(){
		vrik.solver.rightArm.target = rightHandTarget;
		maxAccel = 0;
		currentTime = 0;
		prevStretchDist = 0;
		stretchTarget.parent = rightHandAnchor;
		this.rightHandStretcher.DeletePrevRecord ();
	}
}

