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
        string text = "Trả nợ: \n";
        int sum = 0;
        player.getDebts().ForEach((debt) => {
            if (debt.creditor != null) {
                creditors.Add(debt.creditor);
                text += "- " + debt.creditor.playerName + ": " + debt.amount + "Đ";
            }
            else text += "- " + debt.amount + "Đ";
            if (debt.note != null) {
                text += " (" + debt.note + ")";
            }
            text += "\n";
            sum += debt.amount;
        });
        text += "Tổng: " + sum + "Đ. Bạn có muốn trả không?";

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
                Modal.instance().showModal("Bạn không đủ " + sum + "Đ trả nợ. Cần bán tài sản để tiếp tục!", "OK", () => { });
            }
        }
        else
        {
            Modal.instance().showModal(text, "Có", "Không",
                () => {
                    player.payDebts();
                },
                () => { }
                );
        }
    }
}
