using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public int count = 4;
    [SerializeField] Vector2 minBounds, maxBounds;
    [SerializeField] GameObject enemy;
    EnemyScript[] enemies;

    public Vector2 startPos;

	void Start () {
        startPos = transform.position;
        enemies = new EnemyScript[count];
        while (count > 0)
        {
            GameObject e = Instantiate(enemy, transform.position, transform.rotation);
            transform.position = new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
            e.GetComponent<EnemyScript>().Initialize(this);
            enemies[count - 1] = e.GetComponent<EnemyScript>();
            count--;
        }
        foreach(EnemyScript es in enemies)
        {
            es.spawning = false;
        }
        InvokeRepeating("Check", 15, 15);
	}
	
	void Check()
    {
        if(count > 0)
        {
            GameObject e = Instantiate(enemy, transform.position, transform.rotation);
            transform.position = new Vector2(Random.Range(minBounds.x, maxBounds.x), Random.Range(minBounds.y, maxBounds.y));
            e.GetComponent<EnemyScript>().Initialize(this);
            enemies[count - 1] = e.GetComponent<EnemyScript>();
            count--;
            e.GetComponent<EnemyScript>().spawning = false;
        }
    }
}
