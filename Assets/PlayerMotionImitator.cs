using System;
using UnityEngine;

public class PlayerMotionImitator
{
	GameObject player;
	GameObject imitator;
	GameObject stretchingRightHand;
	GameObject stretchingJoint;

	public PlayerMotionImitator ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		imitator = GameObject.FindGameObjectWithTag("Imitator");
		//stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
		stretchingJoint = GameObject.FindGameObjectWithTag ("StretchingJoint");
	}

	public void Imitate()
	{
		CopyObjectPositions (false);
	}

	public void ImitateExceptHandPosition()
	{
		CopyObjectPositions (true);
	}

	void CopyObjectPositions(bool isArmTranslated)
	{
		Transform[] playerChildTransforms = player.GetComponentsInChildren<Transform> ();
		Transform[] imitatorChildTransforms = imitator.GetComponentsInChildren<Transform> ();
		var count = 0;
		bool isPositionCopied = true;

		foreach (Transform child in imitatorChildTransforms) {
			if (isArmTranslated && MatchesTranslatedObjectName (child.name))
				isPositionCopied = false;
			if (isPositionCopied)
				child.position = playerChildTransforms[count].position;
			child.rotation = playerChildTransforms [count].rotation;
			count++;
		}
	}

	bool MatchesTranslatedObjectName(string childName){
		string translatedObjectName = stretchingJoint.transform.name;//stretchingRightHand.transform.name;
		if (childName.Equals (translatedObjectName)) {
			return true;
		}
		return false;
	}
}


