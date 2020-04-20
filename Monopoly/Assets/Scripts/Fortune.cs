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
                Modal.instance().showModal("Khí Vận:\nBạn khai trương nhà hát, thu của mỗi người chơi 50Đ tiền vé.", "OK", () => {
                    GameController.getPlayersNotInTurn().ForEach((player) => {
                        player.pay(GameController.playerInTurn(), 50, "Tiền vé");
                    });
                });
                break;

            case 1:
                Modal.instance().showModal("Khí Vận:\nTrả viện phí 100Đ.", "OK", () => {
                    if (GameController.playerInTurn().getFund() < 100)
                    {
                        if (GameController.playerInTurn().calNetWorth() < 100)
                        {
                            GameController.playerInTurn().declareBankrupt();
                        }
                        else
                        {
                            GameController.playerInTurn().debit(100);
                            Modal.instance().showModal("Bạn không đủ " + 100 + "Đ. Cần bán tài sản để trả!", "OK", () => { });
                        }
                    }
                    else
                    {
                        GameController.playerInTurn().pay(100);
                    }
                });
                break;

            case 2:
                Modal.instance().showModal("Khí Vận:\nBán cổ phiếu được 45Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(45);
                });
                break;

            case 3:
                Modal.instance().showModal("Khí Vận:\nNhận tiền bảo hiểm nhân thọ 100Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(100);
                });
                break;

            case 4:
                Modal.instance().showModal("Khí Vận:\nThu phí dịch vụ, nhận 25Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(25);
                });
                break;

            case 5:
                Modal.instance().showModal("Khí Vận:\nTiền thừa kế 100Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(100);
                });
                break;

            case 6:
                Modal.instance().showModal("Khí Vận:\nĐạt giải Á quân hoa hậu, thưởng 10Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(10);
                });
                break;

            case 7:
                Modal.instance().showModal("Khí Vận:\nDo sai sót của ngân hàng, nhận 200Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(200);
                });
                break;

            case 8:
                Modal.instance().showModal("Khí Vận:\nĐến hạn thu ngân sách Giáng sinh, nhận 100Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(100);
                });
                break;

            case 9:
                Modal.instance().showModal("Khí Vận:\nHoàn trả thuế thu nhập, nhận 20Đ.", "OK", () => {
                    GameController.playerInTurn().addFund(20);
                });
                break;

            case 10:
                Modal.instance().showModal("Khí Vận:\nVào tù!", "OK", () => {
                    Board.instance().getBlock(30).activate();
                });
                break;

            case 11:
                Modal.instance().showModal("Khí vận:\nVề Khởi hành nhận 200Đ.", "OK", () => {
                    GameController.jumpPlayerInTurn(0);
                    Board.instance().getPlayerInTurnBlock().activate();
                });
                break;

            case 12:
                Modal.instance().showModal("Khí Vận:\nPhí khám bệnh, trả 50Đ.", "OK", () => {
                    if (GameController.playerInTurn().getFund() < 50)
                    {
                        if (GameController.playerInTurn().calNetWorth() < 50)
                        {
                            GameController.playerInTurn().declareBankrupt();
                        }
                        else
                        {
                            GameController.playerInTurn().debit(50);
                            Modal.instance().showModal("Bạn không đủ " + 50 + "Đ. Cần bán tài sản để trả!", "OK", () => { });
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
