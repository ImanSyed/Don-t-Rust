using UnityEngine;
using System.Collections;

public class IconScript: MonoBehaviour{
    public string item;
    public bool available;
    public int[] requirements = new int[3];
    [SerializeField] Sprite av, unav;
    PCScript pc;
    [SerializeField] GameObject effect;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
    }

    private void Update()
    {
        if(available && Input.GetKeyDown(KeyCode.Space))
        {
            GameObject e = Instantiate(effect, transform.position, Quaternion.identity);
            StartCoroutine(Craft());
        }
    }

    public void Check()
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (pc.resources[i, 0] >= requirements[i])
            {
                available = true;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = av;
            }
            else
            {
                available = false;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = unav;
                return;
            }
        }
    }

    public void Display()
    {
        for (int i = 0; i < requirements.Length; i++)
        {
            if (pc.resources[i, 0] >= requirements[i])
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = av;
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = unav;
                return;
            }
        }
    }

    IEnumerator Craft()
    {
        available = false;
        short c = 0;
        while (c < requirements.Length)
        {
            pc.resources[c, 0] -= requirements[c];
            c++;
        }
        pc.redCount.text = pc.resources[0, 0].ToString();
        pc.blueCount.text = pc.resources[1, 0].ToString();
        pc.yellowCount.text = pc.resources[2, 0].ToString();

        yield return new WaitForSeconds(1);
        foreach(IconScript icon in FindObjectsOfType<IconScript>())
        {
            icon.Display();
        }
        pc.AddToInventory(item, 1);
        Check();
    }

}
