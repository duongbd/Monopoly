using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPrison : Block
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
        Player player = GameController.playerInTurn();
        Modal.instance().showModal("<size=150%><color=#dd3e42><b>Vào Tù</b></color></size>\nBạn có thể <i>ra tù sau 3 lượt hoặc đổ được xúc xắc 2 mặt giống nhau, hoặc trả <color=#aa0115><b>50Đ</b></color></i> để không phải vào tù.", "Trả <color=#aa0115><b>50Đ</b></color>", "Vào tù",
            () => {
                if (player.getFund() < 50)
                {
                    Modal.instance().showModal("Bạn không có đủ <color=#aa0115><b>50Đ</b></color> tiền mặt!", "OK",
                        () =>
                        {
                            GameController.jumpPlayerInTurn(10);
                            player.inPrison = 3;
                        });
                }
                else
                    player.pay(50);
            },
            () => {
                GameController.jumpPlayerInTurn(10);
                player.inPrison = 3;
            });
    }
}
