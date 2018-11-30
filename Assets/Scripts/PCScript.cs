using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PCScript : MonoBehaviour {

    [SerializeField] float originalSpeed;
    [SerializeField] GameObject dustE, attackE, hitEffect, projectile, gate;
    [SerializeField] IconSwitch icon1, icon2, icon3;
    [SerializeField] SpriteRenderer C;
    [SerializeField] Sprite endGameSprite;
    public TMPro.TextMeshPro redCount, blueCount, yellowCount;

    RuntimeAnimatorController d0, a0;

    public Crafting craftUI;

    public int[,] resources = new int[3, 4];

    public float originalDamage = 10, health = 100;

    bool collecting, attacking, attackStun, hitStun, stunned, dying;

    public short armsType, torsoType, legsType, attackCounter, lockCounter;

    Collider2D col;

    Vector3 pos;

    GameObject killer, dustEffect;

    public bool canCraft;

    private void Start()
    {
        d0 = dustE.GetComponent<Animator>().runtimeAnimatorController;
        craftUI = FindObjectOfType<Crafting>();

    }

    void Update()
    {
        if (!collecting && !attackStun && !stunned && !dying)
        {
            Move();
            Inputs();
        }
        else if (!dying && legsType == 3)
        {
            Move();
        }
    }

    void Inputs()
    {
        if (!craftUI.isActiveAndEnabled || !craftUI.activateControls)
        {
            if (Input.GetKeyDown(KeyCode.F) && !attacking)
            {
                StartCoroutine(Attack());
            }
            if(Input.GetKeyDown(KeyCode.Alpha1) && armsType != 0)
            {
                armsType++;
                if (armsType > 3)
                {
                    armsType = 1;
                }
                while (resources[armsType - 1, 1] == 0)
                {
                    armsType++;
                    if (armsType > 3)
                    {
                        armsType = 1;
                    }
                }
                icon1.Check(armsType);
                Equip();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && torsoType != 0)
            {
                torsoType++;
                if (torsoType > 3)
                {
                    torsoType = 1;
                }
                while (resources[torsoType - 1, 2] == 0)
                {
                    torsoType++;
                    if (torsoType > 3)
                    {
                        torsoType = 1;
                    }
                }
                icon2.Check(torsoType);
                Equip();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && legsType != 0)
            {
                legsType++;
                if (legsType > 3)
                {
                    legsType = 1;
                }
                while (resources[legsType - 1, 3] == 0)
                {
                    legsType++;
                    if (legsType > 3)
                    {
                        legsType = 1;
                    }
                }
                icon3.Check(legsType);
                Equip();
            }
        }
        if (Input.GetKeyDown(KeyCode.C)){
            ToggleCrafting();
        }

    }
   
    void Move()
    {
        if (!craftUI.isActiveAndEnabled || !craftUI.activateControls)
        {
            pos = transform.position;
            float speed = originalSpeed;
            switch (legsType)
            {
                case 1:
                    if (speed != originalSpeed + 0.025f)
                    {
                        speed = originalSpeed + 0.025f;
                    }
                    break;
                case 2:
                    if (speed != originalSpeed + 0.005f)
                    {
                        speed = originalSpeed + 0.005f;
                    }
                    break;
                case 3:
                    if (speed != originalSpeed - 0.005f)
                    {
                        speed = originalSpeed - 0.005f;
                        Debug.Log(1);
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
                    dustEffect = Instantiate(dustE, tempPos, Quaternion.identity);
                    dustEffect.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
                    dustEffect.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 4;
                    StartCoroutine(DestroyEffect(dustEffect));
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
        if (canCraft)
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


    public void Equip()
    {
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            if (child.gameObject.name == "Arms")
            {
                switch (armsType)
                {
                    case 1:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 1);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                    case 2:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 1);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                    case 3:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 1);
                        break;
                    default:
                        child.GetComponent<Animator>().SetLayerWeight(1, 1);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;

                }
            }
            else if (child.gameObject.name == "Torso")
            {
                switch (torsoType)
                {
                    case 1:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 1);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                    case 2:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 1);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                    case 3:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 1);
                        break;
                    default:
                        child.GetComponent<Animator>().SetLayerWeight(1, 1);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;

                }
            }
            else if (child.gameObject.name == "Legs")
            {
                switch (legsType)
                {
                    case 1:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 1);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                    case 2:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 1);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                    case 3:
                        child.GetComponent<Animator>().SetLayerWeight(1, 0);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 1);
                        break;
                    default:
                        child.GetComponent<Animator>().SetLayerWeight(1, 1);
                        child.GetComponent<Animator>().SetLayerWeight(2, 0);
                        child.GetComponent<Animator>().SetLayerWeight(3, 0);
                        child.GetComponent<Animator>().SetLayerWeight(4, 0);
                        break;
                }
            }
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        attackStun = true;
        float damage = originalDamage;
        float stunDuration = 0.3f;
        GetComponent<Animator>().Play("Attack");
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            if (child.gameObject.name == "Arms")
            {
                GameObject tempEffect = attackE;
                switch (armsType)
                {
                    case 1:
                        child.GetComponent<Animator>().Play("Attack", 2, 0);
                        tempEffect = Instantiate(attackE, transform.position, transform.rotation);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(0, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(1, 1);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(2, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(3, 0);
                        StartCoroutine(DestroyEffect(tempEffect));
                        break;
                    case 2:
                        child.GetComponent<Animator>().Play("Attack", 3, 0);
                        tempEffect = Instantiate(attackE, transform.position, transform.rotation);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(0, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(1, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(2, 1);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(3, 0);
                        StartCoroutine(DestroyEffect(tempEffect));
                        break;
                    case 3:
                        tempEffect = Instantiate(attackE, transform.position, transform.rotation);
                        child.GetComponent<Animator>().Play("Attack", 4, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(0, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(1, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(2, 0);
                        tempEffect.GetComponent<Animator>().SetLayerWeight(3, 1);
                        StartCoroutine(DestroyEffect(tempEffect));
                        stunDuration = 0.75f;
                        break;
                    default:
                        child.GetComponent<Animator>().Play("Attack");
                        break;
                }

                if (GetComponent<SpriteRenderer>().flipX)
                {
                    tempEffect.GetComponent<SpriteRenderer>().flipX = true;
                }
                else
                {
                    tempEffect.GetComponent<SpriteRenderer>().flipX = false;
                }
                tempEffect.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;

            }
            if (child.gameObject.name == "Torso")
            {
                switch (torsoType)
                {
                    case 1:
                        child.GetComponent<Animator>().Play("Attack", 2, 0);
                        break;
                    case 2:
                        child.GetComponent<Animator>().Play("Attack", 3, 0);
                        break;
                    case 3:
                        child.GetComponent<Animator>().Play("Attack", 4, 0);
                        break;
                    default:
                        child.GetComponent<Animator>().Play("Attack");
                        break;
                }
            }
            if (child.gameObject.name == "Legs")
            {
                switch (legsType)
                {
                    case 1:
                        child.GetComponent<Animator>().Play("Attack", 2, 0);
                        break;
                    case 2:
                        child.GetComponent<Animator>().Play("Attack", 3, 0);
                        break;
                    case 3:
                        child.GetComponent<Animator>().Play("Attack", 4, 0);
                        break;
                    default:
                        child.GetComponent<Animator>().Play("Attack");
                        break;
                }
            }
        }
        yield return new WaitForSeconds(0.1f);
        if (armsType == 2)
        {
            Vector2 point = transform.position;
            point.y += 0.5f;
            if (GetComponent<SpriteRenderer>().flipX)
            {
                point.x += 0.5f;
                GameObject b = Instantiate(projectile, point, Quaternion.identity);
                b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 3f);
            }
            else
            {
                point.x -= 0.5f;
                GameObject b = Instantiate(projectile, point, Quaternion.identity);
                b.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 3f);
            }
            yield return new WaitForSeconds(0.5f);
            attackStun = false;
        }
        else
        {
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
                GameObject tempEffect = Instantiate(hitEffect, col.gameObject.transform.position, Quaternion.identity);
                Vector2 ypos = tempEffect.transform.position;
                ypos.y += 0.5f;
                tempEffect.transform.position = ypos;
                StartCoroutine(DestroyEffect(tempEffect));
                StartCoroutine(col.GetComponent<EnemyScript>().ReceiveDamage(damage, stunDuration));

            }
            yield return new WaitForSeconds(0.4f);
            attackStun = false;
        }
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    public IEnumerator ReceiveDamage(float amount, float stunDuration, EnemyScript enemy)
    {
        health -= amount;
        stunned = true;
        attacking = false;
        GetComponent<Animator>().Play("Hit");
        foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
        {
            if (child.gameObject.name == "Arms")
            {
                switch (torsoType)
                {
                    case 1:
                        child.GetComponent<Animator>().Play("Hit", 2, 0);
                        break;
                    case 2:
                        child.GetComponent<Animator>().Play("Hit", 3, 0);
                        break;
                    case 3:
                        child.GetComponent<Animator>().Play("Hit", 4, 0);
                        break;
                    default:
                        child.GetComponent<Animator>().Play("Hit");
                        break;
                }
                if (child.gameObject.name == "Torso")
                {
                    switch (torsoType)
                    {
                        case 1:
                            child.GetComponent<Animator>().Play("Hit", 2, 0);
                            break;
                        case 2:
                            child.GetComponent<Animator>().Play("Hit", 3, 0);
                            break;
                        case 3:
                            child.GetComponent<Animator>().Play("Hit", 4, 0);
                            break;
                        default:
                            child.GetComponent<Animator>().Play("Hit");
                            break;
                    }
                }
                if (child.gameObject.name == "Legs")
                {
                    switch (legsType)
                    {
                        case 1:
                            child.GetComponent<Animator>().Play("Hit", 2, 0);
                            break;
                        case 2:
                            child.GetComponent<Animator>().Play("Hit", 3, 0);
                            break;
                        case 3:
                            child.GetComponent<Animator>().Play("Hit", 4, 0);
                            break;
                        default:
                            child.GetComponent<Animator>().Play("Hit");
                            break;
                    }
                }
            }
        }
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
        dying = true;
        yield return new WaitForSeconds(1f);
        FindObjectOfType<Follow>().target = killer.transform;
        Destroy(gameObject);
    }

    public void AddToInventory(string item, int amount)
    {
        switch(item)
        {
            case "Red":
                resources[0, 0] += amount;
                redCount.text = resources[0, 0].ToString();
                health += 5;
                break;
            case "Blue":
                resources[1, 0] += amount;
                blueCount.text = resources[1, 0].ToString();
                health += 5;
                break;
            case "Yellow":
                resources[2, 0] += amount;
                health += 5;
                yellowCount.text = resources[2, 0].ToString();
                break;
            case "Red Arms":
                resources[0, 1] += amount;
                armsType = 1;
                icon1.Check(armsType);
                break;
            case "Blue Arms":
                resources[1, 1] += amount;
                armsType = 2;
                icon1.Check(armsType);
                break;
            case "Yellow Arms":
                resources[2, 1] += amount;
                armsType = 3;
                icon1.Check(armsType);
                break;
            case "Red Torso":
                torsoType = 1;
                resources[0, 2] += amount;
                icon2.Check(torsoType);
                break;
            case "Blue Torso":
                torsoType = 2;
                icon2.Check(torsoType);
                resources[1, 2] += amount;
                break;
            case "Yellow Torso":
                torsoType = 3;
                icon2.Check(torsoType);
                resources[2, 2] += amount;
                break;
            case "Red Legs":
                legsType = 1;
                icon3.Check(legsType);
                resources[0, 3] += amount;
                break;
            case "Blue Legs":
                resources[1, 3] += amount;
                legsType = 2;
                icon3.Check(legsType);
                break;
            case "Yellow Legs":
                resources[2, 3] += amount;
                legsType = 3;
                icon3.Check(legsType);
                break;
        }
        Equip();

        collecting = false;
    }

    IEnumerator DestroyEffect(GameObject e)
    {
        yield return new WaitForSeconds(0.417f);
        GameObject eff = e;
        Destroy(eff);
    }

    public void LockOpen()
    {
        lockCounter++;
        if(lockCounter >= 3)
        {
            gate.GetComponent<SpriteRenderer>().sprite = endGameSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 10 || collision.gameObject.layer == 12)
        { 
        }
        if(collision.gameObject.layer == 11)
        {
            Destroy(collision.gameObject);
            StartCoroutine(ReceiveDamage(10, 0.25f, FindObjectOfType<EnemyScript>()));
        }
        if(collision.gameObject.tag == "Finish" && lockCounter >= 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.CompareTag("Craft"))
        {
            canCraft = true;
            C.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Craft"))
        {
            canCraft = false;
            C.enabled = false;
        }
    }
}