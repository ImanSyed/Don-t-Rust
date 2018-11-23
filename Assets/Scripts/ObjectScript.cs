using UnityEngine;

public class ObjectScript : MonoBehaviour {

    public enum ObjectType
    {
        none, gears
    }

    public ObjectType myType;

    PCScript pc;

    [SerializeField] int amount = 1;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
        GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 32);
    }

    public void Interact()
    {
        if (myType == ObjectType.gears)
        {
            StartCoroutine(pc.AddToInventory("Gears", amount));
        }
    }
}
