using System;
using UnityEngine;

public class AvatarCalibrator
{
    Transform avatarTransform;
    Transform shoulder;
    Transform upperarm;
    Transform forearm;
    Transform hand;
    Transform touchTransform;

    Vector3 headPosRef = new Vector3(-0.1f, 1.5f, -10.5f);
    Vector3 handPosRef = new Vector3(0.1f, 1.3f, -9.9f);
    Vector3 handRotRef = new Vector3(322.7f, 349f, 85.9f);
    Vector3 avatarScaleRef;

    bool calibrateFlag = false;
    Vector3 preShoulderPos;
    Vector3 preUpperarmPos;
    Vector3 preForearmPos;

	public AvatarCalibrator(Transform avatarTransform, Transform shoulder, Transform upperarm, Transform forearm, Transform hand, Transform touchTransform)
	{
        this.avatarTransform = avatarTransform;
        this.shoulder = shoulder;
        this.upperarm = upperarm;
        this.forearm = forearm;
        this.hand = hand;
        this.touchTransform = touchTransform;
        avatarScaleRef = avatarTransform.localScale;
        preShoulderPos = shoulder.localPosition;
        preUpperarmPos = upperarm.position;
        preForearmPos = forearm.position;
	}

    public AvatarCalibrator(Transform touchTransform) {
        this.touchTransform = touchTransform;
    }

    public void calibrateAvatarPosition(Vector3 headPos)
    {
        Vector3 newAvatarPos = new Vector3(headPos.x, 0, headPos.z);
        avatarTransform.position = newAvatarPos;
        Debug.Log(avatarTransform.position);
    }

    public void calibrateAvatarScale(Vector3 headPos)
    {
        float scale = headPos.y / headPosRef.y;
        avatarTransform.localScale = new Vector3(avatarScaleRef.x, avatarScaleRef.y*scale, avatarScaleRef.z);
        Debug.Log(avatarTransform.localScale);
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
            //shoulder.localPosition = preShoulderPos;
        }
        //Debug.Log("Shoulder Position Calibration : " + widthDiff + "," + heightDiff);
        Debug.Log("Shoulder Position Calibration : " + (handPos.x-shoulder.position.x) + "," + (handPos.y-shoulder.position.y));
        //shoulder.position += new Vector3(widthDiff, heightDiff, 0);
        //forearm.position = new Vector3(handPos.x, handPos.y, forearm.position.z);
        //shoulder.position = new Vector3(hand.position.x, hand.position.y, shoulder.position.z);
        //forearm.position = new Vector3(hand.position.x, hand.position.y, forearm.position.z);
        //upperarm.position = new Vector3(hand.position.x, hand.position.y, upperarm.position.z);
        shoulder.position = new Vector3(handPos.x, handPos.y, shoulder.position.z);
        
        //upperarm.position = new Vector3(forearm.position.x, forearm.position.y, upperarm.position.z);
        //shoulder.position = new Vector3(touchTransform.position.x, touchTransform.position.y, shoulder.position.z);
        calibrateFlag = true;
    }

    public void calibrateShoulderPosition2(Vector3 targetPos, Vector3 handPos) {
        float x_s, y_s, z_s;
        z_s = shoulder.position.z;
        float a = Math.Abs(targetPos.z - z_s) / Math.Abs(targetPos.z - handPos.z);
        x_s = targetPos.x + (handPos.x - targetPos.x) * a;
        y_s = targetPos.y + (handPos.y - targetPos.y) * a;
        shoulder.position = new Vector3(x_s, y_s, z_s);

        calibrateFlag = true;
    }

    public void calibrateArmLength(Vector3 headPos, Vector3 handPos)
    {
        float armLength = handPos.z - headPos.z;
        float armLengthRef = handPosRef.z - headPosRef.z;
        float armLengthDiff = armLength - armLengthRef;
        if (calibrateFlag) {
            //upperarm.position = preUpperarmPos;
            //forearm.position = preForearmPos;
        }
        Debug.Log("Arm Length Calibratioin : " + armLengthDiff);
        upperarm.Translate(new Vector3(0, armLengthDiff / 4, 0));
        forearm.Translate(new Vector3(0, armLengthDiff / 4, 0));
        calibrateFlag = true;
    }

    public bool isArmParallel() {
        float thr_x = 5f, thr_y = 5f;
        float x_min = getAvailableAngle(handRotRef.x - thr_x);
        float x_max = getAvailableAngle(handRotRef.x + thr_x);
        float y_min = getAvailableAngle(handRotRef.y - thr_y);
        float y_max = getAvailableAngle(handRotRef.y + thr_y);
        float x = getAvailableAngle(touchTransform.localEulerAngles.x);
        float y = getAvailableAngle(touchTransform.localEulerAngles.y);
        //Debug.Log(touchTransform.localEulerAngles);
        //Debug.Log(x_min + "," + x_max + "," + y_min + "," + y_max);
        //Debug.Log(x + "," + y);
        if (x_min <= x && x <= x_max) {
            if(y_min <= y && y <= y_max) {
                //Debug.Log("calibration OK");
                return true;
            }
        }
        return false;
    }

    public float getAvailableAngle(float angle) {
        float ret;
        if(angle <= 180f) {
            ret = angle;
        }else{
            ret = angle - 360f;
        }
        return ret;
    }
}
