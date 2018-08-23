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
	Vector3 stretchDirectionVec;
    Transform hand, shoulder, elbow, stretchingHand, realHand;

	public ArmStretchController (float degree, float totalTime)
    {
		stretchingJoints = GameObject.FindGameObjectsWithTag ("StretchingJoint");
		stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
		stretchDegree = degree;
		stretchTotalTime = totalTime;

        hand = GameObject.FindGameObjectWithTag("RightHand").transform;
        stretchingHand = GameObject.FindGameObjectWithTag("StretchingRightHand").transform;
        shoulder = GameObject.FindGameObjectWithTag ("RightShoulder").transform;
        elbow = GameObject.FindGameObjectWithTag("RightElbow").transform;
        realHand = GameObject.FindGameObjectWithTag("RealHand").transform;
        armLength = (hand.position - elbow.position).magnitude;
    }

    public void setStretchDegree(float degree)
    {
        stretchDegree = degree;
    }

    public void setStretchTotalTime(float totalTime)
    {
        stretchTotalTime = totalTime;
    }

    public void setStretchDirectionVec(Vector3 directionVec) {
        stretchDirectionVec = directionVec;
    }

    public void Stretch()
    {
		if (stretchCurrentTime < stretchTotalTime) {
            //ArmTranslate(deltaStretchLength());
            //ArmTranslate(stretchDirectionVec, deltaStretchLength());
            ArmTranslate(realHand.position-shoulder.position, deltaStretchLength());
            drawSphereAt(realHand.position);
            drawSphereAt(shoulder.position);
            stretchCurrentTime += frameDeltaTime;
		} else if (stretchCurrentTime >= stretchTotalTime) {
			stretchCurrentTime = stretchTotalTime;
		}
		//Debug.Log (stretchCurrentTime);
    }

	public void Shrink()
	{
		if (stretchCurrentTime > 0f) {
            //ArmTranslate(-deltaStretchLength());
            ArmTranslate(stretchingHand.position - elbow.position, -deltaStretchLength());
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

    public void ArmTranslate(Vector3 stretchDirection, float deltaStretchLength) {
        Vector3 stretchVector = (stretchDirection*10f).normalized * deltaStretchLength;
        foreach (GameObject stretchingJoint in stretchingJoints) {
            stretchingJoint.transform.Translate(stretchVector, Space.World);
        }
    }

    public void drawSphereAt(Vector3 pos) {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        sphere.transform.Translate(pos);
    }
}


