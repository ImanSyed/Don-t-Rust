using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamColor : MonoBehaviour
{

	// Use this for initialization
	public Transform red, green, blue;
	public Color colRed, colGreen, colBlue;
	public float influenceRadius=8, coreRadius=2;
	Color colDefault, col;
	int mode = 0;

	void Start ()
	{
		colDefault = GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float redDist = Vector2.Distance (red.transform.position, transform.position);
		float greenDist = Vector2.Distance (green.transform.position, transform.position);
		float blueDist = Vector2.Distance (blue.transform.position, transform.position);

		if (redDist < influenceRadius) {
			if (redDist > coreRadius)
				GetComponent<SpriteRenderer> ().color = Vector4.Lerp (colRed, colDefault, (redDist - coreRadius) / (influenceRadius-coreRadius));
			else
				GetComponent<SpriteRenderer> ().color = colRed;
		} else if (greenDist < influenceRadius) {
			if (greenDist > coreRadius)
							GetComponent<SpriteRenderer> ().color = Vector4.Lerp (colGreen, colDefault, (greenDist - coreRadius) / (influenceRadius-coreRadius));
			else
				GetComponent<SpriteRenderer> ().color = colGreen;
		} else if (blueDist < influenceRadius) {
			if (blueDist > coreRadius)
							GetComponent<SpriteRenderer> ().color = Vector4.Lerp (colBlue, colDefault, (blueDist - coreRadius) / (influenceRadius-coreRadius));
			else
				GetComponent<SpriteRenderer> ().color = colBlue;
		} else {
			GetComponent<SpriteRenderer> ().color = colDefault;

		}			
		}
	}
