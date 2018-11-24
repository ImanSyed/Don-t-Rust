using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public enum Enemy
    {
        red, green, yellow
    }

    public Enemy enemyType;

    bool aggro, rest, alert, attacking, stunned, dying;

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
            originalSpeed = 0.02f;
            health = 50;
            damage = 10;
        }
        else if(enemyType == Enemy.red)
        {
            originalSpeed = 0.035f;
            damage = 7.5f;
            health = 40;

        }
        else if(enemyType == Enemy.yellow)
        {
            originalSpeed = 0.01f;
            health = 60;
            damage = 15f;
        }
        currentSpeed = originalSpeed;
    }

	void Update () {
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 32);
        if (!attacking && !stunned && !dying)
        {
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
                if (currentSpeed == originalSpeed)
                {
                    currentSpeed += 0.025f;
                }
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
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(waypoint.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position = Vector2.MoveTowards(transform.position, waypoint, originalSpeed);   
        }

        if (alert && pc)
        {
            if (pc.transform.position.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (pc.transform.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
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

    public IEnumerator ReceiveDamage(float amount, float stunDuration)
    {
        health -= amount;
        stunned = true;
        attacking = false;
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
        if (health <= 0)
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
        aggro = attacking = stunned = alert = rest = false;
        dying = true;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    void Chase()
    {
        if (!anim.GetBool("Running"))
        {
            anim.SetBool("Running", true);
        }
        if (Vector2.Distance(transform.position, pc.transform.position) < 0.6f && !attacking)
        {
            StartCoroutine(PerformAttack());
        }
        else
        {
            if (pc.transform.position.x > transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (pc.transform.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, pc.transform.position, currentSpeed);
        }
    }

    IEnumerator PerformAttack()
    {
        anim.SetBool("Running", false);
        attacking = true;
        float attackDelay = 0;
        if(enemyType == Enemy.green)
        {
            attackDelay = 0.5f;
        }
        if (enemyType == Enemy.red)
        {
            attackDelay = 0.7f;
        }
        if (enemyType == Enemy.yellow)
        {
            attackDelay = 0.75f;
        }
        Vector2 point = pc.transform.position;
        point.y += 0.2f;
        anim.Play("Attack", 0);
        yield return new WaitForSeconds(0.21f);
        if (!stunned)
        {
            Collider2D col = Physics2D.OverlapCircle(point, 0.25f, 1 << LayerMask.NameToLayer("Player"));
            if(col && col.GetComponent<PCScript>())
            {
                StartCoroutine(pc.ReceiveDamage(damage, 0.3f, this));
            }
        }
        yield return new WaitForSeconds(attackDelay);
        attacking = false;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            waypoint = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.isTrigger)
        {
            if (enemyType == Enemy.yellow)
            {
                aggro = true;
                trigger.radius = 3.25f;
            }
            else
            {
                alert = true;
                waypoint = Vector2.zero;
                anim.SetBool("Running", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.isTrigger)
        {
            if (aggro)
            {
                aggro = false;
                trigger.radius = 1.75f;
            }
            else
            {
                alert = false;
            }
        }
    }
}
