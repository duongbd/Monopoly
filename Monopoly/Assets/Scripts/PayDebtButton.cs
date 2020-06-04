using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PayDebtButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(payDebt);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.playerInTurn().inDept)
        {
            GetComponent<Button>().interactable = true;
        } else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void payDebt()
    {
        if (GameController.waitModal)
        {
            return;
        }
        Player player = GameController.playerInTurn();
        List<Player> creditors = new List<Player>();
        string text = "<size=150%><b>Trả nợ</b></size>\n";
        int sum = 0;
        player.getDebts().ForEach((debt) => {
            if (debt.creditor != null) {
                creditors.Add(debt.creditor);
                text += "- <b>" + debt.creditor.playerName + "</b>: <i>" + debt.amount + "Đ</i>";
            }
            else text += "- <i>" + debt.amount + "Đ</i>";
            if (debt.note != null) {
                text += " (" + debt.note + ")";
            }
            text += "\n";
            sum += debt.amount;
        });
        text += "Tổng: <color=#aa0115><b>" + sum + "Đ</b></color>. ";

        if (player.getFund() < sum)
        {
            if (player.calNetWorth() < sum)
            {
                if (creditors.Count == 0) player.declareBankrupt();
                if (creditors.Count == 1) player.declareBankrupt(creditors[0]);
                if (creditors.Count > 1) player.declareBankrupt(creditors);
            }
            else
            {
                Modal.instance().showModal(text + "<i>Bạn không đủ tiền trả nợ. Cần bán tài sản để tiếp tục!</i>", "OK", () => { });
            }
        }
        else
        {
            Modal.instance().showModal(text + "Bạn có muốn trả không?", "Có", "Không",
                () => {
                    player.payDebts();
                },
                () => { }
                );
        }
    }
}
