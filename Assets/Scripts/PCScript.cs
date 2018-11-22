using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PCScript : MonoBehaviour {

    [SerializeField] float originalSpeed;
    Crafting craftUI;

    public int[] resources = new int[4];
    [HideInInspector] public bool hasSpace = true;

    public GameObject[] inventory;

    float energy = 750, originalDamage = 10;

    bool collecting, attacking;

    short armType, torsoType, legType;

    Collider2D col;

    [SerializeField] Collider2D attackTrigger;

    private void Start()
    {
        craftUI = GetComponentInChildren<Crafting>();
    }

    void Update()
    {
        if (!collecting && !attacking)
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
        if ((Vector2)craftUI.transform.localPosition != craftUI.destination)
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

        if ((Vector2)craftUI.transform.localPosition == craftUI.start)
        {
            Vector3 pos = transform.position;
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
                    GetComponent<SpriteRenderer>().flipX = false;
                    foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs" || child.gameObject.name == "Shadow")
                        {
                            child.flipX = false;
                        }
                    }
                }
                else if(pos.x > transform.position.x)
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
                    child.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 4;
                }
            }
        }
    }

    void ToggleCrafting() {
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

    void Equip()
    {

    }

    IEnumerator Attack()
    {
        attacking = true;
        float damage = originalDamage;
        float delay = 0.5f;
        switch (armType)
        {
            case 1:
                damage += -5;
                delay = 0.2f;
                break;
            case 2:
                damage += 15;
                delay = 1f;
                break;
            default:
                damage = originalSpeed;
                break;
        }
        yield return new WaitForSeconds(delay);
        Vector2 point = transform.position;
        if (GetComponent<SpriteRenderer>().flipX)
        {
            point.x -= 1;
        }
        else
        {
            point.x += 1;
        }
        col = Physics2D.OverlapBox(point, Vector2.one, 1 << LayerMask.NameToLayer("Enemies"));
        if (col && col.GetComponent<EnemyScript>())
        {
            col.GetComponent<EnemyScript>().ReceiveDamage(originalDamage);
            Debug.Log(1);
        }
        attacking = false;

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
}
