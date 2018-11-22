using UnityEngine;

public class Follow : MonoBehaviour {

    [SerializeField] Transform target;


	void Update () {
        Vector3 pos = transform.position;
        pos.x = target.position.x;
        pos.y = target.position.y;
        transform.position = pos;
	}
}
