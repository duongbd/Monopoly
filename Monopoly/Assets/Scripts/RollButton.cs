using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RollButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(roll);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.rolled == true)
        {
            GetComponent<Button>().interactable = false;
        } else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    private void roll()
    {
        if (GameController.waitModal == false)
        {
            if (GameController.playerInTurn().inDept)
            {
                Modal.instance().showModal("Bạn phải trả nợ trước khi đổ xúc xắc!", "OK", () => { });
            } else
            {
                if (GameController.tradingMode) GameController.tradingModeOff();
                GameController.rollTheDice();
            }
        }
    }
}
