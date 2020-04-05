using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Buildable : Buyable
{
    public int propertyPrice;
    public int[] rent = new int[6];

    private int properties;
    private TextMeshProUGUI propertyBuilt;

    private void Start()
    {
        setOwnerStamp();
        setOwner(null);
        propertyBuilt = GameObject.Find(gameObject.name + "/PropertyBuilt").GetComponent<TextMeshProUGUI>();
        propertyBuilt.SetText("0");
    }

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
            if (count == blocks.Length && properties == 0)
            {
                return rent[properties] * 2;
            }
            return rent[properties];
        }
        return 0;
    }

    public void buildProperty()
    {
        if (properties < 5)
        {
            properties++;
            propertyBuilt.SetText(properties.ToString());
        }
    }

    public void destroyProperty()
    {
        if (properties > 0)
        {
            properties--;
            propertyBuilt.SetText(properties.ToString());
        }
    }

    public int getProperties()
    {
        return properties;
    }

    public void setProperties(int n)
    {
        properties = n;
        propertyBuilt.SetText(properties.ToString());
    }
}
