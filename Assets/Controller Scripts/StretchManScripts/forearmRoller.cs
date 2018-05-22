using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forearmRoller : MonoBehaviour {

    public GameObject wrist;
    private float initialRoll;
    private float prevRoll = 0f;

    private float lowerLim = 50.0f;
    private float upperLim = 290.0f;

	// Use this for initialization
	void Start () {
        initialRoll = this.gameObject.transform.localEulerAngles.y;
//        Debug.Log(wrist.transform.localEulerAngles.y);
	}
	
	// Update is called once per frame
	void Update () {

        float def = wrist.transform.localEulerAngles.y - prevRoll;
        
        if(def > 180f)
        {
            def -= 360f;
        }


        if (wrist.transform.hasChanged && Mathf.Abs(def) >= Mathf.Epsilon)
        {
            if (this.gameObject.transform.localEulerAngles.y + def > upperLim ||
                this.gameObject.transform.localEulerAngles.y + def < lowerLim)
            {
               // Debug.Log(def);
                this.gameObject.transform.localEulerAngles += Vector3.up * def;
                wrist.transform.localEulerAngles = new Vector3(wrist.transform.localEulerAngles.x, 0, wrist.transform.localEulerAngles.z);
            }

        }

    }
}
