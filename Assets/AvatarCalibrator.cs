using System;
using UnityEngine;

public class AvatarCalibrator
{
    Transform avatarTransform;
    Transform shoulder;
    Transform upperarm;
    Transform forearm;
    Vector3 preAvatarScale;

    Vector3 headPosRef;
    Vector3 handPosRef;

	public AvatarCalibrator(Transform avatarTransform, Transform shoulder, Transform upperarm, Transform forearm)
	{
        this.avatarTransform = avatarTransform;
        this.shoulder = shoulder;
        this.upperarm = upperarm;
        this.forearm = forearm;
        Vector3 preInvisibleAvatarScale = avatarTransform.localScale;
	}

    public void calibrateAvatarPosition(Vector3 headPos)
    {
        Vector3 newAvatarPos = new Vector3(headPos.x, 0, headPos.z);
        avatarTransform.position = newAvatarPos;
    }

    public void calibrateAvatarScale(Vector3 headPos)
    {
        float scale = headPos.y / headPosRef.y;
        avatarTransform.localScale = new Vector3(1f, scale, 1f);
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

        upperarm.Translate(new Vector3(0, armLengthDiff / 2, 0));
        forearm.Translate(new Vector3(0, armLengthDiff / 2, 0));
    }
}
