using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    private static GameObject infoPanel;
    private GameObject detail;
    private GameObject info;
    private GameObject properties;
    private GameObject activeFrame;
    private SpriteRenderer card;
    private TextMeshProUGUI owner;
    private TextMeshProUGUI rent;
    private GameObject[] propertiesImages;
    private Button buyButton;
    private Button sellButton;
    private Button sellBlockButton;

    public static InfoPanel instance()
    {
        return infoPanel.GetComponent<InfoPanel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        infoPanel = GameObject.Find("InfoPanel");
        card = GameObject.Find("Card").GetComponent<SpriteRenderer>();
        detail = GameObject.Find("Detail");
        owner = GameObject.Find("Owner").GetComponent<TextMeshProUGUI>();
        rent = GameObject.Find("Rent").GetComponent<TextMeshProUGUI>();
        propertiesImages = GameObject.FindGameObjectsWithTag("Property");
        Array.Sort(propertiesImages, (GameObject a, GameObject b) => {
            return a.transform.GetSiblingIndex() - b.transform.GetSiblingIndex();
        });
        info = GameObject.Find("Info");
        properties = GameObject.Find("Properties");
        buyButton = GameObject.Find("BuyButton").GetComponent<Button>();
        sellButton = GameObject.Find("SellButton").GetComponent<Button>();
        sellBlockButton = GameObject.Find("SellBlockButton").GetComponent<Button>();
        activeFrame = transform.Find("TradeActiveFrame").gameObject;
        activeFrame.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void showInfo(Block block)
    {
        Buyable buyable = block.GetComponent<Buyable>();
        Buildable buildable = block.GetComponent<Buildable>();
        card.sprite = block.card;
        if (buyable != null)
        {
            owner.SetText(buyable.getOwnerName());
            rent.SetText(buyable.calRent() + "Đ");
            detail.SetActive(true);
        } else
        {
            detail.SetActive(false);
        }
        if (buildable != null)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i < buildable.getProperties()) propertiesImages[i].SetActive(true);
                else propertiesImages[i].SetActive(false);
            }
            info.transform.localPosition = new Vector3(-1.86f, 0.3f, 0);
            properties.SetActive(true);
        } else
        {
            info.transform.localPosition = new Vector3(0, 0.3f, 0);
            properties.SetActive(false);
        }
        if (GameController.tradingMode)
        {
            if (buyable != null)
            {
                sellBlockButton.onClick.RemoveAllListeners();
                sellBlockButton.onClick.AddListener(() => {
                    sellBlockEvent(buyable);
                });
                sellBlockButton.gameObject.SetActive(true);
            }
            if (buildable != null)
            {
                buyButton.onClick.RemoveAllListeners();
                sellButton.onClick.RemoveAllListeners();
                if (buildable.getProperties() > 0)
                {
                    sellButton.onClick.AddListener(() => {
                        sellPropertyEvent(buildable);
                    });
                    sellButton.interactable = true;
                } else
                {
                    sellButton.interactable = false;
                }
                if (buildable.getProperties() < 5)
                {
                    buyButton.onClick.AddListener(() => {
                        buyPropertyEvent(buildable);
                    });
                    buyButton.interactable = true;
                } else
                {
                    buyButton.interactable = false;
                }
                sellButton.gameObject.SetActive(true);
                buyButton.gameObject.SetActive(true);
            }
            
            activeFrame.SetActive(true);
        }
        else
        {
            sellButton.gameObject.SetActive(false);
            buyButton.gameObject.SetActive(false);
            sellBlockButton.gameObject.SetActive(false);
            activeFrame.SetActive(false);
        }
        infoPanel.SetActive(true);
    }

    public void closeInfo()
    {
        infoPanel.SetActive(false);
        card.sprite = null;
    }

    private void sellBlockEvent(Buyable block)
    {
        if (GameController.waitModal)
        {
            return;
        }
        int amount;
        if (block.GetComponent<Buildable>() != null)
        {
            amount = block.price / 2 + block.GetComponent<Buildable>().getProperties() * block.GetComponent<Buildable>().propertyPrice / 2;
        }
        else
        {
            amount = block.price / 2;
        }
        Modal.instance().showModal("Bạn có chắc muốn bán <color=" + block.color + "><b>" + block.blockName + "</b></color> không? Bạn sẽ nhận được <color=#216C2A><b>" + amount + "Đ</b></color>.", "Có", "Không",
            () =>
            {
                GameController.playerInTurn().sell(block);
                closeInfo();
            },
            () => { }
            );
    }

    private void sellPropertyEvent(Buildable block)
    {
        if (GameController.waitModal)
        {
            return;
        }
        string text;
        if (block.getProperties() < 5)
        {
            text = "Bạn có chắc muốn bán nhà trên <color=" + block.color + "><b>" + block.blockName + "</b></color> với giá <color=#216C2A><b>" + block.propertyPrice / 2 + "Đ</b></color> không?";
        }
        else
        {
            text = "Bạn có chắc muốn bán khách sạn trên <color=" + block.color + "><b>" + block.blockName + "</b></color> với giá <color=#216C2A><b>" + block.propertyPrice / 2 + "Đ</b></color> không?";
        }
        Modal.instance().showModal(text, "Có", "Không",
            () =>
            {
                GameController.playerInTurn().sellProperty(block);
                showInfo(block);
            },
            () => { }
            );
    }

    private void buyPropertyEvent(Buildable block)
    {
        if (GameController.waitModal)
        {
            return;
        }
        Player player = GameController.playerInTurn();
        if (player.getFund() >= block.propertyPrice)
        {
            string text;
            if (block.getProperties() < 4)
            {
                text = "Bạn có chắc muốn mua nhà trên <color=" + block.color + "><b>" + block.blockName + "</b></color> với giá <color=#aa0115><b>" + block.propertyPrice + "Đ</b></color> không?";
            }
            else
            {
                text = "Bạn có chắc muốn mua khách sạn trên <color=" + block.color + "><b>" + block.blockName + "</b></color> với giá <color=#aa0115><b>" + block.propertyPrice + "Đ</b></color> không?";
            }
            Modal.instance().showModal(text, "Có", "Không",
                () =>
                {
                    player.buyProperty(block);
                    showInfo(block);
                },
                () => { }
                );
        }
        else
        {
            Modal.instance().showModal("Bạn không đủ tiền để mua tài sản!", "OK", () => { });
        }
    }
}
