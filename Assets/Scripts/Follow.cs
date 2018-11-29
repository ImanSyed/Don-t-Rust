using UnityEngine;

public class Follow : MonoBehaviour
{

	public Transform target;
	public int shakeMagnitude;
	public int roughness;
	public int numberOfShakes;


	bool shaking = false;
	float shakeX=0, shakeY=0;
	float tarX=0, tarY=0;
	int countdown=0;
	int shakesLeft=0;
	[SerializeField] Vector2 minPlane, maxPlane;

	void Update ()
	{
		if (target) {
           
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

			if (Input.GetKeyDown (KeyCode.F)) {
				shaking = true;
				shakesLeft = numberOfShakes;
				countdown = roughness;


			}
			if (shaking) {
				tarX = pos.x + shakeMagnitude / 10f;
                tarY = pos.y + shakeMagnitude / 10f;

            }
            else {
				tarX = pos.x;
                tarY = pos.y;

			}

			if (countdown > 0) countdown--;
			if (countdown <= 0) {
				if (shakesLeft > 0) {
					shakesLeft--;
					countdown = roughness;
					tarX = -tarX;
					tarY = -tarY;
				} else {
					countdown = 0;
					shaking = false;
				}
			}

			if (shakeX == 0 && shakeY == 0)
				shaking = false;

            transform.position = new Vector3(transform.position.x + 0.01f * (tarX - transform.position.x), transform.position.y + 0.01f * (tarY - transform.position.y), pos.z);

            if (shakeX <= 0)
				shakeX = 0;
			else
				shakeX -= shakeMagnitude / 100f;
			
			if (shakeY <= 0)
				shakeY = 0;
			else
				shakeY -= shakeMagnitude / 100f;
			
		}
	}
}
