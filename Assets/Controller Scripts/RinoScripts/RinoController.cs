using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoController : MonoBehaviour {

	//public GameObject rinoHead;

	private Transform player;

	private EnemiesManager enemiesManager;
	//private RhandCollsionDetection collisionDetection;

	private Animator anim;
	private AnimatorStateInfo currentBaseState;

	//private SkinnedMeshRenderer[] RinoRenderers;
	//private SkinnedMeshRenderer[] RinoFaceRenderers;

	//static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int moveState = Animator.StringToHash("Base Layer.Move");
	static int attackState = Animator.StringToHash("Base Layer.Attack");
	static int deathState = Animator.StringToHash("Base Layer.Dead");

	private float timeCounter = 0; 
	private bool isHit = false;

	private enum State{
		Initial,
		Move,
		Attack,
		Death
	};

	private State state = State.Initial;
	private State nextState = State.Initial;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		enemiesManager = GameObject.FindGameObjectWithTag ("EnemiesBase").GetComponent<EnemiesManager>();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		//collisionDetection = GameObject.FindGameObjectWithTag ("RightHand").GetComponent<RhandCollsionDetection> ();
		//RinoRenderers = GetComponentsInChildren<SkinnedMeshRenderer> ();
		//RinoFaceRenderers = rinoHead.GetComponentsInChildren<SkinnedMeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

		nextState = Reducer (state);

		if (nextState == state) {
			switch(state){
			case State.Initial:
				break;
			case State.Move:
				break;
			case State.Attack:
				break;
			case State.Death:
				break;
			}

		} else {
			if(state == State.Initial && nextState == State.Death){		
				anim.SetBool ("isIdle", false);             
				anim.SetBool ("isDead", true);
				//StartCoroutine (DeathHandling(3));
				//StartCoroutine (FadeCoroutine ());
			}
			else if(state == State.Move && nextState == State.Death){		
				anim.SetBool ("isMoving", false);             
				anim.SetBool ("isDead", true);
				//StartCoroutine (DeathHandling(3));
				//StartCoroutine (FadeCoroutine ());
			}
			else if(state == State.Attack && nextState == State.Death){		
				anim.SetBool ("isAttacking", false);             
				anim.SetBool ("isDead", true);
				//StartCoroutine (DeathHandling(3));
				//StartCoroutine (FadeCoroutine ());
			}
			else if(state == State.Initial && nextState == State.Move){		// Start to run after waitseconds
				//StartCoroutine (MovingCoroutine(1.5f));   
				anim.SetBool ("isIdle", false);
				anim.SetBool ("isMoving", true);
			}
			else if(state == State.Move && nextState == State.Attack){	// Right to attack after closing to the player
				anim.SetBool ("isMoving", false);               
				anim.SetBool ("isAttacking", true);
			}

			state = nextState;
		}

	}

	State Reducer(State currentState){

		switch (currentState){
		case State.Initial:
			timeCounter += Time.deltaTime;

			if (isHit) {
				return State.Death;
			} else if (timeCounter >2.0f) {
				timeCounter = 0;
				return State.Move;
			}
			return State.Initial;

		case State.Move:

			if(isHit){
				return State.Death;
			}
			else if (currentBaseState.fullPathHash == moveState) {
				if (!anim.IsInTransition (0)) {
					Vector3 direction = player.position - this.transform.position;
					direction.y = 0;
					this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), Time.deltaTime);
					this.transform.Translate (0, 0, 0.01f);

					if (Vector3.Distance (player.position, this.transform.position) < 3) {
						return State.Attack;
					}
				}
			}
			return State.Move;

		case State.Attack:
			
			if(isHit){
				return State.Death;
			}
			else if (currentBaseState.fullPathHash == attackState) {
				if (!anim.IsInTransition (0)) {
					anim.SetBool ("isAttacking", false);
				}
			}

			timeCounter += Time.deltaTime;
			if(timeCounter < 5){
				anim.SetBool ("isAttacking", true);
				timeCounter = 0;
			}
			return State.Attack;
		
		case State.Death:
			timeCounter += Time.deltaTime;
			if(timeCounter>3){
				timeCounter = 0;
				enemiesManager.EnemyDestroyed ();
				//collisionDetection.
				Destroy (gameObject);
			}
			return State.Death;

		default:
			return currentState;
		}

	}

	IEnumerator MovingCoroutine(float waitTime){
		yield return new WaitForSeconds(waitTime);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isMoving", true);
	}

	IEnumerator AttackCoroutine(float waitTime){
		yield return new WaitForSeconds(waitTime);
		anim.SetBool ("isAttacking", true);
	}

	IEnumerator DeathHandling(float waitTime){
		yield return new WaitForSeconds(waitTime);
		enemiesManager.EnemyDestroyed ();
		Destroy (gameObject);
	}

/*	IEnumerator FadeCoroutine(){

		for(float f=1f; f>=0; f-=0.01f){
			foreach(SkinnedMeshRenderer render in RinoFaceRenderers){
				Color newColor = render.material.color;
				newColor.a = f;
				render.material.color = newColor;
			}

			foreach(SkinnedMeshRenderer render in RinoRenderers){
				Color newColor = render.material.color;
				newColor.a = f;
				render.material.color = newColor;
				//print (render.material.color);
			}
			yield return null;
		}
	}*/

	public void GetHit(){
		isHit = true;
	}
}
