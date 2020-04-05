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
        if (GameController.rolled && block.GetComponent<Buyable>() != null && block.GetComponent<Buyable>().getOwner() == null)
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
                    Modal.instance().showModal("Bạn không đủ tiền mua " + block.blockName, "OK", () => { });
                }
                else
                {
                    Modal.instance().showModal("Bạn không đủ tiền mặt để mua " + block.blockName + ". Bạn có thể bán tài sản để mua.", "OK", () => { });
                }
            }
            else
            {
                Modal.instance().showModal("Bạn có chắc muốn mua " + block.blockName + " với giá " + block.price + "Đ không?", "Có", "Không",
                    () => {
                        player.buy(block);
                    },
                    () => { }
                    );
            }
        }
    }
}
