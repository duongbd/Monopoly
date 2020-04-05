using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(endTurn);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.turnLock)
        {
            GetComponent<Button>().interactable = false;
        } else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    private void endTurn()
    {
        if (GameController.waitModal == false) {
            if (GameController.rolled == false || GameController.activated == false)
            {
                Modal.instance().showModal("Bạn chưa đổ xúc xắc!!", "OK", () => { });
            }
            else
            {
                if (GameController.playerInTurn().inDept)
                {
                    Modal.instance().showModal("Bạn chưa trả nợ, hãy trả nợ để tiếp tục!", "OK", () => { });
                } else
                {
                    Modal.instance().showModal("Bạn có chắc muốn kết thực lượt", "Có", "Không",
                        () => {
                            GameController.endTurn();
                        },
                        () => { }
                        );
                }
            }
        }
    }
}
