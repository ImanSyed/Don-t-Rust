using UnityEngine;
using UnityEngine.UI;


public class IconSwitch : MonoBehaviour {

    [SerializeField] Sprite[] spritesUnlocked;
    [SerializeField] int partType;

    short c;


    public void Swap()
    {
        if(c >= spritesUnlocked.Length)
        {
            c = 0;
        }
        if (FindObjectOfType<PCScript>().resources[c, partType] > 0)
        {
            GetComponent<Image>().sprite = spritesUnlocked[c];
           
        }
        c++;
    }

    public void Check()
    {
        if (c < 0)
        {
            c = (short)(spritesUnlocked.Length - 1);
        }
        if (FindObjectOfType<PCScript>().resources[c, partType] > 0)
        {
            GetComponent<Image>().sprite = spritesUnlocked[c];
        }
    }
}
