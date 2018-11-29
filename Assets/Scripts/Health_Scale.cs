using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Scale : MonoBehaviour
{
    public SpriteRenderer hColor;
    public float hScale;
    PCScript pc;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
    }

    // Update is called once per frame
    void Update ()
    {
        hColor.transform.localScale = new Vector3(1, pc.health / 100, 1);
	}
}
