using UnityEngine;
using System.Collections;
using System;

public class CharacterUnit  {

	private World world;
	private GameObject flocks;
	private string modelName;
	private float _speed;
	private GameObject character;

	//視野の半径
	private float viewR = 5.0f;
	//視野の角度
	private float viewTheta = 270.0f;

	public CharacterUnit(World world, string modelName, Vector3 position){
		this.modelName = modelName;
		this.world = world;
		//this.flocks = this.world.gameObject.transform.Find ("Flocks").gameObject;

		GameObject parent = GameObject.Find ("Flocks");

		Debug.Log (parent);
		this.character = UnityEngine.Object.Instantiate(Resources.Load(this.modelName)) as GameObject;
		this.character.transform.parent = parent.transform;// this.flocks.transform;

		this.Position = position;
	}

	public Vector3 Position{
		get{
			return this.character.transform.localPosition;
		}
		set{
			this.character.transform.localPosition = value;
		}
	}

	public Vector3 Rotation{
		get{
			return this.character.transform.localRotation.eulerAngles;
		}
		set{
			this.character.transform.localRotation = Quaternion.Euler (value);
		}
	}

	public float Speed{
		get{
			return this._speed;
		}
		set{
			this._speed = value;
		}
	}

	public void SetView(float r, float theta){
		this.viewR = r;
		this.viewTheta = theta;
	}

	public bool InView(CharacterUnit unit){
	//	Debug.Log ("---------------------" + " / " + DateTime.Now);
		float r = Vector3.Distance (unit.Position,this.Position);

	//	Debug.Log ("r = " + r + " / " + DateTime.Now);

		if (r > viewR) {
			return false;
		}

		Vector2 unitVec2 = new Vector2 (unit.Position.x, unit.Position.z);
		Vector2 thisVec2 = new Vector2 (this.Position.x, this.Position.z);
	//	Debug.Log (unitVec2 + " / " + DateTime.Now);
	//	Debug.Log (thisVec2 + " / " + DateTime.Now);

		float dx = unitVec2.x - thisVec2.x;
		float dy = unitVec2.y - thisVec2.y;

		float ang = ((Mathf.Atan(dy / dx) / Mathf.PI 
			+ ((dx < 0)? 1.0f : 0.0f)) * 180.0f + 720.0f - 90) % 360 ;

		if (dx == 0 && dy == 0) {
			ang = 0;
		}

		//float ang = Vector2.Angle (unitVec2 ,thisVec2);

		Debug.Log ("angle = " + ang + " / " + DateTime.Now);

		if (ang > 360 - this.viewTheta / 2 || ang < this.viewTheta / 2) {
			return true;
		}

		return false;
	}
}
