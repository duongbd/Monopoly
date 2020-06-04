using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prison : Block
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void activate()
    {
        Modal.instance().showModal("<size=150%><b>" + blockName + "</b></size>", "OK", () => {});
    }
}
