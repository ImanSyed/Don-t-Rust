using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform target;

    [SerializeField] Vector2 minPlane, maxPlane;

    GameObject craftUI;

    void Update()
    {
        if (target)
        {
            if (target.position.x > minPlane.x && target.position.x < maxPlane.x && target.position.y > minPlane.y && target.position.y < maxPlane.y)
            {
                if (craftUI)
                {
                    craftUI.transform.SetParent(target.GetComponent<PCScript>().transform);
                    craftUI = null;
                    target.GetComponent<PCScript>().ToggleCraftUI();
                }
                
                Vector3 pos = transform.position;
                pos.x = target.position.x;
                pos.y = target.position.y;
                transform.position = pos;
            }
            else
            {
                if (!craftUI)
                {
                    craftUI = target.GetComponent<PCScript>().craftUI.gameObject;
                    craftUI.transform.SetParent(null);
                    target.GetComponent<PCScript>().ToggleCraftUI();

                }
            }
        }
    }
}
