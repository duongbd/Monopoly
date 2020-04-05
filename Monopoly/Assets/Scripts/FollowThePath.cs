using UnityEngine;
using System;

public class FollowThePath : MonoBehaviour {
    private float moveSpeed = 5f;

    public int target = 0;

    public bool moveAllowed = false;

    public GameObject[] path;

    // Use this for initialization
    private void Start ()
    { 
        path = Board.instance().blocks;
        transform.position = path[0].transform.position;
    }

    // Update is called once per frame
    private void Update () { 
        if (moveAllowed)
            Move();
	}

    private void Move()
    {
        if (target >= 40)
        {
            target = target % 40;
        }
        transform.position = Vector2.MoveTowards(
            transform.position, 
            path[target].transform.position, 
            moveSpeed
            );
        
        if (Vector3.Distance(transform.position, path[target].transform.position) < 0.5)
        {
            target += 1;
        }
    }

    public void setSpeed(float speed)
    {
        moveSpeed = speed;
    }
}
