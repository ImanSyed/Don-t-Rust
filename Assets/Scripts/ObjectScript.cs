using UnityEngine;

public class ObjectScript : MonoBehaviour {

    public enum ObjectType
    {
        none, red, blue, yellow
    }

    public ObjectType myType;

    PCScript pc;

    [SerializeField] int amount = 1;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 32);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.isTrigger)
        {
            if (myType == ObjectType.red)
            {
                pc.AddToInventory("Red", amount);
                Destroy(gameObject);
            }
            else if (myType == ObjectType.blue)
            {
                pc.AddToInventory("Blue", amount);
                Destroy(gameObject);
                
            }
            else if (myType == ObjectType.yellow)
            {
                pc.AddToInventory("Yellow", amount);
                Destroy(gameObject);

            }
        }
    }
}
