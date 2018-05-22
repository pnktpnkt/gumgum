using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhandCollsionDetection : MonoBehaviour {

	public GameObject explosion;
	public GameObject gomuSecond;
	//public GameObject player;

	private GameObject tmpFireBall;

	private Vibrator vibrator;

	private bool isStretching = false;
	private bool isShrinking = false;
	private bool isHandColliding = false;
	private bool isOnFire = false;

	//private RhandStretchActivation playerScript;

	void Start(){
		vibrator = GetComponent<Vibrator> ();
	}

	void Update(){
		if (OVRInput.GetDown (OVRInput.RawButton.RIndexTrigger) && !isStretching && !isShrinking) {
			Vector3 fixedVector = gameObject.transform.position;
			fixedVector.z += 0.05f;
			tmpFireBall = Instantiate (gomuSecond, fixedVector, gameObject.transform.rotation);
			tmpFireBall.transform.parent = gameObject.transform;
			isOnFire = true;
		} else if (OVRInput.GetUp (OVRInput.RawButton.RIndexTrigger) && isOnFire) {
			if (tmpFireBall != null) {
				DestroyImmediate (tmpFireBall, true);
				isOnFire = false;
			}
		}	
			
		if(isOnFire && isHandColliding && isStretching){
			Destroy (Instantiate(explosion, transform.position, transform.rotation), 5);
			if (tmpFireBall != null) {
				DestroyImmediate (tmpFireBall, true);
			}
			isOnFire = false;
		}

		if(!isStretching && !isShrinking){
			isHandColliding = false;
		}
	}


	public void CheckStretchState(){
		isStretching = true;
	}

	public void CancelStretchState(){
		isStretching = false;
	}

	public void CheckShrinkState(){
		isShrinking = true;
	}

	public void CancelShrinkState(){
		isShrinking = false;
	}



	public bool GetStretchState(){
		return isStretching;
	}

	public bool GetShrinkState(){
		return isShrinking;
	}

	public bool GetPunchFire(){
		return isOnFire;
	}

	public bool GetCollisionState(){
		return isHandColliding;
	}

	void OnTriggerEnter(Collider collision){

		if(!isHandColliding && isStretching){
			if (collision.CompareTag ("Tortoise") && isStretching) {
				Transform rootTransform = collision.transform.root;
				rootTransform.gameObject.GetComponent<TortoiseController> ().GetHit ();
				//isHittingTurtle = true;
				isHandColliding = true;
				vibrator.playCollisionClip ();
			}
			if (collision.CompareTag ("Rino") && isStretching) {
				Transform rootTransform = collision.transform.root;
				rootTransform.gameObject.GetComponent<RinoController> ().GetHit ();
				isHandColliding = true;
				vibrator.playCollisionClip ();
			}
		}
	}
		
	void OnCollisionEnter(Collision collision){

		if (isStretching && !isHandColliding) {  //When hitting other objects like floor
			isHandColliding = true;
			vibrator.playCollisionClip ();
		}
	}
		
}
