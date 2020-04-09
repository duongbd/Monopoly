using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debt
{
    public Player creditor;
    public int amount;
    public string note;
    public Debt(int amount, Player creditor, string note = null)
    {
        this.creditor = creditor;
        this.amount = amount;
        this.note = note;
    }
    public Debt(int amount, string note = null)
    {
        this.creditor = null;
        this.amount = amount;
        this.note = note;
    }
}


public class Player : MonoBehaviour
{
    public bool needUpdate = false;
    public string playerName;
    public int represent;
    public int position = 0;
    public bool bankrupt = false;
    public bool inDept = false;
    public int inPrison = -1;

    private int fund = 1500;
    private List<Debt> debts;

    // Start is called before the first frame update
    void Start()
    {
        debts = new List<Debt>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void move(int steps)
    {
        if (position + steps >= 40)
        {
            position = (position + steps) % 40;
        }
        else
        {
            position = position + steps;
        }
    }

    public void moveBack(int steps)
    {
        if (position - steps < 0)
        {
            position = position - steps + 40;
        }
        else
        {
            position = position - steps;
        }
    }

    public int getFund()
    {
        return fund;
    }

    public void addFund(int amount)
    {
        fund += amount;
        needUpdate = true;
    }

    public void subFund(int amount)
    {
        fund -= amount;
        needUpdate = true;
    }

    public void buy(Buyable block)
    {
        if (block.getOwner() == null)
        {
            if (fund >= block.price)
            {
                block.setOwner(this);
                fund -= block.price;
                needUpdate = true;
            }
        }
    }

    public void pay(Player owner, int rent, string note = null)
    {
        if (fund < rent)
        {
            if (calNetWorth() < rent)
            {
                declareBankrupt(owner);
            }
            else
            {
                debit(rent, owner, note);
            }
        }
        else
        {
            fund -= rent;
            owner.addFund(rent);
            needUpdate = true;
        }
    }

    public void pay(int value, string note = null)
    {
        if (fund < value)
        {
            if (calNetWorth() < value)
            {
                declareBankrupt();
            }
            else
            {
                debit(value, note);
            }
        }
        else
        {
            fund -= value;
            inDept = false;
            needUpdate = true;
        }
    }

    public void debit(int amount, Player creditor, string note = null)
    {
        inDept = true;
        debts.Add(new Debt(amount, creditor, note));
    }

    public void debit(int amount, string note = null)
    {
        inDept = true;
        debts.Add(new Debt(amount, note));
    }

    public void payDebts()
    {
        if (inDept)
        {
            debts.ForEach((debt) => {
                if (debt.creditor != null) pay(debt.creditor, debt.amount);
                else pay(debt.amount);
            });
            debts.Clear();
            inDept = false;
        }
    }

    public List<Debt> getDebts()
    {
        return debts;
    }

    public int calNetWorth()
    {
        int netWorth = fund;
        Board.instance().getBlockOwnedByPlayer(this).ForEach((block) =>
        {
            if (block.GetComponent<Buildable>() != null)
            {
                netWorth += block.price / 2 + block.GetComponent<Buildable>().getProperties() * block.GetComponent<Buildable>().propertyPrice / 2;
            } else
            {
                netWorth += block.price / 2;
            }
        });
        return netWorth;
    }

    public void sell(Buyable block) {
        if (block.getOwner() == this)
        {
            int amount;
            if (block.GetComponent<Buildable>() != null)
            {
                amount = block.price / 2 + block.GetComponent<Buildable>().getProperties() * block.GetComponent<Buildable>().propertyPrice / 2;
                block.GetComponent<Buildable>().setProperties(0);
            }
            else
            {
                amount = block.price / 2;
            }
            addFund(amount);
            block.setOwner(null);
        }
    }

    public void sellProperty(Buildable block)
    {
        if (block.getOwner() == this)
        {
            if (block.getProperties() > 0) {
                block.destroyProperty();
                addFund(block.propertyPrice / 2);
            }
        }
    }

    public void buyProperty(Buildable block)
    {
        if (block.getOwner() == this)
        {
            if (fund >= block.propertyPrice)
            {
                block.buildProperty();
                subFund(block.propertyPrice);
            } 
        }
    }

    public void declareBankrupt(Player beneficiary)
    {
        Modal.instance().showModal(playerName + " đã phá sản do không có đủ khả năng chi trả! Mọi tài sản được chuyển cho " + beneficiary.playerName + ".", "OK", () => {
            Board.instance().getBlockOwnedByPlayer(this).ForEach((block) => {
                block.setOwner(beneficiary);
            });
            beneficiary.addFund(fund);
            fund = 0;
            bankrupt = true;
            needUpdate = true;
            if (GameController.playerInTurn() == this) GameController.endTurn();
        });
    }

    public void declareBankrupt(List<Player> beneficiaries)
    {
        string text = playerName + " đã phá sản do không có đủ khả năng chi trả! Tổng tài sản được chia đều cho:\n";
        beneficiaries.ForEach((beneficiary) => {
            text += "- " + beneficiary.playerName + "\n";
        });
        Modal.instance().showModal(text, "OK", () => {
            Board.instance().getBlockOwnedByPlayer(this).ForEach((block) => {
                sell(block);
            });
            int amount = fund / beneficiaries.Count;
            beneficiaries.ForEach((beneficiary) => {
                beneficiary.addFund(amount);
            });
            fund = 0;
            bankrupt = true;
            needUpdate = true;
            if (GameController.playerInTurn() == this) GameController.endTurn();
        });
    }

    public void declareBankrupt()
    {
        Modal.instance().showModal(playerName + " đã phá sản do không có đủ khả năng chi trả!", "OK", () => {
            bankrupt = true;
            Board.instance().getBlockOwnedByPlayer(this).ForEach((block) => {
                block.setOwner(null);
            });
            fund = 0;
            needUpdate = true;
            if (GameController.playerInTurn() == this) GameController.endTurn();
        });
    }
}