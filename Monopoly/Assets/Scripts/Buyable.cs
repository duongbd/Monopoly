using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buyable : Block
{
    private Player owner;
    private SpriteRenderer ownerStamp;
    public int price;
    
    public abstract int calRent();

    private void Start()
    {
        setOwnerStamp();
        owner = null;
    }

    public string getOwnerName()
    {
        if (owner == null)
        {
            return "Không";
        }
        return owner.playerName;
    }

    public Player getOwner()
    {
        return owner;
    }

    public void setOwner(Player player)
    {
        owner = player;
        addOwnerStamp();
    }

    public override void activate()
    {
        if (getOwner() == null)
        {
            Modal.instance().showModal("Bạn đã đi vào " + blockName + ", không có chủ. Bạn có thể mua ô này.", "OK", () => { });
        }
        else
        {
            Player player = GameController.playerInTurn();
            int rent = calRent();
            if (owner != player)
            {
                if (player.getFund() < rent)
                {
                    if (player.calNetWorth() < rent)
                    {
                        player.declareBankrupt(owner);
                    }
                    else
                    {
                        player.debit(rent, owner, "Tiền thuê");
                        Modal.instance().showModal("Bạn không đủ " + rent + "Đ thuê. Cần bán tài sản và trả nợ để tiếp tục!", "OK", () => { });
                    }
                }
                else
                {
                    Modal.instance().showModal("Ban đã đi vào ô của " + owner.playerName + " phải trả " + rent + "Đ", "OK",
                        () => {
                            player.pay(owner, rent);
                        }
                        );
                }
            } else
            {
                Modal.instance().showModal("Đây là ô của bạn, bạn có thể mua/bán tài sản hoặc KT lượt.", "OK", () => { });
            }
        }
    }

    public void addOwnerStamp()
    {
        if (owner != null)
        {
            ownerStamp.sprite = Resources.Load<Sprite>("Player/small " + GameController.getPlayerRepresentative(owner.represent));
        } else
        {
            ownerStamp.sprite = null;
        }
    }

    public void setOwnerStamp()
    {
        ownerStamp = GameObject.Find(gameObject.name + "/OwnerStamp").GetComponent<SpriteRenderer>();
    }
}
