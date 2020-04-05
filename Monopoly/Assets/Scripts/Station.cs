using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : Buyable
{
    // Update is called once per frame
    void Update()
    {
        
    }

    public override int calRent()
    {
        if (getOwner() != null)
        {
            string type = gameObject.tag;
            int count = 0;
            GameObject[] blocks = GameObject.FindGameObjectsWithTag(type);
            foreach (GameObject block in blocks)
            {
                if (block.GetComponent<Buyable>().getOwner() == getOwner())
                {
                    count++;
                }
            }
            if (count > 0)
            {
                return 25 * (int)Mathf.Pow(2, count - 1);
            }
        }
        return 0;
    }
}
