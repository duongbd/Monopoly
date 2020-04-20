using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TradeButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(trade);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.tradingMode)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/blue button");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/button");
        }
        if (GameController.turnLock)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    private void trade()
    {
        if (GameController.waitModal == false)
        {
            if (GameController.tradingMode == false) GameController.tradingModeOn();
            else GameController.tradingModeOff();
        }
    }
}
