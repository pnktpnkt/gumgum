using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class PsychologicalHandPositionMeasurer : MonoBehaviour {
    public Transform leftHandTarget;
    bool leftElbowFlag;
    private Vector3 leftElbowPos;
    private Vector3 leftHandPos;
    private MyLogger logger;
    public int playerIndex = 0;

    // Use this for initialization
    void Start() {
        leftElbowFlag = false;
        leftElbowPos = Vector3.zero;
        leftHandPos = Vector3.zero;
        logger = new MyLogger(playerIndex);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) { // When pushing 'Space', undo deciding elbow postion.
            leftElbowFlag = false;
        } else if (!leftElbowFlag) {
            if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.Y)) {
                leftElbowPos = leftHandTarget.position;
                leftElbowFlag = true;
            }
        } else {
            if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.Y)) {
                leftHandPos = leftHandTarget.position;
                double leftElbowAngle = getAngle(leftElbowPos, leftHandPos);
                Debug.Log(leftElbowAngle);
            }
        }
        
    }

    private double getAngle(Vector3 pos1, Vector3 pos2) {
        double angle;
        angle = Math.Atan((pos2.z - pos1.z) / (pos2.x - pos1.x));
        return angle;
    }
}

