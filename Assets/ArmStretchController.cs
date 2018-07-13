using System;
using UnityEngine;

// this class transforms the set stretchingObjects using set stretchVector
public class ArmStretchController
{
	// Targets to stretch
	private GameObject[] stretchingJoints;
	private GameObject stretchingRightHand;
    private float armLength;

    float stretchDegree = 5.0f; // this value is the number of avatar's forearm
    float stretchTotalTime = 1.0f; // this value is time for stretching avatar's arm
	float stretchCurrentTime = 0f;
    float frameDeltaTime = Time.deltaTime;
	Vector3 stretchVector;

	public ArmStretchController (float degree, float totalTime)
    {
		stretchingJoints = GameObject.FindGameObjectsWithTag ("StretchingJoint");
		stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
		stretchDegree = degree;
		stretchTotalTime = totalTime;

        Vector3 stretchTargetPos = GameObject.FindGameObjectWithTag("RightHand").transform.position;
        //Vector3 startPos = GameObject.FindGameObjectWithTag ("RightShoulder").transform.position;
        Vector3 stretchStartPos = GameObject.FindGameObjectWithTag("RightElbow").transform.position;
        armLength = (stretchTargetPos - stretchStartPos).magnitude;
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
            ArmTranslate(deltaStretchLength());
			stretchCurrentTime += frameDeltaTime;
		} else if (stretchCurrentTime >= stretchTotalTime) {
			stretchCurrentTime = stretchTotalTime;
		}
		//Debug.Log (stretchCurrentTime);
    }

	public void Shrink()
	{
		if (stretchCurrentTime > 0f) {
            ArmTranslate(-deltaStretchLength());
			stretchCurrentTime -= frameDeltaTime;
		} else if (stretchCurrentTime <= 0f) {
			stretchCurrentTime = 0f;
		}
		//Debug.Log (stretchCurrentTime);
	}

    public float deltaStretchLength() {
        return armLength*(stretchDegree-1.0f) / (stretchTotalTime/frameDeltaTime);
    }

    public void ArmTranslate(float deltaStretchLength) {
        foreach (GameObject stretchingJoint in stretchingJoints) {
            stretchingJoint.transform.Translate(new Vector3(0, deltaStretchLength, 0));
        }
    }
}


