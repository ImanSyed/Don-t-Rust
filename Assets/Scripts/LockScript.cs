using UnityEngine;

public class LockScript : MonoBehaviour {

    PCScript pc;
    bool pcNear;
    [SerializeField] short armsRequired, torsoRequired, legsRequired; 

	// Use this for initialization
	void Start () {
        pc = FindObjectOfType<PCScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && pcNear)
        {
            if(pc.armsType == armsRequired && pc.torsoType == torsoRequired && pc.legType == legsRequired)
            {

            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == pc.gameObject)
        {
            pcNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == pc.gameObject)
        {
            pcNear = false;
        }
    }
}
