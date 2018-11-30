using UnityEngine;

public class LockScript : MonoBehaviour {

    PCScript pc;
    bool pcNear;
    [SerializeField] short armsRequired, torsoRequired, legsRequired;
    [SerializeField] Sprite unlocked;
    [SerializeField] GameObject C;


	// Use this for initialization
	void Start () {
        pc = FindObjectOfType<PCScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C) && pcNear)
        {
            if(pc.armsType == armsRequired && pc.torsoType == torsoRequired && pc.legsType == legsRequired)
            {
                GetComponent<SpriteRenderer>().sprite = unlocked;
                pc.LockOpen();
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == pc.gameObject)
        {
            pcNear = true;
            C.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == pc.gameObject)
        {
            pcNear = false;
            C.SetActive(false);
        }
    }
}
