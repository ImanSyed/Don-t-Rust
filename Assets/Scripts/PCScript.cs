using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PCScript : MonoBehaviour {

    [SerializeField] float originalSpeed;
    Crafting craftUI;

    public int[] resources = new int[4];
    [HideInInspector] public bool hasSpace = true;

    public GameObject[] inventory;

    float energy = 750;

    bool collecting;

    short armType, torsoType, legType;

    Collider2D obj;

    private void Start()
    {
        craftUI = GetComponentInChildren<Crafting>();
    }

    void Update()
    {
        if (!collecting)
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
                
                obj = Physics2D.OverlapCircle(transform.position, 1, 1 << LayerMask.NameToLayer("Interactable"));
                if (obj && obj.GetComponent<ObjectScript>())
                {
                    collecting = true;
                    obj.GetComponent<ObjectScript>().Interact();

                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {

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
                        if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
                        {
                            child.flipX = false;
                        }
                    }
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    foreach (SpriteRenderer child in GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (child.gameObject.name == "Arms" || child.gameObject.name == "Torso" || child.gameObject.name == "Legs")
                        {
                            child.flipX = true;
                        }
                    }
                }
            }
            else if (GetComponent<Animator>().GetBool("Running"))
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
                Destroy(obj.gameObject);
                obj = null;
                break;
            case "Red Arms":
                break;
        }
        collecting = false;
    }
}
