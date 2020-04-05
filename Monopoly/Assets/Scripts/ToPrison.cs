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
        Modal.instance().showModal("Bạn đã bị bắt vào tù, bạn có thể ra tù sau 3 lượt hoặc đổ được xúc xắc 2 mặt giống nhau, hoặc trả 50Đ ngay bây giờ để không phải vào tù. Bạn có muốn trả không?", "Có", "Không",
            () => {
                if (player.getFund() < 50)
                {
                    Modal.instance().showModal("Bạn không có đủ 50Đ tiền mặt!", "OK",
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
