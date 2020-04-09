using UnityEngine;
using System;

public class FollowThePath : MonoBehaviour {
    private float moveSpeed = 5f;

    private bool moveBack = false;

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
        if (target < 0)
        {
            target = target + 40;
        }
        transform.position = Vector2.MoveTowards(
            transform.position, 
            path[target].transform.position, 
            moveSpeed
            );
        
        if (Vector3.Distance(transform.position, path[target].transform.position) < 0.5)
        {
            if (!moveBack) target += 1;
            else target -= 1;
        }
    }

    public void setSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void setDirection(bool backward)
    {
        if (moveBack != backward)
        {
            if (backward) target -= 2;
            else target += 2;
        }
        moveBack = backward;
    }

    public bool backward()
    {
        return moveBack;
    }
}
