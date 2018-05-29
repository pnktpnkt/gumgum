using System;
using UnityEngine;

public class InvisibleAvatarMotionImitator
{
	GameObject invisibleAvatar;
	GameObject visibleAvatar;
	GameObject stretchingRightHand;
	GameObject stretchingJoint;

	public InvisibleAvatarMotionImitator ()
	{
		invisibleAvatar = GameObject.FindGameObjectWithTag ("Player");
		visibleAvatar = GameObject.FindGameObjectWithTag("VisibleAvatar");
		//stretchingRightHand = GameObject.FindGameObjectWithTag ("StretchingRightHand");
		stretchingJoint = GameObject.FindGameObjectWithTag ("StretchingJoint");
	}
		
	public void setInvisibleAvatar(GameObject avatar){
		invisibleAvatar = avatar;
	}

	public void setVisibleAvatar(GameObject avatar){
		visibleAvatar = avatar;
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
		Transform[] invisibleAvatarChildTransforms = invisibleAvatar.GetComponentsInChildren<Transform> ();
		Transform[] visibleAvatarChildTransforms = visibleAvatar.GetComponentsInChildren<Transform> ();
		var count = 0;
		bool isPositionCopied = true;

		foreach (Transform child in visibleAvatarChildTransforms) {
			if (isArmTranslated && MatchesTranslatedObjectName (child.name))
				isPositionCopied = false;
			if (isPositionCopied)
				child.position = invisibleAvatarChildTransforms[count].position;
			child.rotation = invisibleAvatarChildTransforms [count].rotation;
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


