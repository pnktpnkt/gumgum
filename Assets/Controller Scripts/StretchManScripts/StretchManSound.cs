using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchManSound : MonoBehaviour {

	public AudioSource stretchingClip;
	public AudioSource shrinkingClip;

	//initialize Audio
	void Start () {
		
	}

	public void playStretchingClip(){

		stretchingClip.Play ();
	}

	public void stopStretchingClip(){
		
		stretchingClip.Stop ();
	}

	public void playShrinkingClip(){

		shrinkingClip.Play ();
	}

	public void stopShrinkingClip(){

		shrinkingClip.Stop ();
	}


}
