using UnityEngine;
using System.Collections;

public class IconScript: MonoBehaviour{
    public string item;
    public bool available;
    public int[] requirements = new int[3];

    PCScript pc;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
    }

    private void Update()
    {
        if(available && Input.GetKeyDown(KeyCode.Space))
        {
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
            }
            else
            {
                available = false;
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
        pc.AddToInventory(item, 1);
        Check();
    }

}
