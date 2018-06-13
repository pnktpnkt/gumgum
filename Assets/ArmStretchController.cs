using System;
using UnityEngine;

// this class transforms the set stretchingObjects using set stretchVector
public class ArmStretchController
{
	// Targets to stretch
	private GameObject[] stretchingJoints;
	private GameObject stretchingRightHand;

	// parameter for arm stretching
	int stretchCount;
	int stretchMaxCount;
	float stretchPercentage;

    float stretchDegree = 5.0f; // this value is the number of avatar's arm
    float stretchTotalTime = 2.0f; // this value is time for stretching avatar's arm
	float stretchCurrentTime = 0f;
    float frameDeltaTime = Time.deltaTime;

	public ArmStretchController ()
	{
		stretchingJoints = GameObject.FindGameObjectsWithTag ("StretchingJoint");
		stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
		stretchCount = 0;
		stretchMaxCount = 100;
		stretchPercentage = 0.01f;
	}

    public void setStretchDegree(float degree)
    {
        stretchDegree = degree;
    }

    public void setStretchTotalTime(float totalTime)
    {
        stretchTotalTime = totalTime;
    }

    public void Stretch()
    {
		if (stretchCurrentTime < stretchTotalTime) {
			ArmTranslate (GetStretchVector ());
			stretchCurrentTime += frameDeltaTime;
		} else if (stretchCurrentTime >= stretchTotalTime) {
			stretchCurrentTime = stretchTotalTime;
		}
		//Debug.Log (stretchCurrentTime);
    }

	public void Shrink()
	{
		if (stretchCurrentTime > 0f) {
			ArmTranslate (-GetStretchVector ());
			stretchCurrentTime -= frameDeltaTime;
		} else if (stretchCurrentTime <= 0f) {
			stretchCurrentTime = 0f;
		}
		//Debug.Log (stretchCurrentTime);
	}

	public Vector3 GetStretchVector()
	{
		Vector3 stretchTargetPos = GameObject.FindGameObjectWithTag ("StretchingRightHand").transform.position;
		//Vector3 startPos = GameObject.FindGameObjectWithTag ("RightShoulder").transform.position;
		Vector3 stretchStartPos = GameObject.FindGameObjectWithTag ("RightElbow").transform.position;
		Vector3 armVector = stretchTargetPos - stretchStartPos;
		Vector3 stretchVector = armVector*(stretchDegree - 1.0f) / (stretchTotalTime/frameDeltaTime);
		return stretchVector;
	}

	public void ArmTranslate(Vector3 stretchVector)
	{
		foreach (GameObject stretchingJoint in stretchingJoints) {
			//Debug.Log (stretchingJoint.ToString ());
			stretchingJoint.transform.Translate (stretchVector, Space.World);
		}
		//Debug.Log (stretchingRightHand.ToString ());
		//stretchingRightHand.transform.Translate (stretchVector, Space.World);
	}
}


