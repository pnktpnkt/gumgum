using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesManager : MonoBehaviour {

	public GameObject RinoPrefab;
	public GameObject TortoisePrefab;
	public GameObject UIImage;
	public GameObject PlayerCamera;
	public GameObject SpawnedSpaces;

	private GameObject Tortoise;
	public Transform[] SpawnedTransforms;
	public Transform middleTransform;

	private UIManager UIController;

	private AudioSource WinningClip;
	private AudioSource BGMClip;

	private const int Wave1Rinos = 2;
	private int defeatedRinos = 0;
	private int rinosOnMap = 0;
	private int bossHP = 600;
	private bool isVictory = false;

	private enum State {
		Idle,
		Wave1,
		Wave2
	};
	private State battleState = State.Idle;

	// Use this for initialization
	void Start () {
		UIController = UIImage.GetComponent<UIManager> ();
		SpawnedTransforms = SpawnedSpaces.GetComponentsInChildren<Transform> ();
		AudioSource[] ListenerAudioSources = PlayerCamera.GetComponents<AudioSource> ();
		BGMClip = ListenerAudioSources [0];
		WinningClip = ListenerAudioSources [1];
	}
	
	// Update is called once per frame
	void Update () {
		// Restart the Game
		if(Input.GetKeyDown ("space")){
			SceneManager.LoadScene (0);
		}

		switch(battleState){

		case State.Idle:
			if (UIController.CheckEndTutorial () == true && !isVictory) {
				BGMClip.Play ();
				InvokeRepeating ("SpawnRinoI", 3, 3);
				defeatedRinos = 0;
				battleState = State.Wave1;
			}
			break;
		case State.Wave1:
			if (defeatedRinos >= Wave1Rinos) {
			//	InvokeRepeating ("SpawnRinoII", 4, 7);
				Invoke ("SpawnTurtleBoss", 0.5f);
				battleState = State.Wave2;
			}
			break;
		case State.Wave2:
			if (Tortoise != null) {
				bossHP = Tortoise.GetComponent<TortoiseController>().GetTortoiseHP ();
				Debug.Log (bossHP);
				if (bossHP <= 0) {
					isVictory = true;
					battleState = State.Idle;
					Invoke ("VictoryAudios", 3);
					GameObject[] Rinos = GameObject.FindGameObjectsWithTag ("RinoMain");
					foreach (GameObject rino in Rinos) {
						if (rino != null) {
							Destroy (rino);
						}
					}
					battleState = State.Idle;
				}
			}
			break;
		}

	}
		
	void SpawnRinoI(){
		if( defeatedRinos >= Wave1Rinos ){
			return;
		}
		else if ( rinosOnMap < 1) {
			int rand = Random.Range (0, SpawnedTransforms.Length + 1);
			Debug.Log (rand);
			if (rand == SpawnedTransforms.Length) {
				Instantiate (RinoPrefab, middleTransform.position, middleTransform.rotation);
			} else {
				Instantiate (RinoPrefab, SpawnedTransforms [rand].position, SpawnedTransforms [rand].rotation);
			}
			rinosOnMap++;
		}

	}

	void SpawnRinoII(){
		if(bossHP<=0){
			return;
		}
		if (rinosOnMap < 1) {
			int rand = Random.Range (0, SpawnedTransforms.Length);
			Instantiate (RinoPrefab, SpawnedTransforms [rand].position, SpawnedTransforms [rand].rotation);
			rinosOnMap++;
		}
	}

	void SpawnTurtleBoss(){
		Tortoise = Instantiate (TortoisePrefab,middleTransform.transform.position, TortoisePrefab.transform.rotation) as GameObject ;
	}

	void VictoryAudios(){
		if(BGMClip.loop){
			BGMClip.loop = false;
			BGMClip.Stop ();
			WinningClip.Play ();
		}
	}

	public void EnemyDestroyed(){
		defeatedRinos++;
		rinosOnMap--;
	}

	public bool CheckVictory(){
		return isVictory;
	}

}
