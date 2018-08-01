using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibrationTextScript : MonoBehaviour {
    public Transform hand_right;
    private AvatarCalibrator ac;
	// Use this for initialization
	void Start () {
        ac = new AvatarCalibrator(hand_right);
	}
	
	// Update is called once per frame
	void Update () {
        if (ac.isArmParallel()) {
            this.GetComponent<TextMesh>().text = "OK";
        }
        this.GetComponent<TextMesh>().text = "NG";
	}
}
