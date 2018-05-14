using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_event : MonoBehaviour {
	Animator anime;
	bool ittrue;
	// Use this for initialization
	void Start () {
		anime = GetComponent<Animator> ();

	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.E) == true && ittrue == true){
			if (anime.GetBool ("door_open") == true) {
				anime.SetBool ("door_open", false);
			} else {
				anime.SetBool ("door_open", true);
			}
		}
	}
	void OnTriggerEnter(){
		ittrue = true;
	}
	void OnTriggerExit(){
		ittrue = false;
	}
}
