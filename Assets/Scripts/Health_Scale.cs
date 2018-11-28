using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Scale : MonoBehaviour
{
    public SpriteRenderer hColor;
    public float hScale = 1;
	
	// Update is called once per frame
	void Update ()
    {
        hColor.transform.localScale = new Vector3(0, hScale, 0);
	}
}
