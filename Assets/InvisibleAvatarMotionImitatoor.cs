using System;
using UnityEngine;

public class InvisibleAvatarMotionImitator
{
	GameObject invisibleAvatar;
	GameObject visibleAvatar;
	GameObject stretchingRightHand;
	GameObject stretchingJoint;
	GameObject stretchingObject;
    GameObject attackingObject;

    public InvisibleAvatarMotionImitator(GameObject invisibleAvatar, GameObject visibleAvatar, GameObject stretchingObject) {
        this.invisibleAvatar = invisibleAvatar;
        this.visibleAvatar = visibleAvatar;
        this.stretchingObject = stretchingObject;
    }

    public InvisibleAvatarMotionImitator (GameObject invisibleAvatar, GameObject visibleAvatar, GameObject stretchingObject, GameObject attackingObject)
	{
		this.invisibleAvatar = invisibleAvatar;
		this.visibleAvatar = visibleAvatar;
		this.stretchingObject = stretchingObject;
        this.attackingObject = attackingObject;
	}

	public void Imitate()
	{
		CopyObjectPositions (false);
	}

	public void ImitateWhileArmStretching()
	{
		CopyObjectPositions (true);
	}

	void CopyObjectPositions(bool isArmStretching)
	{
		Transform[] invisibleAvatarChildTransforms = invisibleAvatar.GetComponentsInChildren<Transform> ();
		Transform[] visibleAvatarChildTransforms = visibleAvatar.GetComponentsInChildren<Transform> ();
		var count = 0;
		bool isPositionCopied = true;

		foreach (Transform child in visibleAvatarChildTransforms) {
            if (!MatchesAttackingObjectName(child.name)) {
                if (isArmStretching && MatchesStretchingObjectName(child.name))
                    isPositionCopied = false;
                if (isPositionCopied)
                    child.position = invisibleAvatarChildTransforms[count].position;
                child.rotation = invisibleAvatarChildTransforms[count].rotation;
                count++;
            }
		}
	}

	bool MatchesStretchingObjectName(string name){
		string stretchingObjectName = stretchingObject.transform.name;
		if (name.Equals (stretchingObjectName)) {
			return true;
		}
		return false;
	}

    bool MatchesAttackingObjectName(String name) {
        string attackingObjectName = attackingObject.transform.name;
        if (name.Equals(attackingObjectName)) {
            return true;
        }
        return false;
    }
}


