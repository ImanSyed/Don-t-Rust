using UnityEngine;

public class Follow : MonoBehaviour {

    public Transform target;


    void Update()
    {
        if (target)
        {
            Vector3 pos = transform.position;
            pos.x = target.position.x;
            pos.y = target.position.y;
            transform.position = pos;
        }
    }
}
