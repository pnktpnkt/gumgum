using System;
using UnityEngine;

public class InvisibleAvatarMotionImitator
{
	GameObject invisibleAvatar;
	GameObject visibleAvatar;
	GameObject stretchingRightHand;
	GameObject stretchingJoint;
	GameObject translatedParentObject;

	public InvisibleAvatarMotionImitator (GameObject invisibleAvatar, GameObject visibleAvatar, GameObject translatedParentObject)
	{
		this.invisibleAvatar = invisibleAvatar;
		this.visibleAvatar = visibleAvatar;
		this.translatedParentObject = translatedParentObject;
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
		string translatedParentObjectName = translatedParentObject.transform.name;
		if (childName.Equals (translatedParentObjectName)) {
			return true;
		}
		return false;
	}
}


