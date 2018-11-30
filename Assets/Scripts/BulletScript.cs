using UnityEngine;

public class BulletScript : MonoBehaviour {

	void Start ()
    {
        Invoke("DestroyMe", 5);
	}
	
	void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 12)
        {
            Destroy(gameObject);
        }
    }
}
