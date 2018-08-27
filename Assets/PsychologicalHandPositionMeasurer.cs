using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PsychologicalHandPositionMeasurer : MonoBehaviour {
    public Transform rightHandTarget;
    public Transform leftHandTarget;
    bool leftElbowFlag;
    private Vector3 leftElbowPos;
    private MyLogger logger;
    public int playerIndex = 0;
    private int logCount = 0;

    // Use this for initialization
    void Start() {
        leftElbowFlag = false;
        leftElbowPos = Vector3.zero;
        logger = new MyLogger(playerIndex);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { // When pushing 'Space', undo deciding elbow postion.
            leftElbowFlag = false;
            logger.log("------UNDO------");
        } else if (!leftElbowFlag) {
            if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.Y)) {
                leftElbowPos = leftHandTarget.position;
                leftElbowFlag = true;
            }
        } else {
            if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.Y)) {
                Vector3 realRightHandPos = rightHandTarget.position;
                double realAngle = getAngle(leftElbowPos, realRightHandPos);
                logger.log("RealAngle" + logCount + " : " + realAngle);
                Vector3 leftHandPos = leftHandTarget.position;
                double virtualAngle = getAngle(leftElbowPos, leftHandPos);
                logger.log("VirtualAngle" + logCount + " : " + virtualAngle);
                logCount++;
                Debug.Log(realAngle);
                Debug.Log(virtualAngle);
            }
        }
        
    }

    private double getAngle(Vector3 pos1, Vector3 pos2) {
        double radian;
        radian = Math.Atan(Math.Abs(pos2.z - pos1.z) / Math.Abs(pos2.x - pos1.x));
        double angle = (radian * 180 / Math.PI); 
        return angle;
    }
}

