using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public int count = 4;
    [SerializeField] Vector2 minBounds, maxBounds;
    [SerializeField] GameObject enemy;
    EnemyScript[] enemies;
 

	void Start () {
        enemies = new EnemyScript[count];
        while (count > 0)
        {
            GameObject e = Instantiate(enemy, transform.position, transform.rotation);
            e.GetComponent<EnemyScript>().Initialize(this);
            enemies[count - 1] = e.GetComponent<EnemyScript>();
            count--;
            transform.position = new Vector2(Random.Range(minBounds.x, minBounds.y), Random.Range(maxBounds.x, maxBounds.y));
        }
        foreach(EnemyScript es in enemies)
        {
            es.spawning = false;
        }
        InvokeRepeating("Check", 6, 6);
	}
	

	void Check()
    {

    }
}
