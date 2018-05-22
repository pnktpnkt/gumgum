using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TortoiseController : MonoBehaviour {
	
    // Use this for initialization
	public Color maxHealthColor = Color.green;
	public Color minHealthColor = Color.red;
	public Slider healthSlider;
	public Image HPGauge;
	//public GameObject rendererObject;

	private const float velocity = 0.02f;
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

	private int bossHP = 600;
	private bool isPunched = false;
    private float Timer = 0;
    private float movingSpeed = 0;
	private bool isWalking = false;

    void Awake () {
        anim = GetComponent<Animator>();
		//turtleRenderer = rendererObject.GetComponent<SkinnedMeshRenderer> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		collisionDetection = GameObject.FindGameObjectWithTag ("RightHand").GetComponent<RhandCollsionDetection> ();
		//colorStart = turtleRenderer.material.color;
		//colorEnd = turtleRenderer.material.color;
		//StartCoroutine(TortoiseFadeIn (5));
		AudioSourcresInitialization ();
    }

	void Update () {

		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
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
		else if(currentBaseState.fullPathHash == howlState){
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isHowling", false);
			}
		}
		else if(currentBaseState.fullPathHash == attackState){
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isAttacking", false);
			}
		}
		else if(currentBaseState.fullPathHash == wrathState){
			if (!anim.IsInTransition (0)) {
				anim.SetBool ("isAngry", false);
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

 	void EnemyLocomotion(){
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
				bossHP -= 200;
			} else {
				bossHP -= 150;
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
				anim.SetBool ("isHowling", true);
				attackClip1.Play ();
				Timer = 0;
			} else {
				anim.SetBool ("isAttacking", true);
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
				anim.SetBool ("isHowling", true);
				attackClip1.Play ();
				Timer = 0;
			} else if (rand > 2) {
				anim.SetBool ("isAttacking", true);
				//attackClip2.PlayDelayed (4.0f);
				Timer = 0;
			} else {
				anim.SetBool ("isAngry", true);
				attackClip3.Play ();
				Timer = 0;
			}
		}
	}

	void CounterAction(){
		if (bossHP >= 500) {
			anim.SetBool ("isHowling", true);
			attackClip1.Play ();
		} else if (bossHP >= 300) {
			anim.SetBool ("isAttacking", true);
		} else if (bossHP >= 200) {    // impell the boss to use skill
			anim.SetBool ("isAngry", true);
			attackClip3.Play ();
			//Instantiate (attackRock, );
		}else{
			int rand = Random.Range (0, 1);
			if (rand == 1) {
				anim.SetBool ("isAngry", true);
				attackClip3.Play ();
			} else {
				anim.SetBool ("isAttacking", true);
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
	}


	public int GetTortoiseHP(){
		return bossHP;
	}

	public void GetHit(){
		isPunched = true;
	}
}
