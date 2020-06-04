using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingLot : Block
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
        Modal.instance().showModal("<size=150%><b>" + blockName + "</b></size>\nBạn được đỗ xe miễn phí.", "OK", () => { });
    }
}
