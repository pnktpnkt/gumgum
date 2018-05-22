using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTransformAfter : MonoBehaviour
{
	public Transform particle;
	public float time;

	// Use this for initialization
	void Start () {
		Invoke("Create", time);
	}

	void Create()
	{
        //Vector3 v = transform.position;
		//Vector3 forwardVector = (player.position - transform.position).normalized;
        //v.y = 1.7f;
		//Destroy(Instantiate(particle, v + transform.forward * 20, transform.rotation).gameObject, 5);
		GameObject splashTarget = GameObject.Find("SuperStretchMan");
		Vector3 splashPosition = splashTarget.transform.position;
		splashPosition.y += 0.05f;
			
		Destroy(Instantiate(particle, splashPosition, transform.rotation).gameObject, 5); 
	}
}
