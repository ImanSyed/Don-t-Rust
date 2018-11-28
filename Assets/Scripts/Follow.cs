using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform target;

    [SerializeField] Vector2 minPlane, maxPlane;

    void Update()
    {
        if (target)
        {
           
            Vector3 pos = target.position;
            pos.z = -10;
            /*if (target.position.x > minPlane.x && target.position.x < maxPlane.x)
            {
                pos.x = target.position.x;
            }
            if (target.position.y > minPlane.y && target.position.y < maxPlane.y)
            {
                pos.y = target.position.y;
            }*/
            transform.position = pos;
        }
    }
}
