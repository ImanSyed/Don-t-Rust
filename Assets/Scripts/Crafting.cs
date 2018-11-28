using UnityEngine;

public class Crafting : MonoBehaviour {

    short moveDirection, trayMoveDirection;

    [HideInInspector] public Vector2 start, destination;

    [SerializeField] GameObject[] parentIcons;

    Transform[] childIcons;
    Transform[] temp = new Transform[3];


    GameObject activeTray;
    public bool activateControls;


    short counter1, counter2;

    private void Start()
    {
        activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
        childIcons = activeTray.GetComponentsInChildren<Transform>();
        temp[0] = childIcons[1];
        temp[1] = childIcons[2];
        temp[2] = childIcons[3];
        childIcons = temp;
    }

    private void Update()
    {
        if (activateControls)
        {
            Controls();
        }
    }

    void Controls()
    {
        if (activeTray.transform.localPosition.x == -0.75f || activeTray.transform.localPosition.x == 0.75f)
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
                childIcons = activeTray.GetComponentsInChildren<Transform>();
                temp[0] = childIcons[1];
                temp[1] = childIcons[2];
                temp[2] = childIcons[3];
                childIcons = temp;
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
                childIcons = activeTray.GetComponentsInChildren<Transform>();
                temp[0] = childIcons[1];
                temp[1] = childIcons[2];
                temp[2] = childIcons[3];
                childIcons = temp;
                activeTray.GetComponent<Animator>().Play("Forward", 0);
            }

            if (activeTray.transform.localPosition.x == 0.75f)
            {
                if(childIcons[counter2].GetComponent<SpriteRenderer>().color != Color.grey)
                {
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.grey;

                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.white;
                    childIcons[counter2].GetComponent<IconScript>().available = false;
                    counter2++;
                    if (counter2 > childIcons.Length - 1)
                    {
                        counter2 = 0;
                    }
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.grey;
                    childIcons[counter2].GetComponent<IconScript>().Check();

                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.white;
                    childIcons[counter2].GetComponent<IconScript>().available = false;
                    counter2--;
                    if (counter2 < 0)
                    {
                        counter2 = (short)(childIcons.Length - 1);
                    }
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.grey;
                    childIcons[counter2].GetComponent<IconScript>().Check();
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
        }
    }
}
