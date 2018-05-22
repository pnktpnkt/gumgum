using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhandStretchActivation : MonoBehaviour {

	public GameObject rightWrist;
//	public GameObject TutorialCanvas;

	public Transform rightShoulder;
	public Transform stretchTarget;
	public Transform headTarget;
	public Transform rightHandTarget;
	public Transform tempParentOfTarget; 
	public Transform rightHandAnchor;

	public float basicMass = 0.2f;
	public float stiffnessK = 1;
	public float accelBuffer = 0.4f;

	//private CanvasGroup canvasManager;
	private GameObject[] rightArmJoints;
	[SerializeField]
	private RhandCollsionDetection collideDetectScript;

	//private AudioSource stretchingClip;
	//private AudioSource shrinkingClip;

	private Vector3[] originArmJointLocalPos;

	private RootMotion.FinalIK.VRIK vrik;

	private RhandStretchController rightHandStretcher;

	private float maxAccel = 0.0f;
	private float currentTime = 0.0f;
	private float prevStretchDist = 0.0f;

	private bool isDebugMode = false;

	public enum State{
		Idle, 
		Stretching, 
		Shrinking,
	};

	public State state = State.Idle;
	private State nextState = State.Idle;

	void Start () {
		///canvasManager = TutorialCanvas.GetComponent<CanvasGroup> ();
		vrik = gameObject.GetComponent<RootMotion.FinalIK.VRIK> ();
		rightArmJoints = GameObject.FindGameObjectsWithTag ("Joint");
		collideDetectScript = rightWrist.GetComponent<RhandCollsionDetection>();
		this.rightHandStretcher = new RhandStretchController (this.rightArmJoints, this.stretchTarget);
		//AudioInitialization ();
	}


	void Update () {
		stretchTarget.localRotation = rightHandTarget.rotation; 

		ModeSelecting ();

		switch(state){
		case State.Idle:
			UserMotionCheck ();
			break;
		case State.Stretching:
			OnStretching ();
			break;
		case State.Shrinking:
			OnShrinking ();
			break;
		}

		if(state != nextState){
			state = nextState;
			switch(state){
			case State.Idle:
				IdleStart ();
				break;
			case State.Stretching:
				OnStretchingStart ();
				break;
			case State.Shrinking:
				OnShrinkingStart ();
				break;
			}
		}
	}

/*	IEnumerator Fade(){
		for(float f=1f; f>=0; f-=0.01f){
			canvasManager.alpha = f;
			yield return null;
		}
	}*/

	void ModeSelecting(){
		if (Input.GetKeyDown (KeyCode.Return)) {
			isDebugMode = !isDebugMode;
			if(isDebugMode){
				Debug.Log ("Debug Mode activates!");
			}
			else{
				Debug.Log ("Debug Mode cancelled");
			}
		}

	/*	if (OVRInput.GetDown (OVRInput.RawButton.RIndexTrigger)) {
			StartCoroutine ("Fade");
			Debug.Log (canvasManager.alpha);
		}*/
	}

/*	void AudioInitialization(){
		AudioSource[] stretchAudios = rightWrist.GetComponents<AudioSource> ();
		stretchingClip = stretchAudios [0];
		shrinkingClip = stretchAudios [1];
	}*/

	void ChangeState(State nextState){
		this.nextState = nextState;
	}

	void UserMotionCheck(){
		Vector3 handAccel = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
		float headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;

		if(handAccel.magnitude >= 8){      		        // Get the max acceleration
			if (maxAccel < handAccel.magnitude) {
				maxAccel = handAccel.magnitude;            
			}
		}

		if(headToHandDist > 0.35 && maxAccel > 15){     // Gomugomu pistol starts when max acceleration is over 15 and usr's arm stretches out
			//shrinkingClip.Stop ();
//			stretchingClip.Play (); sound functionalities was separated to StretchManSound class
			ChangeState (State.Stretching);
		}
	}

	public void OnStretchingStart(){
		// Get the data for stretching simulation
		float handMass = basicMass*maxAccel;            
		this.rightHandStretcher.GetAmplitude (handMass, maxAccel, stiffnessK, accelBuffer);       
		this.rightHandStretcher.GetAngularFreq (handMass, stiffnessK);
		this.rightHandStretcher.SaveForwardVec (stretchTarget.position, rightShoulder.position);

		tempParentOfTarget.position = stretchTarget.position;
		stretchTarget.parent = tempParentOfTarget;
		vrik.solver.rightArm.target = stretchTarget;     // VRIK target changes from the object of rightHandTarget to StretchingTarget 
	}

	public void OnStretching(){
		float headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		float stretchingDist = this.rightHandStretcher.getStretchDist();
	
		if (stretchingDist >= prevStretchDist && !collideDetectScript.GetCollisionState()) {    // Stretching if the arm hasn't reached the maximum distance and it isn't blocked by another object
			currentTime += Time.fixedDeltaTime;
			this.rightHandStretcher.Stretch (currentTime);	
			prevStretchDist = stretchingDist;
		} 
		else if(collideDetectScript.GetCollisionState()){
			//stretchingClip.Stop ();
		}
			
		if (headToHandDist < 0.3) {  // Check whether user's hand has come back
//			stretchingClip.Stop ();
//			shrinkingClip.Play (); separated to StretchManSound class
			ChangeState (State.Shrinking);
		}
	}

	void OnShrinkingStart(){
		float headToHandDist = (rightHandTarget.position - headTarget.position).sqrMagnitude;
		float stretchingDist = this.rightHandStretcher.getStretchDist();

		if (stretchingDist < prevStretchDist) {     // In the max stretching distance case, CurrentTime needs to be fixed because stretching distance overpasses amplitude by 1 unit 				
			currentTime -= Time.fixedDeltaTime*2;
		}
		else{       // in the case of objecet collision and shrinking on the way, it needs to shrink once for the direction reverses
			currentTime -= Time.fixedDeltaTime;
			this.rightHandStretcher.Stretch (currentTime);
			prevStretchDist = stretchingDist;
		}
	}

	void OnShrinking(){
		float stretchingDist = this.rightHandStretcher.getStretchDist();
		if(stretchingDist <= prevStretchDist){      // Shrinking until it attempts to stretch again
			currentTime -= Time.fixedDeltaTime;
			this.rightHandStretcher.Stretch (currentTime);
			prevStretchDist = stretchingDist;
		}
		else{                                      
			ChangeState(State.Idle);
		}
	}
		
	void IdleStart(){
		RecoverParameters ();
	}

	// Recover all parameters when shrinking comes to end
	void RecoverParameters(){
		vrik.solver.rightArm.target = rightHandTarget;
		maxAccel = 0;
		currentTime = 0;
		prevStretchDist = 0;
		stretchTarget.parent = rightHandAnchor;
		this.rightHandStretcher.DeletePrevRecord ();
	}
}
