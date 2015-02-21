using UnityEngine;
using System.Collections;

public class CharacterCtrl : MonoBehaviour {

	private Animation animation;

	public float speed = 0.05f;

	public Vector3 rotationVec = new Vector3(0.0f,0.0f,0.0f);

	private bool isMove = false;




	private string[] AnimationKeyLoop = new string[]{
		"Basic_Run_01",
		"Basic_Run_02",
		"Basic_Run_03",
		"Basic_Walk_01",
		"Basic_Walk_02"
	};


	// Use this for initialization
	void Start () {
		this.animation = GetComponent<Animation> ();
		this.speed = 0.05f;
	}
		
	// Update is called once per frame
	void Update () {
		if (this.isMove) {
			Debug.Log (this.transform.forward * this.speed);
			this.transform.Translate (this.transform.forward * this.speed * 1.0f);
		}
	}

	public Vector3 rotation{
		get{
			return rotationVec;
		}
		set{
			this.transform.localRotation = Quaternion.Euler (value);
			this.rotationVec = value;
		}
	}


	public void Move(){
		this.isMove = true;
		this.Walk ();
	}

	public void Stop(){
		this.isMove = false;
		//Time.timeScale = 0.0f;
		this.animation.Stop ();
	}

	public void Walk(){
		this.animation.CrossFade (AnimationKeyLoop [3]);
	}

	public void Run(){
		this.animation.CrossFade (AnimationKeyLoop [2]);
	}

	public void Jump(){
		this.Stop ();
	}

}
