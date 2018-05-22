using System;
using UnityEngine;

class RhandStretchController{

	private GameObject[] armjoints;

	private Transform stretchTarget;

	private Vector3[] originJointsPos;
	private Vector3[] prevJointsStretch;
	private Vector3 prevTargetPos = Vector3.zero;
	private Vector3 normStretchVec = Vector3.zero;

	private float amplitude = 0.0f;
	private float angularFreq = 0.0f;
	private float stretchDist = 0.0f;
	private float stretchVelocity = 0.0f;

	public RhandStretchController(GameObject[] armjoints, Transform stretchTarget){
		this.armjoints = (GameObject[])armjoints.Clone ();
		this.stretchTarget = stretchTarget;

		prevJointsStretch = new Vector3[this.armjoints.Length];
		for(int i=0; i<prevJointsStretch.Length; i++){
			prevJointsStretch[i] = Vector3.zero;
		}

		originJointsPos = new Vector3[this.armjoints.Length];
		for(int i = 0; i<originJointsPos.Length; i++){
			originJointsPos [i] = this.armjoints [i].transform.localPosition;
		}
	}

	public void SaveForwardVec(Vector3 TargetPos, Vector3 RShoulderPos){
		normStretchVec = (TargetPos - RShoulderPos).normalized;
	}

	public void GetAngularFreq(float mass, float constantK){
		angularFreq = Mathf.Sqrt (mass/constantK);
	}

	public void GetAmplitude(float mass, float accel, float constantK, float buffer){
		amplitude = mass * accel * buffer / constantK;
	}

	public float getStretchDist(){
		return stretchDist;
	}

	public float getStretchVelocity(){
		return stretchVelocity;
	}

	public Vector3 getForwardVec(){
		return normStretchVec;
	}

	public void UpdatePrevTargetPos(Vector3 TargetPos){
		prevTargetPos = TargetPos;
	}

	public void UpdatePrevJointsStretch(int i, Vector3 JointPos) {
		prevJointsStretch [i] = JointPos;
	}

	public void Stretch(float currentTime){
		Vector3 deltaX = normStretchVec * amplitude * Mathf.Cos (angularFreq*currentTime - Mathf.PI);     // because the lowest position of arm spring starts at the point of -PI. 
		Vector3 amplitudeVec = amplitude * normStretchVec;
		Vector3 stretchVec = amplitudeVec + deltaX;   

		for(int i=0; i<this.armjoints.Length; i++){
			this.armjoints [i].transform.Translate (stretchVec/armjoints.Length - prevJointsStretch[i], Space.World);    // each joint stretches the distance of 1/(numbers of joints).
			prevJointsStretch [i] = stretchVec / armjoints.Length;
			//Debug.Log (prevJointsStretch [i]);
			//if(i>=1)
			//	Debug.DrawLine (this.armjoints [i].transform.position, this.armjoints [i-1].transform.position, Color.green);
		}
		this.stretchTarget.Translate (stretchVec - prevTargetPos, Space.World);

		prevTargetPos = stretchVec;

		stretchDist = stretchVec.magnitude;
		stretchVelocity = -Mathf.Sin (angularFreq * currentTime - Mathf.PI);
	}

	public void DeletePrevRecord(){
		for(int i=0; i<this.armjoints.Length; i++){
			prevJointsStretch[i] = Vector3.zero;
		}
		prevTargetPos = Vector3.zero;

		for(int i=0; i<this.armjoints.Length; i++){
			this.armjoints [i].transform.localPosition = originJointsPos [i] ;
		}
		this.stretchTarget.localPosition = Vector3.zero;

		stretchDist = 0.0f;
		stretchVelocity = 0.0f;
		normStretchVec = Vector3.zero;
	}

}

