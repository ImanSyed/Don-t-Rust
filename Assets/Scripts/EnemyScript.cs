using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public enum Enemy
    {
        red, green, yellow
    }

    public Enemy enemyType;

    bool aggro, rest;

    float speed, health, damage;

    PCScript pc;
    Animator anim;

    [SerializeField] CircleCollider2D trigger;

    Vector2 waypoint;

	void Start () {
        anim = GetComponent<Animator>();
        pc = FindObjectOfType<PCScript>();
        if(enemyType == Enemy.green)
        {
            speed = 0.05f;
            health = 50;
        }
        else if(enemyType == Enemy.red)
        {
            speed = 0.06f;
            health = 30;

        }
        else if(enemyType == Enemy.yellow)
        {
            speed = 0.04f;
            health = 60;

        }
    }

	void Update () {
        if (!aggro)
        {
            Wander();
        }
        else
        {
            Chase();
        }
	}

    void Wander()
    {
        if (!rest) {
            if ((Vector2)transform.position == waypoint)
            {
                waypoint = Vector2.zero;
            }
            else if (!anim.GetBool("Running"))
            {
                anim.SetBool("Running", true);
            }
            if (waypoint == Vector2.zero && !rest)
            {
                StartCoroutine(Rest());
            }
            if (waypoint.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if(waypoint.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            transform.position = Vector2.MoveTowards(transform.position, waypoint, speed);
            
            GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 32);
        }
    }
    IEnumerator Rest()
    {
        rest = true;
        anim.SetBool("Running", false);
        yield return new WaitForSeconds(Random.Range(1, 5));
        waypoint = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        anim.SetBool("Running", true);
        rest = false;
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            StartCoroutine(KillMe());
        }
        else if(!aggro)
        {
            aggro = true;
        }
    }

    IEnumerator KillMe()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, pc.transform.position, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyType == Enemy.yellow)
        {
            if (collision.tag == "Player")
            {
                aggro = true;
                trigger.radius = 3.5f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (aggro)
        {
            if (collision.tag == "Player")
            {
                aggro = false;
                trigger.radius = 2f;
            }
        }
    }
}
