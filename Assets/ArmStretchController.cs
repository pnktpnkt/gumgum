using System;
using UnityEngine;

public class ArmStretchController
{
	// Targets to stretch
	private GameObject[] stretchingJoints;
	private GameObject stretchingRightHand;

	// parameter for arm stretching
	int stretchCount;
	int stretchMaxCount;
	float stretchPercentage;

	public ArmStretchController ()
	{
		stretchingJoints = GameObject.FindGameObjectsWithTag ("StretchingJoint");
		stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
		stretchCount = 0;
		stretchMaxCount = 100;
		stretchPercentage = 0.01f;
	}

	public void Stretch()
	{
		if (stretchCount < stretchMaxCount) {
			ArmTranslate (GetStretchVector ());
			stretchCount++;
		}
		Debug.Log (stretchCount);
	}

	public void Shrink()
	{
		if (stretchCount > 0) {
			ArmTranslate (-GetStretchVector ());
			stretchCount--;
		}
		Debug.Log (stretchCount);
	}

	void ArmTranslate(Vector3 stretchVector)
	{
		foreach (GameObject stretchingJoint in stretchingJoints) {
			//Debug.Log (stretchingJoint.ToString ());
			stretchingJoint.transform.Translate (stretchVector, Space.World);
		}
		//Debug.Log (stretchingRightHand.ToString ());
		stretchingRightHand.transform.Translate (stretchVector, Space.World);
	}

	Vector3 GetStretchVector()
	{
		Vector3 stretchingTargetPos = GameObject.FindGameObjectWithTag ("StretchingRightHand").transform.position;
		//Vector3 startPos = GameObject.FindGameObjectWithTag ("RightShoulder").transform.position;
		Vector3 startPos = GameObject.FindGameObjectWithTag ("RightElbow").transform.position;
		return GetNormalizedStretchVector (stretchingTargetPos, startPos) * stretchPercentage;
	}

	Vector3 GetNormalizedStretchVector(Vector3 stretchingTargetPos, Vector3 startPos)
	{
		return (stretchingTargetPos - startPos).normalized;
	}
}


