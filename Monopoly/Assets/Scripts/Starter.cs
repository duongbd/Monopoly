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
        Modal.instance().showModal("Bạn đi qua ô Khởi Hành nhận được 200Đ", "OK", () => {
            GameController.playerInTurn().addFund(200);
            if (GameController.playerInTurn().position != 0) {
                Board.instance().getPlayerInTurnBlock().activate();
            }
        });
    }
}
