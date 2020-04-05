using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tax : Block
{
    public int value;

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
        Player player = GameController.playerInTurn();
        if (player.getFund() < value)
        {
            if (player.calNetWorth() < value)
            {
                player.declareBankrupt();
            }
            else
            {
                player.debit(value, "Tiền thuế");
                Modal.instance().showModal("Bạn không đủ " + value + "Đ trả thuế. Cần bán tài sản và trả nợ để tiếp tục!", "OK", () => { });
            }
        }
        else
        {
            Modal.instance().showModal("Ban phải trả thuế " + value + "Đ", "OK",
                () =>
                {
                    player.pay(value);
                }
                );
        }
    }
}
