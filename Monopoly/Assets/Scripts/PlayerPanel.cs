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
    private int curFundValue;

    public float time;
    public iTween.EaseType easeType;

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
        curFundValue = value;
        fund.SetText(value + "<color=#216C2A><b> Đ</b>");
    }

    public void setNetWorth(int value)
    {
        netWorth.SetText(value + "<color=#216C2A><b> Đ</b>");
    }

    public void updatePanel(string name, int fundValue, int netWorthValue)
    {
        setName(name);
        setFund(fundValue);
        setNetWorth(netWorthValue);
    }

    public void updatePanel(int fundValue, int netWorthValue)
    {
        object prms = new object[] { fundValue - curFundValue, fundValue, netWorthValue };
        StartCoroutine("changeFund", prms);
    }

    public void updateBankruptStatus()
    {
        bankrupt.SetActive(true);
        iTween.PunchScale(bankrupt, iTween.Hash("x", 150, "y", 150, "time", 1));
    }

    private IEnumerator changeFund(object[] prms)
    {
        GameObject newGO = new GameObject("Change");
        newGO.transform.SetParent(this.transform);
        TextMeshProUGUI change;
        change = newGO.AddComponent<TextMeshProUGUI>();
        change.transform.position = fund.transform.position + new Vector3(10, 28, 0);
        change.alignment = TextAlignmentOptions.Right;
        change.fontStyle = FontStyles.Bold;
        change.fontSize = 28f;

        int changeVal = (int)prms[0];
        int funVal = (int)prms[1];
        int netWorthVal = (int)prms[2];

        if (changeVal > 0)
        {
            change.SetText("+" + changeVal);
            change.color = new Color32(33, 108, 42, 255);
        }
        else
        {
            change.SetText("-" + changeVal * (-1));
            change.color = new Color32(170, 1, 20, 255);
        }

        iTween.MoveTo(change.gameObject, iTween.Hash("position", fund.transform.position, "easeType", easeType, "time", time));

        yield return new WaitForSeconds(time);
        Destroy(change.gameObject);
        setFund(funVal);
        setNetWorth(netWorthVal);
    }
}
