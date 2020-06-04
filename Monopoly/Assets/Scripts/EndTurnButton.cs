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
                iTween.PunchRotation(GameObject.Find("RollButton"), iTween.Hash("z", 20, "time", 1));
                iTween.PunchScale(GameObject.Find("RollButton"), iTween.Hash("x", 1.2, "y", 1.2, "time", 1));
            }
            else
            {
                if (GameController.playerInTurn().inDept)
                {
                    Modal.instance().showModal("Bạn chưa trả nợ, hãy trả nợ để tiếp tục!", "OK", () => { });
                } else
                {
                    Modal.instance().showModal("Bạn có chắc muốn kết thúc lượt", "Có", "Không",
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
