using UnityEngine;
using System.Collections;

public class IconScript: MonoBehaviour{
    public string item;
    public bool available;
    public int[] requirements = new int[5];

    PCScript pc;

    private void Start()
    {
        pc = FindObjectOfType<PCScript>();
    }

    private void Update()
    {
        if(available && Input.GetKeyDown(KeyCode.Space) && pc.hasSpace)
        {
            StartCoroutine(Craft());
        }
    }

    public void Check()
    {
        for (int i = 0; i < requirements.Length - 1; i++)
        {
            if (pc.resources[i] >= requirements[i])
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
        yield return new WaitForSeconds(1);
        short c = 0;
        while(c < pc.resources.Length - 1)
        {
            pc.resources[c] -= requirements[c];
            c++;
        }
        pc.AddToInventory(item, 1);
        Check();
    }

}
