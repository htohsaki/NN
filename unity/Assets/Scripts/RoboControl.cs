using UnityEngine;
using System.Collections;

public class RoboControl : MonoBehaviour {

	private Animation animation;

	public float speed = 0.0001f;

	private Vector3 vec;

	private bool isMove = false;


	private string[] AnimationKeyLoop = new string[]{
		"loop_idle", "loop_run_funny", "loop_walk_funny"
	};

	private string[] AnimationKeyCombo = new string[]{
		"cmb_street_fight"
	};

	private string[] AnimationKeyKick = new string[]{
		"kick_jump_right", "kick_lo_right"
	};

	private string[] AnimationKeyPunch = new string[]{
		"punch_hi_left", "punch_hi_right"
	};

	private string[] AnimationKeyOther = new string[]{
		"def_head", "final_head", "jump",  "xhit_body", "xhit_head"
	};



	// Use this for initialization
	void Start () {
		this.animation = GetComponent<Animation> ();
		this.speed = 0.01f;
		this.transform.Rotate(new Vector3(0.0f, 39.24f, 0.0f));
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isMove) {
			Debug.Log (this.transform.forward * this.speed);
			this.transform.Translate (this.transform.right * this.speed * -1.0f, this.transform);
		}
		this.transform.FindChild ("robo_rigg_2014:Main").transform.localRotation = Quaternion.Euler(new Vector3 (0.0f, 0.0f, 0.0f));
	}

	public void Move(){
		this.isMove = true;
		this.Walk ();
	}

	public void Stop(){
		this.isMove = false;
	}

	private void Walk(){
		this.animation.CrossFade (AnimationKeyLoop [2]);
	}

	private void Run(){
		this.animation.CrossFade (AnimationKeyLoop [1]);
	}

	public void Fight(){
		this.Stop ();
		this.animation.CrossFade (AnimationKeyCombo [0]);
		this.animation.CrossFadeQueued (AnimationKeyLoop [0]);
	}

	public void Idle(){
		this.Stop ();
		this.animation.CrossFade (AnimationKeyLoop [0]);
	}

	public void Jump(){
		this.Stop ();
		this.animation.CrossFade (AnimationKeyOther [2]);
		this.animation.CrossFadeQueued (AnimationKeyLoop [0]);
	}

}
