using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buyable : Block
{
    private Player owner;
    private SpriteRenderer ownerStamp;
    public string color;
    public int price;
    
    public abstract int calRent();

    private void Awake()
    {
        switch (gameObject.tag)
        {
            case "Brown":
                color = "#603c2d";
                break;
            case "Ocean":
                color = "#59aaa9";
                break;
            case "Pink":
                color = "#ff83d2";
                break;
            case "Orange":
                color = "#fe8306";
                break;
            case "Red":
                color = "#cc2c2f";
                break;
            case "Banana":
                color = "#d5e024";
                break;
            case "Green":
                color = "#669d1d";
                break;
            case "Sky":
                color = "#328bcb";
                break;
            default:
                color = "#00d8ff";
                break;
        }
    }

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
            Modal.instance().showModal("<size=150%><color=" + color + "><b>" + blockName + "</b></color></size>\nBạn có thể mua ô này.", "OK", () => { });
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
                        Modal.instance().showModal("<size=150%><color=" + color + "><b>" + blockName + "</b></color></size>\nBạn đã đi vào ô của <b>" + owner.playerName + "</b>. Bạn không đủ <color=#aa0115><b>" + rent + "Đ</b></color> thuê. Cần bán tài sản và trả nợ để tiếp tục!", "OK", () => { });
                    }
                }
                else
                {
                    Modal.instance().showModal("<size=150%><color=" + color + "><b>" + blockName + "</b></color></size>\nBạn đã đi vào ô của <b>" + owner.playerName + "</b> phải trả <color=#aa0115><b>" + rent + "Đ</b></color>", "OK",
                        () => {
                            player.pay(owner, rent);
                        }
                        );
                }
            } else
            {
                Modal.instance().showModal("<size=150%><color=" + color + "><b>" + blockName + "</b></color></size>\nĐây là ô của bạn, bạn có thể mua/bán tài sản hoặc KT lượt.", "OK", () => { });
            }
        }
    }

    public void addOwnerStamp()
    {
        if (owner != null)
        {
            ownerStamp.sprite = Resources.Load<Sprite>("Player/small " + GameController.getPlayerRepresentative(owner.represent));
            iTween.PunchScale(ownerStamp.gameObject, iTween.Hash("x", 1.5, "y", 1.5, "time", 1));
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
