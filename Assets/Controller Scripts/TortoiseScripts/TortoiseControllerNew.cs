using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TortoiseControllerNew : MonoBehaviour {
	
    // Use this for initialization
	public Color maxHealthColor = Color.green;
	public Color minHealthColor = Color.red;
	public Slider healthSlider;
	public Image HPGauge;
	public GameObject rendererObject;

	private const float velocity = 0.005f;
	private const int bossMaxHP = 600;

	//private Color colorStart;
	//private Color colorEnd;

	private Transform player;
	private GameObject playerWrist;

	private IEnumerator coroutine;

	private SkinnedMeshRenderer turtleRenderer;
	private Animator anim;                          
    private AnimatorStateInfo currentBaseState;

    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int walkState = Animator.StringToHash("Base Layer.Walk");
	static int howlState = Animator.StringToHash("Base Layer.Howl");
	static int attackState = Animator.StringToHash("Base Layer.Attack");
	static int wrathState = Animator.StringToHash("Base Layer.Wrath");
    static int damageState = Animator.StringToHash("Base Layer.GetDamage");
	static int deathState = Animator.StringToHash("Base Layer.Death");

	private RhandCollsionDetection collisionDetection;

	private AudioSource punchClip;
	private AudioSource attackClip1;
	private AudioSource attackClip3;
	private AudioSource idleClip;
	private AudioSource walkClip;

	private int bossHP;
	private bool isActivateTimer = false;
	private bool isPunched = false;
   // private float Timer = 0;
    //private float movingSpeed = 0;
	//private bool isWalking = false;

	enum State{
		Initial,
		Idle,
		Walk,
		Howl,
		Attack,
		Wrath,
		Damage,
		Death
	};

	State state = State.Idle;
	State nextState = State.Idle;

    void Start () {
		bossHP = bossMaxHP;
        anim = GetComponent<Animator>();
		turtleRenderer = rendererObject.GetComponent<SkinnedMeshRenderer> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		collisionDetection = GameObject.FindGameObjectWithTag ("RightHand").GetComponent<RhandCollsionDetection> ();
		//colorStart = turtleRenderer.material.color;
		//colorEnd = turtleRenderer.material.color;
		//StartCoroutine(TortoiseFadeIn (5));
		AudioSourcresInitialization ();
    }

	void Update () {
		//Debug.Log (state);
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);

		switch (state) {
		case State.Initial:
			ActionSelector (bossHP);
			break;
		case State.Idle:
			if(isPunched){
				StopCoroutine (coroutine);
				isPunched = false;
				//anim.SetBool ("isIdle", false);
				ChangeState (State.Damage);
			}
			else if(!isActivateTimer){
				coroutine = ChangeStateCounter(3, State.Initial, "isIdle");
				StartCoroutine (coroutine);
				isActivateTimer = true;
			}
			break;
		case State.Walk:
			if (isPunched) {
				StopCoroutine (coroutine);
				isPunched = false;
				anim.SetBool ("isWalking", false);
				ChangeState (State.Damage);
			} 
			else if (!isActivateTimer) {
				coroutine = ChangeStateCounter(3, State.Initial, "isWalking");
				StartCoroutine (coroutine);
				isActivateTimer = true;
			} 

			OnWalk();
			break;

		case State.Howl: 
			OnHowl ();
			break;
		case State.Attack: 
			OnAttack ();
			break;
		case State.Wrath:
			OnWrath ();
			break;
		case State.Damage:
			OnDamage ();
			break;
		case State.Death:

			break;
		}

		if (state != nextState) {
			state = nextState;
			switch (state){
			case State.Initial:
				isActivateTimer = false;
				anim.SetBool ("isIdle", true);
				break;
			case State.Idle:
				IdleStart ();
				break;
			case State.Walk:
				WalkStart ();
				break;
			case State.Howl:
				HowlStart ();
				break;
			case State.Attack:
				AttackStart ();
				break;
			case State.Wrath:
				WrathStart ();
				break;
			case State.Damage:
				isActivateTimer = false;
				DamageStart ();
				break;
			case State.Death:
				anim.SetTrigger ("isDead");
				break;
			}

		}

	/*	currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		anim.SetFloat ("TurtleSpeed", movingSpeed);

		if(bossHP <= 0){
			anim.SetTrigger ("isDeath");
		}

		if (isWalking) {
			Walk ();
		}

		EnemyLocomotion ();
		DamageProcessing ();

 	   if (currentBaseState.fullPathHash == damageState) {
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isHurt", false);
				CounterAction ();
			}
        }
        else if (currentBaseState.fullPathHash == idleState){
			Idle ();
        }
		else if(currentBaseState.fullPathHash == attackState1){
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isAttacking1", false);
			}
		}
		else if(currentBaseState.fullPathHash == attackState2){
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isAttacking2", false);
			}
		}
		else if(currentBaseState.fullPathHash == shootingState){
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isExplosion", false);
			}
		} */
	}

	void ChangeState(State nextState){
		this.nextState = nextState;
	}

	IEnumerator ChangeStateCounter(float waitTime, State nextState, string animFlag){
		yield return new WaitForSeconds (waitTime);
		anim.SetBool (animFlag, false);
		this.nextState = nextState;
	}

	void ActionSelector(int currentHP){
		if (currentHP < 300) {
			WrathActionPattern ();
		} else if (currentHP > 0) {
			NormalActionPattern ();
		}
	}

	void NormalActionPattern(){
		int rand = Random.Range (0, 12);
		//Debug.Log (rand);
		if (rand < 4) {
			ChangeState(State.Walk);
		} else if(rand < 8){
			ChangeState(State.Howl);
		} else if(rand < 12){
			ChangeState(State.Attack);
		}
		anim.SetBool ("isIdle", false);
	}

	void WrathActionPattern(){
		int rand = Random.Range (0, 12);
		//Debug.Log (rand);
		if (rand < 4) {
			ChangeState(State.Howl);
		} else if(rand < 8){
			ChangeState(State.Attack);
		} else if(rand < 12){
			ChangeState(State.Wrath);
		}
		anim.SetBool ("isIdle", false);
	}

	void IdleStart(){
		if (!anim.IsInTransition (0)) {
			idleClip.Play ();
			//anim.SetBool ("isWalking", false);
			//anim.SetBool ("isIdle", true);
		}
	}

	void WalkStart(){
		if (!anim.IsInTransition (0)) {
			//anim.SetBool ("isIdle", false);
			anim.SetBool ("isWalking", true);
		}
	}

	void OnWalk(){
		if (Vector3.Distance (this.transform.position, player.transform.position) > 3) {
			Vector3 direction = (player.transform.position - this.transform.position).normalized;
			direction.y = 0;
			//this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime);

			this.transform.Translate (velocity * direction);
			//Debug.DrawLine(this.transform.position, player.transform.position, Color.red);
			//Debug.Log(Vector3.Distance(this.transform.position, player.transform.position));
		}
	}

	void HowlStart(){
		if (!anim.IsInTransition (0)) {
			//anim.SetBool ("isIdle", false);
			//anim.SetBool ("isWalking", false);
			anim.SetBool ("isHowling", true);
		}
	}

	void OnHowl(){
		if (currentBaseState.fullPathHash == howlState) {
			if (isPunched) {
				anim.SetBool ("isHowling", false);
				isPunched = false;
				ChangeState (State.Damage);
			}
			else if (!anim.IsInTransition (0)) {
				anim.SetBool ("isHowling", false);
			} 
		} else {
			ChangeState (State.Initial);
		}
	}
		
	void AttackStart(){
		if (!anim.IsInTransition (0)) {
			anim.SetBool ("isAttacking", true);
		}
	}

	void OnAttack(){
		if (currentBaseState.fullPathHash == attackState) {
			 if (isPunched) {
				anim.SetBool ("isAttacking", false);
				isPunched = false;
				ChangeState (State.Damage);
			}
			else if (!anim.IsInTransition (0)) {
				anim.SetBool ("isAttacking", false);
			} 
		} else {
			ChangeState (State.Initial);
		}
	}

	void WrathStart(){
		if (!anim.IsInTransition (0)) {
			anim.SetBool ("isAngry", true);
		}
	}

	void OnWrath(){
		if (currentBaseState.fullPathHash == wrathState) {
			if (isPunched) {
				anim.SetBool ("isAngry", false);
				isPunched = false;
				ChangeState (State.Damage);
			}
			else if (!anim.IsInTransition (0)) {
				anim.SetBool ("isAngry", false);
			} 
		} else {
			ChangeState (State.Initial);
		}
	}

	void DamageStart(){
		//anim.SetBool ("isIdle", false);
		//anim.SetBool ("isWalking", false);
		anim.SetBool ("isHurt", true);

		punchClip.Play ();
		if (collisionDetection.GetPunchFire()) {
			bossHP -= 150;
		} else {
			bossHP -= 100;
		}
		updateBossHPGauge (bossHP);
		AudioSourcesCancellation ();
	}

	void OnDamage(){
		if (currentBaseState.fullPathHash == damageState) {
			if(bossHP<=0){
				anim.SetBool ("isHurt", false);
				anim.SetTrigger ("isDead");
				ChangeState (State.Death);
			}
			else if (!anim.IsInTransition (0)) {
				anim.SetBool ("isHurt", false);
				ChangeState (State.Initial);
			}
		}
	}

/*	IEnumerator TortoiseFadeIn(float duration){
		colorStart.a = 0;
		for(float i=0; i<duration; i+=Time.deltaTime){
			turtleRenderer.material.color = Color.Lerp(colorStart, colorEnd, i/duration);
			yield return null;
		}
	}*/

	void AudioSourcresInitialization(){
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		punchClip = audioSources [0];
		walkClip = audioSources [1];
		idleClip = audioSources [2];
		attackClip3 = audioSources [3];
		attackClip1 = audioSources [4];
	}

	void AudioSourcesCancellation(){
		attackClip1.Stop();
		attackClip3.Stop();
		idleClip.Stop();
		walkClip.Stop();
	}

	void updateBossHPGauge(int currentHP){
		healthSlider.value = currentHP;
		HPGauge.color = Color.Lerp (minHealthColor, maxHealthColor, (float)currentHP/bossMaxHP);
	}

	/* 	void EnemyLocomotion(){
		if (Vector3.Distance (this.transform.position, player.transform.position) > 4) {
			Vector3 direction = (player.transform.position - this.transform.position).normalized;
			direction.y = 0;
			//this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime);

			if (movingSpeed > 0) {
				this.transform.Translate (velocity * movingSpeed * direction);
			}
			//Debug.DrawLine(this.transform.position, player.transform.position, Color.red);
			//Debug.Log(Vector3.Distance(this.transform.position, player.transform.position));
		}
	}

	void DamageProcessing(){
		if (isPunched) {
			anim.SetBool ("isHurt", true);
			punchClip.Play ();
			if (collisionDetection.GetPunchFire()) {
				bossHP -= 150;
			} else {
				bossHP -= 100;
			}
			updateBossHPGauge (bossHP);
			isPunched = false;
			walkCancel ();	
			AudioSourcesCancellation ();
		} 
	}



   void Idle() {
		if (Timer == 0) {
			idleClip.Play ();
		}
		Timer += Time.fixedDeltaTime;

		if (bossHP > 300) {
			NormalActionPattern ();
		} else {
			WrathActionPattern ();
		}

    }

	void NormalActionPattern(){
		int rand = Random.Range (0, 12);
		if (Timer > 5.0) {
			if (rand > 6) {
				isWalking = true;
				walkClip.Play ();
				Timer = 0;
			} else if (rand > 2) {
				anim.SetBool ("isAttacking1", true);
				attackClip1.Play ();
				Timer = 0;
			} else {
				anim.SetBool ("isAttacking2", true);
				//attackClip2.PlayDelayed (4.0f);
				Timer = 0;
			}
		}
	}

	void WrathActionPattern(){
		int rand = Random.Range (0, 12);
		if (Timer > 3.0) {
			if (rand > 7) {
				isWalking = true;
				walkClip.Play ();
				Timer = 0;
			} else if (rand > 4) {
				anim.SetBool ("isAttacking1", true);
				attackClip1.Play ();
				Timer = 0;
			} else if (rand > 2) {
				anim.SetBool ("isAttacking2", true);
				//attackClip2.PlayDelayed (4.0f);
				Timer = 0;
			} else {
				anim.SetBool ("isExplosion", true);
				attackClip3.Play ();
				Timer = 0;
			}
		}
	}

	void CounterAction(){
		if (bossHP >= 500) {
			anim.SetBool ("isAttacking1", true);
			attackClip1.Play ();
		} else if (bossHP >= 300) {
			anim.SetBool ("isAttacking2", true);
		} else if (bossHP == 200) {    // impell the boss to use skill
			anim.SetBool ("isExplosion", true);
			attackClip3.Play ();
			//Instantiate (attackRock, );
		}else{
			int rand = Random.Range (0, 1);
			if (rand == 1) {
				anim.SetBool ("isExplosion", true);
				attackClip3.Play ();
			} else {
				anim.SetBool ("isAttacking2", true);
				//attackClip2.PlayDelayed (4.5f);
			}
		}
	}

    void Walk() {
		Timer += Time.fixedDeltaTime;
		if (Timer < 5.0){
            if (movingSpeed <= 0.5) {
                movingSpeed += Time.fixedDeltaTime;
            }
        }
        else{
            if (movingSpeed > 0.0){
                movingSpeed -= Time.fixedDeltaTime;
            }
            else{
				walkCancel ();
            }
        }
    }

	void walkCancel(){
		isWalking = false;
		movingSpeed = 0;
		Timer = 0;
		walkClip.Stop();
	}*/


	public int GetTortoiseHP(){
		return bossHP;
	}

	public void GetHit(){
		isPunched = true;
	}
}
