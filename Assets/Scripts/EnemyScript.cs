using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public enum Enemy
    {
        red, green, yellow
    }

    public Enemy enemyType;

    bool aggro, rest, alert;

    float originalSpeed, health, damage, currentSpeed;

    PCScript pc;
    Animator anim;

    [SerializeField] CircleCollider2D trigger;

    Vector2 waypoint;

	void Start () {
        anim = GetComponent<Animator>();
        pc = FindObjectOfType<PCScript>();
        if(enemyType == Enemy.green)
        {
            originalSpeed = 0.025f;
            health = 50;
        }
        else if(enemyType == Enemy.red)
        {
            originalSpeed = 0.035f;
            health = 30;

        }
        else if(enemyType == Enemy.yellow)
        {
            originalSpeed = 0.01f;
            health = 60;

        }
        currentSpeed = originalSpeed;
    }

	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 32);
        if (!aggro)
        {
            Wander();
            if (currentSpeed != originalSpeed)
            {
                currentSpeed -= 0.025f;
            }
        }
        else
        {
            Chase();
            if(currentSpeed == originalSpeed)
            {
                currentSpeed += 0.025f;
            }
        }
	}

    void Wander()
    {
        if (!rest && !alert) {
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

            transform.position = Vector2.MoveTowards(transform.position, waypoint, originalSpeed);
            
            
        }

        if (alert)
        {
            if (pc.transform.position.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (pc.transform.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
    IEnumerator Rest()
    {
        rest = true;
        anim.SetBool("Running", false);
        yield return new WaitForSeconds(Random.Range(1, 5));
        waypoint = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        if (!alert)
        {
            anim.SetBool("Running", true);
        }
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
            trigger.radius = 2.5f;
        }
    }

    IEnumerator KillMe()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

    void Chase()
    {
        if(!anim.GetBool("Running"))
        {
            anim.SetBool("Running", true);
        }
        if (pc.transform.position.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (pc.transform.position.x < transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        transform.position = Vector2.MoveTowards(transform.position, pc.transform.position, currentSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyType == Enemy.yellow)
        {
            if (collision.tag == "Player")
            {
                aggro = true;
                trigger.radius = 3f;
            }
        }
        else
        {
            alert = true;
            waypoint = Vector2.zero;
            anim.SetBool("Running", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (aggro)
        {
            if (collision.tag == "Player")
            {
                aggro = false;
                trigger.radius = 1.5f;
            }
        }
        else
        {
            alert = false;
        }
    }
}
