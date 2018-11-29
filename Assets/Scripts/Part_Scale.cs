using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part_Scale : MonoBehaviour
{
    public SpriteRenderer pColor;
    public float pScale = 1;

    // Update is called once per frame
    void Update()
    {
        pColor.transform.localScale = new Vector3(1, pScale, 1);
    }
}
