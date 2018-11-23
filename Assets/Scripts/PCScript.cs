﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PCScript : MonoBehaviour {

    [SerializeField] float originalSpeed;
    [SerializeField] GameObject dust;


    public Crafting craftUI;

    public int[] resources = new int[4];
    [HideInInspector] public bool hasSpace = true;

    public GameObject[] inventory;

    float energy = 750, originalDamage = 10, health = 50;

    bool collecting, attacking, attackStun, hitStun, stunned;

    short armType, torsoType, legType;

    Collider2D col;

    Vector3 pos, craftUIPos;

    GameObject killer, dustEffect;


    private void Start()
    {
        craftUI = GetComponentInChildren<Crafting>();
        craftUIPos = craftUI.transform.position;
    }

    void Update()
    {
        if (!collecting && !attackStun && !stunned)
        {
            Move();
            Inputs();
        }
        
        if(energy > 0)
        {
            energy -= Time.deltaTime;
            FindObjectOfType<Slider>().value = energy;
        }
    }

    void Inputs()
    {
        if (!craftUI || (Vector2)craftUI.transform.localPosition != craftUI.destination)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                col = Physics2D.OverlapCircle(transform.position, 1, 1 << LayerMask.NameToLayer("Interactables"));
                if (col && col.GetComponent<ObjectScript>())
                {
                    collecting = true;
                    col.GetComponent<ObjectScript>().Interact();

                }
            }
            if (Input.GetKeyDown(KeyCode.F) && !attacking)
            {
                StartCoroutine(Attack());
            }

        }
        if (Input.GetKeyDown(KeyCode.C)){
            ToggleCrafting();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            legType++;
        }
    }

    void Move()
    {
        if (!craftUI || (Vector2)craftUI.transform.localPosition == craftUI.start)
        {
            pos = transform.position;
            float speed = originalSpeed;
            switch (legType)
            {
                case 1:
                    if (speed != originalSpeed + 0.025f)
                    {
                        speed += originalSpeed + 0.025f;
                    }
                    break;
                case 2:
                    if (speed != originalSpeed - 0.025f)
                    {
                        speed -= originalSpeed - 0.025f;
                    }
                    break;
                default:
                    speed = originalSpeed;
                    break;
            }
            pos.x += Input.GetAxis("Horizontal") * speed;
            pos.y += Input.GetAxis("Vertical") * speed;
            if(pos.x != transform.position.x && pos.y != transform.position.y)
            {
                pos = transform.position;
                pos.x += Input.GetAxis("Horizontal") * speed * 0.75f;
                pos.y += Input.GetAxis("Vertical") * speed * 0.75f;
            }
            if (pos.x != transform.position.x || pos.y != transform.position.y)
            {
                if (!dustEffect)
                {
                    Vector2 tempPos = transform.position;
                    tempPos.y += 0.25f;
                    dustEffect = Instantiate(dust, tempPos, Quaternion.identity);
                    dustEffect.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                    dustEffect.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 4;
                    StartCoroutine(DestroyDust());
                }
                if (!GetComponent<Animator>().GetBool("Running"))
                {
                    GetComponent<Animator>().SetBool("Running", true);
                   
                    foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
                        {
                            child.GetComponent<Animator>().SetBool("Running", true);
                        }
                    }
                }
                if (pos.x < transform.position.x)
                {
                    if (GetComponent<SpriteRenderer>().flipX)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                        {
                            if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs" || child.gameObject.name == "Shadow")
                            {
                                child.flipX = false;
                            }
                        }
                    }
                }
                else if(pos.x > transform.position.x)
                {
                    if (!GetComponent<SpriteRenderer>().flipX)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                        {
                            if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs" || child.gameObject.name == "Shadow")
                            {
                                child.flipX = true;
                            }
                        }
                    }
                }
            }
            else if (GetComponent<Animator>().GetBool("Running"))
            {
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    GetComponent<Animator>().SetBool("Running", false);
                    foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
                        {
                            child.GetComponent<Animator>().SetBool("Running", false);
                        }
                    }
                }
            }
            transform.position = pos;
            GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 32);
            foreach(SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
            {
                if(child.gameObject.name == "Arms")
                {
                    child.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
                }
                else if(child.gameObject.name == "Torso")
                {
                    child.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 2;
                }
                else if (child.gameObject.name == "Legs")
                {
                    child.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 3;
                }
                else if (child.gameObject.name == "Shadow")
                {
                    child.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 5;
                }
            }
        }
    }

    void ToggleCrafting() {
        if (craftUI)
        {
            GetComponent<Animator>().SetBool("Running", false);
            foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
            {
                if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
                {
                    child.GetComponent<Animator>().SetBool("Running", false);
                }
            }
            craftUI.Toggle();
        }
    }

    public void ToggleCraftUI()
    {
        if (craftUI)
        {
            Debug.Log(2);
            craftUI = null;
        }
        else
        {
            Debug.Log(1);
            craftUI = FindObjectOfType<Crafting>();
            craftUI.transform.localPosition = craftUIPos;
        }
    }

    void Equip()
    {

    }

    IEnumerator Attack()
    {
        attacking = true;
        attackStun = true;
        float damage = originalDamage;
        float stunDuration = 0.3f;
        float attackDelay = 0.75f;
        switch (armType)
        {
            case 1:
                damage += -5;
                stunDuration = 0.65f;
                break;
            case 2:
                damage += 15;
                attackDelay = 1f;
                break;
            default:
                GetComponent<Animator>().Play("Attack1", 0);
                foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                {
                    if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
                    {
                        child.GetComponent<Animator>().Play("Attack1", 0);
                    }
                }
                damage = originalDamage;
                break;
        }
        yield return new WaitForSeconds(0.1f);
        Vector2 point = transform.position;
        point.y += 0.5f;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            point.x += 0.5f;
        }
        else
        {
            point.x -= 0.5f;
        }
        col = Physics2D.OverlapCircle(point, 0.25f, 1 << LayerMask.NameToLayer("Enemies"));
        if (col)
        {
            StartCoroutine(col.GetComponent<EnemyScript>().ReceiveDamage(damage, stunDuration));
           
        }
        yield return new WaitForSeconds(0.15f);
        attackStun = false;
        yield return new WaitForSeconds(attackDelay);
        attacking = false;
    }

    public IEnumerator ReceiveDamage(float amount, float stunDuration, EnemyScript enemy)
    {
        health -= amount;
        stunned = true;
        attacking = false;
        GetComponent<Animator>().Play("Stunned", 0);
        if (health <= 0)
        {
            StartCoroutine(KillMe());
        }
        yield return new WaitForSeconds(stunDuration);
        killer = enemy.gameObject;
        stunned = false;
        
    }

    IEnumerator KillMe()
    {

        yield return new WaitForSeconds(1f);
        FindObjectOfType<Follow>().target = killer.transform;
        Destroy(gameObject);
    }

    public IEnumerator AddToInventory(string item, int amount)
    {
        GetComponent<Animator>().SetBool("Running", false);
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
            {
                child.GetComponent<Animator>().SetBool("Running", false);
            }
        }
        yield return new WaitForSeconds(1.25f);
        switch(item)
        {
            case "Gears":
                resources[0] += amount;
                Destroy(col.gameObject);
                col = null;
                break;
            case "Red Arms":
                break;
        }
        collecting = false;
    }

    IEnumerator DestroyDust()
    {
        yield return new WaitForSeconds(0.417f);
        GameObject effect = dustEffect;
        dustEffect = null;
        Destroy(effect);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Interactables")
        {
            pos = transform.position;
        }
    }
}
