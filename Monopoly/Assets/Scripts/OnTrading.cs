using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrading : MonoBehaviour
{
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.tradingMode == true && GameController.playerInTurn() == GetComponent<Buyable>().getOwner())
        {
            active = true;
            if (GetComponent<RectTransform>().rect.width == 1.25) GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/active block frame");
            else GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UI/ver active block frame");
            iTween.PunchRotation(gameObject, iTween.Hash("z", 10, "time", 1));
        } else
        {
            active = false;
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    private void OnMouseDown()
    {
        if (GameController.tradingMode == true)
        {
            if (active)
            {
                InfoPanel.instance().showInfo(GetComponent<Block>());
            }
            else
            {
                Modal.instance().showModal("Đây không phải ô của bạn!", "OK", () => { });
            }
        }
    }
}
