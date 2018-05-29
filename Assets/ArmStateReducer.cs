using UnityEngine;
using System;

// must
public class ArmStateReducer
{
	public ArmStateReducer (){

	}

	public ArmState getNewArmState(ArmState previousState, Vector3 handAccel, float headToHandDist){
		return ArmState.Idle;
	}
}

