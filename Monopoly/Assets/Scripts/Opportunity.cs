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
        int rand = Random.Range(0,14);
        switch (rand)
        {
            case 0:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nLùi <b>3 bước</b>.", "OK", () => {
                    GameController.movePlayerInTurn(3, true);
                    steps = 0;
                    needActivation = true;
                });
                break;

            case 1:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nBạn được bầu làm chủ tịch hội đồng quản trị, đưa cho mỗi người chơi <color=#aa0115><b>50Đ</b></color>.", "OK", () => {
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
                            Modal.instance().showModal("Bạn không đủ <color=#aa0115><b>" + sum + "Đ</b></color>. Cần bán tài sản để trả!", "OK", () => { });
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
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐến nhà máy gần nhất. Nếu qua <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    steps = 0;
                    while (Board.instance().getPlayerInTurnBlock().GetComponent<Factory>() == null) {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 3:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\n<b>Vào tù</b>!", "OK", () => {
                    Board.instance().getBlock(30).activate();
                });
                break;

            case 4:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐến <color=#00d8ff><b>Ga Hà Nội</b></color>. Nếu đi qua <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
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
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐến <color=#ff83d2><b>Quảng trường Ba Đình</b></color>. Nếu đi qua <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
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
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nVề <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    GameController.jumpPlayerInTurn(0);
                    steps = 0;
                    needActivation = true;
                });
                break;

            case 7:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐến <color=#328bcb><b>Phú Quốc</b></color>.", "OK", () => {
                    steps = 0;
                    while (GameController.playerInTurn().position != 39)
                    {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 8:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐến hạn thu tiền thuê nhà, nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(200);
                });
                break;

            case 9:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐên <color=#cc2c2f><b>Hội An</b></color>. Nếu đi qua <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    steps = 0;
                    while (GameController.playerInTurn().position != 24)
                    {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 10:
                int fee = 0;
                Board.instance().getBlockOwnedByPlayer(GameController.playerInTurn()).ForEach((block) =>
                {
                    if (block.GetComponent<Buildable>() != null)
                    {
                        int properties = block.GetComponent<Buildable>().getProperties();
                        if (properties < 5)
                        {
                            fee += 25 * properties;
                        }
                        else
                        {
                            fee += 200;
                        }
                    }
                });
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nBảo trì nhà: với mỗi nhà trả <b>25Đ</b>, mỗi khách sạn trả <b>100Đ</b>. Tổng: <color=#aa0115><b>" + fee + "Đ</b></color>", "OK", () => {
                    if (GameController.playerInTurn().getFund() < fee)
                    {
                        if (GameController.playerInTurn().calNetWorth() < fee)
                        {
                            GameController.playerInTurn().declareBankrupt();
                        }
                        else
                        {
                            GameController.playerInTurn().debit(fee);
                            Modal.instance().showModal("Bạn không đủ <color=#aa0115><b>" + fee + "Đ</b></color>. Cần bán tài sản để trả!", "OK", () => { });
                        }
                    }
                    else
                    {
                        GameController.playerInTurn().pay(fee);
                    }
                });
                break;

            case 11:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nĐến nhà ga gần nhất. Nếu qua <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    steps = 0;
                    while (Board.instance().getPlayerInTurnBlock().GetComponent<Station>() == null)
                    {
                        GameController.movePlayerInTurn(1);
                        steps++;
                    }
                    needActivation = true;
                });
                break;

            case 12:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nNộp thuế <i>Đỗ Nghèo Khỉ</i> <color=#aa0115><b>15Đ</b></color>.", "OK", () => {
                    if (GameController.playerInTurn().getFund() < 15)
                    {
                        if (GameController.playerInTurn().calNetWorth() < 15)
                        {
                            GameController.playerInTurn().declareBankrupt();
                        }
                        else
                        {
                            GameController.playerInTurn().debit(15);
                            Modal.instance().showModal("Bạn không đủ <color=#aa0115><b>" + 15 + "Đ</b></color>. Cần bán tài sản để trả!", "OK", () => { });
                        }
                    }
                    else
                    {
                        GameController.playerInTurn().pay(15);
                    }
                });
                break;

            case 13:
                Modal.instance().showModal("<size=150%><color=#523d7b><b>Cơ Hội:</b></color></size>\nNhận tiền lãi ngân hàng <color=#216C2A><b>50Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(50);
                });
                break;
        }
    }
}
