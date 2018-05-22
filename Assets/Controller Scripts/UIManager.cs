using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour {

	public EnemiesManager enemiesController;
	public GameObject movieObject;

	private Image canvasImage;
	private PlayVideo moviePlayer;
	private Sprite[] tutorialsprites; 
	private Sprite[] victorysprites;

	private AudioSource pageTurnClip;

	private int tutorialIndex = 1;
	private int victoryIndex = 0;

	private bool isEndingTutorial = false;
	private bool isVictory = false;
	private bool isSkippingTutorial = false;

	// Use this for initialization
	void Start () {
		moviePlayer = movieObject.GetComponent<PlayVideo> ();
		pageTurnClip = GetComponent<AudioSource> ();
		enemiesController.GetComponent<EnemiesManager> ();
		canvasImage = GetComponent<Image>();
		tutorialsprites = Resources.LoadAll<Sprite> ("TutorialNew2");
		victorysprites = Resources.LoadAll<Sprite> ("VictorySprites");

	}
	
	// Update is called once per frame
	void Update () {
		if (enemiesController.CheckVictory() == true && !isVictory) { 
			StartCoroutine ( ImageFadeIn () );
			isVictory = true;
		}
		else if (PressAnyButtonR() && isVictory) {    // change anotheer victory image
			if(victoryIndex < victorysprites.Length ){
				StartCoroutine (ImageFadeIn ());
				pageTurnClip.Play ();
				canvasImage.sprite = victorysprites [victoryIndex++];
			}
		}
		else if (PressAnyButtonR() && !isEndingTutorial) {          // change tutorial images
			if (tutorialIndex < tutorialsprites.Length) {
				pageTurnClip.Play ();
				if (tutorialIndex == 1) {
					moviePlayer.CancelMovie1 ();
					moviePlayer.PlayMovie2 ();
				} else if(tutorialIndex == 2){
					moviePlayer.CancelMovie2 ();
					moviePlayer.DisableMovieTexture ();
				}
				canvasImage.sprite = tutorialsprites [tutorialIndex++];
			} else {
				StartCoroutine (ImageFading ());
				isEndingTutorial = true;
			}
		}
		else if(isSkippingTutorial){
			moviePlayer.DisableMovieTexture ();
			canvasImage.sprite = victorysprites [victoryIndex++];
			canvasImage.enabled = false;
			isEndingTutorial = true;
		}
	}

	bool PressAnyButtonR(){
		if (OVRInput.GetDown (OVRInput.RawButton.A) || OVRInput.GetDown (OVRInput.RawButton.B) ||OVRInput.GetDown (OVRInput.RawButton.X)||OVRInput.GetDown (OVRInput.RawButton.Y)) {
			return true;
		} else {
			return false;
		}
	}

	IEnumerator ImageFading(){
		for (float f = 1f; f >= 0; f -= 0.01f) {
			Color newColor = canvasImage.color;
			newColor.a = f; 
			canvasImage.color = newColor;

			if (canvasImage.color.a < 0.01) {
				canvasImage.sprite = victorysprites [victoryIndex++];
				canvasImage.enabled = false;
			}
			yield return null;
		}
	}

	IEnumerator ImageFadeIn(){

		if(!canvasImage.enabled){
			yield return new WaitForSeconds(3);
			canvasImage.enabled = true;
		}

		for (float f = 0; f<1; f += 0.01f) {
			Color newColor = canvasImage.color;
			newColor.a = f; 
			canvasImage.color = newColor;
			yield return null;
		}
	}

	public bool CheckEndTutorial(){
		return isEndingTutorial;
	}
}
