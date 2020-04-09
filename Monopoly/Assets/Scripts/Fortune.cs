using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortune : Block
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
        int rand = 0;
        switch (rand)
        {
            case 0:
                Modal.instance().showModal("Khí Vận:\nBạn khai trương nhà hát, thu của mỗi người chơi 50Đ tiền vé.", "OK", () => {
                    GameController.getPlayersNotInTurn().ForEach((player) => {
                        player.pay(GameController.playerInTurn(), 50, "Tiền vé");
                    });
                });
                break;
        }
    }
}
