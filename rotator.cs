using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour {
	Quaternion OriginRotation;
	float Mouse_Y;
	float Mouse_X;
	float Mouse_Sens = 2;
	// Use this for initialization
	void Start () {
		OriginRotation = transform.rotation;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Mouse_X += Input.GetAxis ("Mouse X") * Mouse_Sens;
		Mouse_Y += Input.GetAxis ("Mouse Y") * Mouse_Sens;
		Mouse_Y = Mathf.Clamp (Mouse_Y, -30, 60);
		Quaternion rotationX = Quaternion.AngleAxis (Mouse_X,Vector3.up);
		Quaternion rotationY = Quaternion.AngleAxis (-Mouse_Y,Vector3.right);
		transform.rotation = OriginRotation * rotationX * rotationY;
	}
}
