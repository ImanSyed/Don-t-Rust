using UnityEngine;

public class ObjectScript : MonoBehaviour {

    public enum ObjectType
    {
        gears
    }

    public ObjectType myType;

    PCScript pc;

    [SerializeField] int amount = 1;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
    }

    public void Interact()
    {
        if (myType == ObjectType.gears)
        {
            StartCoroutine(pc.AddToInventory("Gears", amount));
        }
    }
}
