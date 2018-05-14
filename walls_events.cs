using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walls_events : MonoBehaviour {
	public gun _gun;
	void OnTriggerEnter(Collider arg){
		if (arg.gameObject.tag == "walls" ||  arg.gameObject.tag == "glass" || arg.gameObject.tag == "Door" ) {
			for (int i = 0; i < _gun.guns.Length; i++) {
				if (_gun.guns [i].enabled) {
					_gun.guns [i].anime.SetBool ("is_coll", true);
				}
			}
		}
	}
	void OnTriggerExit(Collider arg){
		if (arg.gameObject.tag == "walls" || arg.gameObject.tag == "glass" || arg.gameObject.tag == "Door" ) {
			for (int i = 0; i < _gun.guns.Length; i++) {
				if (_gun.guns [i].enabled) {
					_gun.guns [i].anime.SetBool ("is_coll", false);
				}
			}
		}
	}

}
