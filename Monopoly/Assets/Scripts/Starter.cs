using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starter : Block
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
        Modal.instance().showModal("<size=150%><color=#22c4ff><b>Khởi Hành</b></color></size>\nNhận được <color=#216C2A><b>200Đ</b></color>", "OK", () => {
            GameController.playerInTurn().addFund(200);
            if (GameController.playerInTurn().position != 0) {
                Board.instance().getPlayerInTurnBlock().activate();
            }
        });
    }
}
