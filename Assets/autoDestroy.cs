using System.Collections;
using UnityEngine;

public class autoDestroy : MonoBehaviour {

	// Use this for initialization
	private ParticleSystem ps;
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ps && !ps.IsAlive ()) 
		{
			Destroy (gameObject);
		}
		
	}
}
