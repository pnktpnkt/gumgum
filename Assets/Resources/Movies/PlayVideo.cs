using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayVideo : MonoBehaviour {

	public MovieTexture WatchMovie;
	public MovieTexture PunchMovie;

	// Use this for initialization
	void Start () {

		GetComponent<RawImage> ().texture = WatchMovie as MovieTexture;
		WatchMovie.Play ();
		WatchMovie.loop = true;
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	public void PlayMovie1(){
		//WatchMovie.loop = true;
	}

	public void CancelMovie1(){
		WatchMovie.Stop ();
	}

	public void PlayMovie2(){
		GetComponent<RawImage> ().texture = PunchMovie as MovieTexture;
		GetComponent<RawImage> ().rectTransform.sizeDelta = new Vector2 (304, 173);
		PunchMovie.Play ();
		PunchMovie.loop = true;
	}

	public void CancelMovie2(){
		PunchMovie.Stop ();
	}

	public void DisableMovieTexture(){
		gameObject.SetActive (false);
	}

}
