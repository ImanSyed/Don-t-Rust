using UnityEngine;

public class Crafting : MonoBehaviour {

    short moveDirection, trayMoveDirection;

    [HideInInspector] public Vector2 start, destination;

    [SerializeField] GameObject[] parentIcons;

    Transform[] childIcons;
    Transform[] temp = new Transform[3];


    GameObject activeTray;


    short counter1, counter2;

    private void Start()
    {
        start = transform.localPosition;
        destination = start;
        destination.y -= 2f;
        activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
        childIcons = activeTray.GetComponentsInChildren<Transform>();
        temp[0] = childIcons[1];
        temp[1] = childIcons[2];
        temp[2] = childIcons[3];
        childIcons = temp;
    }

    private void Update()
    {
        if (moveDirection == 1)
        {
            if((Vector2)transform.localPosition == destination)
            {
                moveDirection = 0;
                activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
                activeTray.GetComponent<Animator>().Play("Forward", 0);
            }
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, destination, 0.1f);
        }
        else if(moveDirection == -1)
        {
            if ((Vector2)transform.localPosition == start)
            {
                moveDirection = 0;
            }
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, start, 0.1f);
        }

        if ((Vector2)transform.localPosition == destination)
        {
            Controls();
        }
    }

    void Controls()
    {
        if (activeTray.transform.localPosition.x == -1.35f || activeTray.transform.localPosition.x == 2.65f)
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                activeTray = parentIcons[counter1].transform.GetChild(0).gameObject;
                activeTray.GetComponent<Animator>().Play("Back", 0);
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

            if (activeTray.transform.localPosition.x == 2.65f)
            {
                if(childIcons[counter2].GetComponent<SpriteRenderer>().color != Color.red)
                {
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.red;

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
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.red;
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
                    childIcons[counter2].GetComponent<SpriteRenderer>().color = Color.red;
                    childIcons[counter2].GetComponent<IconScript>().Check();
                }
            }
        }
    }

    public void Toggle()
    {
        if((Vector2)transform.localPosition == destination)
        {
            moveDirection = -1;            
        }
        else if((Vector2)transform.localPosition == start)
        {
            childIcons[counter2].GetComponent<IconScript>().Check();
            moveDirection = 1;
        }
    }
}
