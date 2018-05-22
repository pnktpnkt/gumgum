using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Vibrator: MonoBehaviour
{
	private AudioClip audioClip;
	private OVRHapticsClip collisionClip;

	void Start(){
		byte[] samples = new byte[128];
		for(int i=0;i<samples.Length;i++){
			samples [i] = 255;
		}
		this.collisionClip = new OVRHapticsClip (samples, samples.Length); 
//		this.audioClip = audioClip;
//		collisionClip = new OVRHapticsClip(audioClip);
	}

	public void playCollisionClip(){

		OVRHaptics.RightChannel.Preempt (collisionClip);
		OVRHaptics.LeftChannel.Preempt (collisionClip);

	}

}

