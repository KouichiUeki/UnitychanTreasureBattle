using UnityEngine;
using System.Collections;

public class GoalBall : MonoBehaviour {

    public float rotspeedX;
    public float rotspeedY;
    public float rotspeedZ;

	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotspeedX, rotspeedY, rotspeedZ);
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
