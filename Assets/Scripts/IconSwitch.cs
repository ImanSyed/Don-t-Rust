using UnityEngine;
using UnityEngine.UI;


public class IconSwitch : MonoBehaviour {

    [SerializeField] Sprite[] spritesUnlocked;



    public void Check(short c)
    {
        GetComponent<Image>().sprite = spritesUnlocked[(short)(c - 1)];
    }
}
