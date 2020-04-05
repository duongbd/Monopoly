using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Buyable
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
            if (count == 2)
            {
                return GameController.rollValue * 10;
            }
            if (count == 1)
            {
                return GameController.rollValue * 4;
            }
        }
        return 0;
    }
}
