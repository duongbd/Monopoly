using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opportunity : Block
{
    private bool needActivation = false;
    private int steps = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.playerInTurnActor().moveAllowed == false && needActivation == true)
        {
            if (GameController.playerInTurn().position - steps < 0) Board.instance().getBlock(0).activate();
            else Board.instance().getPlayerInTurnBlock().activate();
            needActivation = false;
        }
    }

    public override void activate()
    {
        int rand = Random.Range(0,7);
        //int rand = 6;
        switch (rand)
        {
            case 0:
                Modal.instance().showModal("Cơ Hội:\nLùi 3 bước.", "OK", () => {
                    GameController.movePlayerInTurn(3, true);
                    steps = 0;
                    needActivation = true;
                });
                break;

            case 1:
                Modal.instance().showModal("Cơ Hội:\nBạn được bầu làm chủ tịch hội đồng quản trị, đưa cho mỗi người chơi 50Đ", "OK", () => {
                    List<Player> otherPlayers = GameController.getPlayersNotInTurn();
                    Player playerInTurn = GameController.playerInTurn();
                    int sum = 50 * otherPlayers.Count;
                    if (playerInTurn.getFund() < sum)
                    {
                        if (playerInTurn.calNetWorth() < sum)
                        {
                            playerInTurn.declareBankrupt(otherPlayers);
                        }
                        else
                        {
                            otherPlayers.ForEach((player) => {
                                playerInTurn.debit(50, player);
                            });
                            Modal.instance().showModal("Bạn không đủ " + sum + "Đ. Cần bán tài sản để trả!", "OK", () => { });
                        }
                    }
                    else
                    {
                        otherPlayers.ForEach((player) => {
                            playerInTurn.pay(player, 50);
                        });
                    }
                });
                break;

            case 2:
                Modal.instance().showModal("Cơ Hội:\nĐến nhà máy gần nhất. Nếu qua Khởi hành nhận 200Đ.", "OK", () => {
                    steps = 0;
                    while (Board.instance().getPlayerInTurnBlock().GetComponent<Factory>() == null) {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 3:
                Modal.instance().showModal("Cơ Hội:\nVào tù!", "OK", () => {
                    Board.instance().getBlock(30).activate();
                });
                break;

            case 4:
                Modal.instance().showModal("Cơ Hội:\nĐến ga Hà Nội. Nếu đi qua Khởi hành nhận 200Đ", "OK", () => {
                    steps = 0;
                    while (GameController.playerInTurn().position != 5)
                    {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 5:
                Modal.instance().showModal("Cơ Hội:\nĐến Quảng trường Ba Đình. Nếu đi qua Khởi hành nhận 200Đ", "OK", () => {
                    steps = 0;
                    while (GameController.playerInTurn().position != 11)
                    {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 6:
                Modal.instance().showModal("Cơ Hội:\nVề Khởi hành nhận 200Đ", "OK", () => {
                    GameController.jumpPlayerInTurn(0);
                    steps = 0;
                    needActivation = true;
                });
                break;
        }
    }
}
