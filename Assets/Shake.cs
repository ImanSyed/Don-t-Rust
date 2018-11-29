using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	bool disabled;
	int shakeX, shakeY;
	// Use this for initialization
	void Start () {
		disabled = false;
		shakeX = shakeY = 5;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown(KeyCode.F) && !disabled)
		{
			disabled = true;
			if(shakeX>=0 || shakeY>=0)
			{	
				transform.position = new Vector3 (200, 200, 0);
			}
		}
	}


}
