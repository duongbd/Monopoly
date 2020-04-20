using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    private TextMeshProUGUI playerName;
    private TextMeshProUGUI fund;
    private TextMeshProUGUI netWorth;
    private GameObject bankrupt;

    private void Awake()
    {
        playerName = GameObject.Find(gameObject.name + "/Name").GetComponent<TextMeshProUGUI>();
        fund = GameObject.Find(gameObject.name + "/Fund").GetComponent<TextMeshProUGUI>();
        netWorth = GameObject.Find(gameObject.name + "/NetWorth").GetComponent<TextMeshProUGUI>();
        bankrupt = GameObject.Find(gameObject.name + "/Bankrupt");
        bankrupt.SetActive(false);
    }

    public void setName(string name)
    {
        playerName.SetText(name);
    }

    public void setFund(int value)
    {
        fund.SetText(value + "Đ");
    }

    public void setNetWorth(int value)
    {
        netWorth.SetText(value + "Đ");
    }

    public void updatePanel(string name, int fundValue, int netWorthValue)
    {
        setName(name);
        setFund(fundValue);
        setNetWorth(netWorthValue);
    }

    public void updatePanel(int fundValue, int netWorthValue)
    {
        setFund(fundValue);
        setNetWorth(netWorthValue);
    }

    public void updateBankruptStatus()
    {
        bankrupt.SetActive(true);
    }
}
