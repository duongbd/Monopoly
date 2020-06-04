using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortune : Block
{
    public override void activate()
    {
        int rand = Random.Range(0, 13);
        switch (rand)
        {
            case 0:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nBạn khai trương nhà hát, thu của mỗi người chơi <color=#216C2A><b>50Đ</b></color> tiền vé.", "OK", () => {
                    GameController.getPlayersNotInTurn().ForEach((player) => {
                        player.pay(GameController.playerInTurn(), 50, "Tiền vé");
                    });
                });
                break;

            case 1:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nTrả viện phí <color=#aa0115><b>100Đ</b></color>.", "OK", () => {
                    if (GameController.playerInTurn().getFund() < 100)
                    {
                        if (GameController.playerInTurn().calNetWorth() < 100)
                        {
                            GameController.playerInTurn().declareBankrupt();
                        }
                        else
                        {
                            GameController.playerInTurn().debit(100);
                            Modal.instance().showModal("Bạn không đủ <color=#aa0115><b>" + 100 + "Đ</b></color>. Cần bán tài sản để trả!", "OK", () => { });
                        }
                    }
                    else
                    {
                        GameController.playerInTurn().pay(100);
                    }
                });
                break;

            case 2:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nBán cổ phiếu được <color=#216C2A><b>45Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(45);
                });
                break;

            case 3:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nNhận tiền bảo hiểm nhân thọ <color=#216C2A><b>100Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(100);
                });
                break;

            case 4:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nThu phí dịch vụ, nhận <color=#216C2A><b>25Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(25);
                });
                break;

            case 5:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nTiền thừa kế <color=#216C2A><b>100Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(100);
                });
                break;

            case 6:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nĐạt giải Á quân hoa hậu, thưởng <color=#216C2A><b>10Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(10);
                });
                break;

            case 7:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nDo sai sót của ngân hàng, nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(200);
                });
                break;

            case 8:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nĐến hạn thu ngân sách Giáng sinh, nhận <color=#216C2A><b>100Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(100);
                });
                break;

            case 9:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nHoàn trả thuế thu nhập, nhận <color=#216C2A><b>20Đ</b></color>.", "OK", () => {
                    GameController.playerInTurn().addFund(20);
                });
                break;

            case 10:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\n<b>Vào tù</b>!", "OK", () => {
                    Board.instance().getBlock(30).activate();
                });
                break;

            case 11:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nVề <color=#22c4ff><b>Khởi hành</b></color> nhận <color=#216C2A><b>200Đ</b></color>.", "OK", () => {
                    GameController.jumpPlayerInTurn(0);
                    Board.instance().getPlayerInTurnBlock().activate();
                });
                break;

            case 12:
                Modal.instance().showModal("<size=150%><color=#f83d3d><b>Khí Vận:</b></color></size>\nPhí khám bệnh, trả <color=#aa0115><b>50Đ</b></color>.", "OK", () => {
                    if (GameController.playerInTurn().getFund() < 50)
                    {
                        if (GameController.playerInTurn().calNetWorth() < 50)
                        {
                            GameController.playerInTurn().declareBankrupt();
                        }
                        else
                        {
                            GameController.playerInTurn().debit(50);
                            Modal.instance().showModal("Bạn không đủ <color=#aa0115><b>" + 50 + "Đ</b></color>. Cần bán tài sản để trả!", "OK", () => { });
                        }
                    }
                    else
                    {
                        GameController.playerInTurn().pay(50);
                    }
                });
                break;
        }
    }
}
