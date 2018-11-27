using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public enum Enemy
    {
        red, blue, yellow
    }

    public Enemy enemyType;

    bool aggro, rest, alert, attacking, stunned, dying;

    float originalSpeed, health, damage, currentSpeed;

    PCScript pc;
    Animator anim;
    SpawnScript mySpawner;

    public bool spawning = true;

    [SerializeField] CircleCollider2D trigger;
    [SerializeField] GameObject deathEffect, scrap1, scrap2, scrap3;

    Vector2 waypoint;

	void Start () {
        anim = GetComponent<Animator>();
        pc = FindObjectOfType<PCScript>();
        if(enemyType == Enemy.blue)
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

    public void Initialize(SpawnScript ss)
    {
        mySpawner = ss;
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
        anim.Play("Stun", 0);
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
        if (health <= 0)
        {
            StartCoroutine(KillMe());
        }
        else if(!aggro)
        {
            aggro = true;
            trigger.radius = 3.5f;
        }
        
    }

    IEnumerator KillMe()
    {
        //GameObject de = Instantiate(deathEffect, transform.position, transform.rotation);
        aggro = attacking = stunned = alert = rest = false;
        dying = true;
        yield return new WaitForSeconds(1f);
        //Destroy(de);
        GameObject s1 = null, s2 = null, s3 = null;
        float r1 = Random.Range(0f, 1f);
        if(r1 > 0.1f && r1 <= 0.5f)
        {
            float r2 = Random.Range(0, 1);
            if(r2 >= 0 && r2 < 0.33f)
            {
                s1 = Instantiate(scrap1, transform.position, transform.rotation);
            }
            else if (r2 >= 0.33f && r2 < 0.66f)
            {
                s1 = Instantiate(scrap2, transform.position, transform.rotation);
            }
            else
            {
                s1 = Instantiate(scrap3, transform.position, transform.rotation);
            }
        }
        else if (r1 > 0.5f && r1 < 0.85f)
        {
            float r2 = Random.Range(0, 1);
            if (r2 >= 0 && r2 < 0.33f)
            {
                s1 = Instantiate(scrap1, transform.position, transform.rotation);
                s2 = Instantiate(scrap2, transform.position, transform.rotation);

            }
            else if (r2 >= 0.33f && r2 < 0.66f)
            {
                s1 = Instantiate(scrap2, transform.position, transform.rotation);
                s2 = Instantiate(scrap3, transform.position, transform.rotation);
            }
            else
            {
                s1 = Instantiate(scrap1, transform.position, transform.rotation);
                s2 = Instantiate(scrap3, transform.position, transform.rotation);
            }
        }
        else if(r1 >= 0.85f && r1 <= 1f)
        {
            s1 = Instantiate(scrap1, transform.position, transform.rotation);
            s2 = Instantiate(scrap2, transform.position, transform.rotation);
            s3 = Instantiate(scrap3, transform.position, transform.rotation);
        }
        if (s1 != null)
        {
            Vector2 pos = transform.position;
            pos.x += Random.Range(-0.3f, 0.3f);
            pos.y += Random.Range(-0.3f, 0.3f);
            s1.transform.position = pos;
            if (s2 != null)
            {
                pos.x += Random.Range(-0.3f, 0.3f);
                pos.y += Random.Range(-0.3f, 0.3f);
                s2.transform.position = pos;
                if (s3 != null)
                {
                    pos.x += Random.Range(-0.3f, 0.3f);
                    pos.y += Random.Range(-0.3f, 0.3f);
                    s3.transform.position = pos;
                }
            }
        }
        mySpawner.count++;
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
        if(enemyType == Enemy.blue)
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
        if (collision.gameObject.layer == 10 || (!spawning && collision.gameObject.tag == "Enemy") )
        {
            waypoint = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        }
        if(spawning && collision.gameObject.tag == "Enemy")
        {
            mySpawner.count++;
            Destroy(gameObject);
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
