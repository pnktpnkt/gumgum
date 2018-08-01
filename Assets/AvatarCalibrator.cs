using System;
using UnityEngine;

public class AvatarCalibrator
{
    Transform avatarTransform;
    Transform shoulder;
    Transform upperarm;
    Transform forearm;
    Transform hand;

    Vector3 headPosRef = new Vector3(-0.1f, 1.5f, -10.5f);
    Vector3 handPosRef = new Vector3(0.1f, 1.3f, -9.9f);
    Vector3 handRotRef = new Vector3(330.6f, 352.7f, 88.8f);
    Vector3 avatarScaleRef;

    bool calibrateFlag = false;
    Vector3 preShoulderPos;
    Vector3 preUpperarmPos;
    Vector3 preForearmPos;

	public AvatarCalibrator(Transform avatarTransform, Transform shoulder, Transform upperarm, Transform forearm, Transform hand)
	{
        this.avatarTransform = avatarTransform;
        this.shoulder = shoulder;
        this.upperarm = upperarm;
        this.forearm = forearm;
        this.hand = hand;
        avatarScaleRef = avatarTransform.localScale;
        preShoulderPos = shoulder.position;
        preUpperarmPos = upperarm.position;
        preForearmPos = forearm.position;
	}

    public AvatarCalibrator(Transform hand) {
        this.hand = hand;
    }

    public void calibrateAvatarPosition(Vector3 headPos)
    {
        Vector3 newAvatarPos = new Vector3(headPos.x, 0, headPos.z);
        avatarTransform.position = newAvatarPos;
        //Debug.Log(avatarTransform.position);
    }

    public void calibrateAvatarScale(Vector3 headPos)
    {
        float scale = headPos.y / headPosRef.y;
        avatarTransform.localScale = new Vector3(avatarScaleRef.x, avatarScaleRef.y*scale, avatarScaleRef.z);
        //Debug.Log(avatarTransform.localScale);
    }

    public void calibrateShoulderPosition(Vector3 headPos, Vector3 handPos)
    {
        float width = handPos.x - headPos.x;
        float widthRef = handPosRef.x - handPosRef.x;
        float widthDiff = width - widthRef;
        float height = headPos.y - handPos.y;
        float heightRef = headPosRef.y - handPosRef.y;
        float heightDiff = height - heightRef;
        if (calibrateFlag) {
            shoulder.position = preShoulderPos;
        }
        Debug.Log("Shoulder Position Calibration : " + widthDiff + "," + heightDiff);
        shoulder.position += new Vector3(widthDiff, heightDiff, 0);
        calibrateFlag = true;
    }

    public void calibrateArmLength(Vector3 headPos, Vector3 handPos)
    {
        float armLength = handPos.z - headPos.z;
        float armLengthRef = handPosRef.z - headPosRef.z;
        float armLengthDiff = armLength - armLengthRef;
        if (calibrateFlag) {
            upperarm.position = preUpperarmPos;
            forearm.position = preForearmPos;
        }
        Debug.Log("Arm Length Calibratioin : " + armLengthDiff);
        upperarm.Translate(new Vector3(0, armLengthDiff / 4, 0));
        forearm.Translate(new Vector3(0, armLengthDiff / 4, 0));
        calibrateFlag = true;
    }

    public bool isArmParallel() {
        float x_min = handRotRef.x - 5f;
        float x_max = handRotRef.x + 5f;
        float y_min = handRotRef.y - 5f;
        float y_max = handRotRef.y + 5f;
        float x = hand.localEulerAngles.x;
        float y = hand.localEulerAngles.y;
        //Debug.Log(hand.localEulerAngles);
        if (x_min <= x && x <= x_max) {
            if(y_min <= y && y <= y_max) {
                //Debug.Log("calibration OK");
                return true;
            }
        }
        return false;
    }
}
