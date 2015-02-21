using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	List<CharacterUnit> teddies;

	// Use this for initialization
	void Start () {

		this.teddies = new List<CharacterUnit> ();

		CharacterUnit cu1 = new CharacterUnit (this, "Teddy", new Vector3 (0, 0, 0));
		CharacterUnit cu2 = new CharacterUnit (this, "Teddy", new Vector3 (2, 0, 2));

		this.teddies.Add (cu1);
		this.teddies.Add (cu2);

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnGUI(){

		/*

		if (GUI.Button (new Rect (230, 10, 100, 30), "Jump")) {
			((CharacterCtrl)(this.teddy.GetComponent<CharacterCtrl> ())).Jump ();
		}
		if (GUI.Button (new Rect (340, 10, 100, 30), "Walk")) {
			((CharacterCtrl)(this.teddy.GetComponent<CharacterCtrl> ())).Walk ();
		}
		if (GUI.Button (new Rect (340, 10, 100, 30), "Run")) {
			((CharacterCtrl)(this.teddy.GetComponent<CharacterCtrl> ())).Run ();
		}

*/

		if (GUI.Button (new Rect (230, 10, 100, 30), "CheckAngle")) {
			Debug.Log(this.teddies [0].InView (this.teddies [1])  + " / " + DateTime.Now);
		}

	}
}
