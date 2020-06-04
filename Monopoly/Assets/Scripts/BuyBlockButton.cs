using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BuyBlockButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(buyCurrentBlock);
    }

    // Update is called once per frame
    void Update()
    {
        Block block = Board.instance().getPlayerInTurnBlock();
        if (GameController.rolled && block.GetComponent<Buyable>() != null && block.GetComponent<Buyable>().getOwner() == null && !GameController.turnLock)
        {
            GetComponent<Button>().interactable = true;
        } else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void buyCurrentBlock()
    {
        if (GameController.waitModal == false)
        {
            Buyable block = Board.instance().getPlayerInTurnBlock().GetComponent<Buyable>();
            Player player = GameController.playerInTurn();
            if (player.getFund() < block.price)
            {
                if (player.calNetWorth() < block.price)
                {
                    Modal.instance().showModal("Bạn không đủ tiền mua <color=" + block.color + "><b>" + block.blockName + "</b></color>", "OK", () => { });
                }
                else
                {
                    Modal.instance().showModal("Bạn không đủ tiền mặt để mua <color=" + block.color + "><b>" + block.blockName + "</b></color>. Bạn có thể bán tài sản để mua.", "OK", () => { });
                }
            }
            else
            {
                Modal.instance().showModal("Bạn có chắc muốn mua <color=" + block.color + "><b>" + block.blockName + "</b></color> với giá <color=#aa0115><b>" + block.price + "Đ</b></color> không?", "Có", "Không",
                    () => {
                        player.buy(block);
                    },
                    () => { }
                    );
            }
        }
    }
}
