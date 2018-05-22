using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPlayerMotionScript : MonoBehaviour {
	GameObject player;
	GameObject stretchingRightHand;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
	}
	
	// Update is called once per frame
	void Update () {
		Transform[] thisChildTransforms = this.GetComponentsInChildren<Transform> ();
		Transform[] playerChildTransforms = player.GetComponentsInChildren<Transform> ();
		int count = 0;
		bool isPositionCopied = true;
		foreach (Transform child in thisChildTransforms) {
			if (MatchesStretchingRightHandName (child.name))
				isPositionCopied = false;
			if (isPositionCopied)
				child.position = playerChildTransforms[count].position;
			child.rotation = playerChildTransforms [count].rotation;
			count++;
		}
	}

	bool MatchesStretchingRightHandName(string childName){
		string stretchingRightHandName = stretchingRightHand.transform.name;
		if (childName.Equals (stretchingRightHandName)) {
				return true;
		}
		return false;
	}
}
