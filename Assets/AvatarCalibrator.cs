using System;
using UnityEngine;

public class AvatarCalibrator
{
    Transform avatarTransform;
    Transform shoulder;
    Transform upperarm;
    Transform forearm;

    Vector3 headPosRef = new Vector3(-0.1f, 1.5f, -10.5f);
    Vector3 handPosRef = new Vector3(0.1f, 1.3f, -9.9f);
    Vector3 avatarScaleRef;

	public AvatarCalibrator(Transform avatarTransform, Transform shoulder, Transform upperarm, Transform forearm)
	{
        this.avatarTransform = avatarTransform;
        this.shoulder = shoulder;
        this.upperarm = upperarm;
        this.forearm = forearm;
        avatarScaleRef = avatarTransform.localScale;
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

        shoulder.position += new Vector3(widthDiff, 0, 0);
    }

    public void calibrateArmLength(Vector3 headPos, Vector3 handPos)
    {
        float armLength = handPos.z - headPos.z;
        float armLengthRef = handPosRef.z - headPosRef.z;
        float armLengthDiff = armLength - armLengthRef;

        Debug.Log(armLengthDiff);

        upperarm.Translate(new Vector3(0, armLengthDiff / 4, 0));
        forearm.Translate(new Vector3(0, armLengthDiff / 4, 0));
    }
}
