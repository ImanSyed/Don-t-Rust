using UnityEngine;

public class Crafting : MonoBehaviour {

    short moveDirection, trayMoveDirection;

    [HideInInspector] public Vector2 start, destination;

    [SerializeField] GameObject[] parentIcons;
    [SerializeField] GameObject cursor;
    IconScript[] childIcons;
    IconScript[] temp = new IconScript[3];

    GameObject activeTray;
    public bool activateControls;

    short counter1, counter2;

    private void Start()
    {
        activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
        childIcons = activeTray.GetComponentsInChildren<IconScript>();
        temp[0] = childIcons[0];
        temp[1] = childIcons[1];
        temp[2] = childIcons[2];
        childIcons = temp;
    }

    private void Update()
    {
        if (activateControls)
        {
            Controls();
        }
        cursor.transform.position = childIcons[counter2].transform.position;
    }

    void Controls()
    {
        if (activeTray.transform.localPosition.x == -1f || activeTray.transform.localPosition.x == 1f)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
                activeTray.GetComponent<Animator>().Play("Back", 0);
                childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.white;
                childIcons[counter2].GetComponent<IconScript>().available = false;
                counter1++;
                if (counter1 > parentIcons.Length - 1)
                {
                    counter1 = 0;
                }
                activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
                childIcons = activeTray.GetComponentsInChildren<IconScript>();
                temp[0] = childIcons[0];
                temp[1] = childIcons[1];
                temp[2] = childIcons[2];
                childIcons = temp;
                childIcons[counter2].GetComponent<IconScript>().Check();
                foreach (IconScript icon in childIcons)
                {
                    icon.Display();
                }
                activeTray.GetComponent<Animator>().Play("Forward", 0);

            }
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
                activeTray.GetComponent<Animator>().Play("Back", 0);
                childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.white;
                childIcons[counter2].GetComponent<IconScript>().available = false;
                counter1--;
                if (counter1 < 0)
                {
                    counter1 = (short)(parentIcons.Length - 1);
                }
                activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
                childIcons = activeTray.GetComponentsInChildren<IconScript>();
                temp[0] = childIcons[0];
                temp[1] = childIcons[1];
                temp[2] = childIcons[2];
                childIcons = temp;
                childIcons[counter2].GetComponent<IconScript>().Check();
                foreach (IconScript icon in childIcons)
                {
                    icon.Display();
                }
                activeTray.GetComponent<Animator>().Play("Forward", 0);
            }

            if (activeTray.transform.localPosition.x == 1f)
            {
                if(cursor.transform.position != childIcons[counter2].transform.position)
                {
                    cursor.transform.position = childIcons[counter2].transform.position;
                    Debug.Log(cursor.transform.position);
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    childIcons[counter2].GetComponent<IconScript>().available = false;
                    counter2++;
                    if (counter2 > childIcons.Length - 1)
                    {
                        counter2 = 0;
                    }
                    cursor.transform.position = childIcons[counter2].transform.position;
                    childIcons[counter2].GetComponent<IconScript>().Check();
                    foreach (IconScript icon in childIcons)
                    {
                        icon.Display();
                    }

                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    childIcons[counter2].GetComponent<IconScript>().available = false;
                    counter2--;
                    if (counter2 < 0)
                    {
                        counter2 = (short)(childIcons.Length - 1);
                    }
                    cursor.transform.position = childIcons[counter2].transform.position;
                    Debug.Log(cursor.transform.position);
                    childIcons[counter2].GetComponent<IconScript>().Check();
                    foreach (IconScript icon in childIcons)
                    {
                        icon.Display();
                    }
                }
            }
        }
    }

    public void Toggle()
    {
        if (activateControls)
        {
            activateControls = false;
            activeTray.GetComponent<Animator>().Play("Back", 0);
        }
        else
        {
            activateControls = true;
            activeTray.GetComponent<Animator>().Play("Forward", 0);
            childIcons[counter2].GetComponent<IconScript>().Check();
            foreach (IconScript icon in childIcons)
            {
                icon.Display();
            }
        }
    }
}
