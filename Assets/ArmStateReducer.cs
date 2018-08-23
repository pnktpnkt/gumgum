using UnityEngine;
using System;

// must
public class ArmStateReducer
{
	private float maxHandAccel = 0.0f;
	private float accelerationThreshold = 17.0f;
	private float headToHandDistThreshold = 0.20f;

	public ArmStateReducer (){

	}

	public ArmState getNewArmState(ArmState previousState, Vector3 handAccel, float headToHandDist){
		switch (previousState) {
		case ArmState.Idle:
			return getNewStateFromIdleState(handAccel, headToHandDist);
		case ArmState.Stretching:
			return getNewStateFromStretchingState(handAccel, headToHandDist);
		case ArmState.Shrinking:
			return getNewStateFromShrinkingState(handAccel, headToHandDist);
		default:
			return previousState;
		}
	}

	public ArmState getNewStateFromIdleState(Vector3 handAccel, float headToHandDist){
		updateMaxHandAccel (handAccel);
		// gumgum pistol starts when max acceleration is over threshold and user's arm stretches out
		if (headToHandDist > headToHandDistThreshold && maxHandAccel > accelerationThreshold) {
			//Debug.Log(maxHandAccel);
			maxHandAccel = 0.0f;
			return ArmState.Stretching;
		}
		return ArmState.Idle;
	}

	public ArmState getNewStateFromStretchingState(Vector3 handAccel, float headToHandDist){
		if (headToHandDist <= headToHandDistThreshold) {
			return ArmState.Shrinking;
		}
		return ArmState.Stretching;
	}

	public ArmState getNewStateFromShrinkingState(Vector3 handAccel, float headToHandDist){
		updateMaxHandAccel (handAccel);
		if (headToHandDist > headToHandDistThreshold && maxHandAccel > accelerationThreshold) {
			//Debug.Log (maxHandAccel);
			maxHandAccel = 0.0f;
			return ArmState.Stretching;
		}
		return ArmState.Shrinking;
	}

	private void updateMaxHandAccel(Vector3 handAccel){
		// get the max acceleration
		if (maxHandAccel < handAccel.magnitude) {
			maxHandAccel = handAccel.magnitude;
		}
	}
}

