using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameController : MonoBehaviour
{
    private static GameObject[] avatar;
    private static GameObject[] actor;
    private static GameObject turnFrame;
    private static PlayerPanel[] playerPanel;
    private static Player[] player;
    private static Dice[] dice;
    private static int turn = 0;

    public static bool activated = false;
    public static int rollValue = 0;
    public static bool rolled = false;
    public static bool waitModal = false;
    public static bool tradingMode = false;
    public static bool turnLock = false;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player[] {
            GameObject.Find("Player1").GetComponent<Player>(),
            GameObject.Find("Player2").GetComponent<Player>(),
            GameObject.Find("Player3").GetComponent<Player>(),
            GameObject.Find("Player4").GetComponent<Player>()
        };
        avatar = new GameObject[] {
            GameObject.Find("/Avatars/Car"),
            GameObject.Find("/Avatars/Hat"),
            GameObject.Find("/Avatars/Shoe"),
            GameObject.Find("/Avatars/Train")
        };
        actor = new GameObject[] {
            GameObject.Find("/Actors/Car"),
            GameObject.Find("/Actors/Hat"),
            GameObject.Find("/Actors/Shoe"),
            GameObject.Find("/Actors/Train")
        };
        dice = new Dice[] {
            GameObject.Find("Dice1").GetComponent<Dice>(),
            GameObject.Find("Dice2").GetComponent<Dice>()
        };
        playerPanel = new PlayerPanel[] {
            GameObject.Find("PlayerPanel1").GetComponent<PlayerPanel>(),
            GameObject.Find("PlayerPanel2").GetComponent<PlayerPanel>(),
            GameObject.Find("PlayerPanel3").GetComponent<PlayerPanel>(),
            GameObject.Find("PlayerPanel4").GetComponent<PlayerPanel>()
        };
        turnFrame = GameObject.Find("TurnFrame");
        turn = 0;
        for (int i = 0; i < player.Length; i++)
        {
            avatar[player[i].represent].transform.position = playerPanel[i].transform.position + new Vector3(-129,50,0);
            playerPanel[i].updatePanel(player[i].playerName, player[i].getFund(), player[i].calNetWorth());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dice[0].ifCoroutineAllowed() && dice[1].ifCoroutineAllowed() && rolled == false && rollValue > 0)
        {
            rolled = true;
            if (playerInTurn().inPrison > 0)
            {
                activated = true;
                if (dice[0].getDiceSideThrown() == dice[1].getDiceSideThrown())
                {
                    playerInTurn().inPrison = -1;
                    Modal.instance().showModal("Chúc mừng bạn đã được ra tù!", "OK", () => {
                        rolled = false;
                        rollValue = 0;
                        activated = false;
                    });
                } else
                {
                    Modal.instance().showModal("Chúc bạn may mắn lần sau.", "OK", () => {
                    });
                }
            } else
            {
                player[turn].move(rollValue);
                moveActor();
            }
        }
        if (rolled && actor[player[turn].represent].GetComponent<FollowThePath>().moveAllowed == false && activated == false)
        {
            if (player[turn].position - rollValue < 0) Board.instance().getBlock(0).activate();
            else Board.instance().getBlock(player[turn].position).activate();
            activated = true;
        }
        if (actor[player[turn].represent].GetComponent<FollowThePath>().target == player[turn].position + 1)
        {
            actor[player[turn].represent].GetComponent<FollowThePath>().moveAllowed = false;
            turnLock = false;
        }
        for (int i = 0; i<4; i++)
        {
            if (player[i].needUpdate)
            {
                playerPanel[i].updatePanel(player[i].getFund(), player[i].calNetWorth());
                if (player[i].bankrupt)
                {
                    playerPanel[i].updateBankruptStatus();
                    actor[player[i].represent].SetActive(false);
                };
                player[i].needUpdate = false;
            }
        }
    }

    public static void endTurn()
    {
        turn++;
        if (turn >= 4) turn = 0;
        if (playerInTurn().bankrupt == true) {
            endTurn();
            return;
        }
        foreach (Dice item in dice)
        {
            item.resetDice();
        }
        tradingModeOff();
        turnFrame.transform.position = playerPanel[turn].transform.position;
        rolled = false;
        rollValue = 0;
        activated = false;
        if (playerInTurn().inPrison > 0)
        {
            playerInTurn().inPrison--;
            Modal.instance().showModal("Bạn đang trong tù. Còn " + playerInTurn().inPrison + " lượt! Bạn có thể trả 50Đ để ra ngay hoặc đổ xúc xắc.", "Trả 50Đ", "Đổ xúc xắc", 
                () => {
                    if (playerInTurn().getFund() < 50)
                    {
                        Modal.instance().showModal("Bạn không có đủ 50Đ tiền mặt!", "Đổ xúc xắc",
                            () =>
                            {
                                rollTheDice();
                            });
                    }
                    else
                    {
                        playerInTurn().inPrison = -1;
                        playerInTurn().pay(50);
                    }
                },
                () => {
                    rollTheDice();
                }
            );
        }
        if (playerInTurn().inPrison == 0)
        {
            playerInTurn().inPrison = -1;
            Modal.instance().showModal("Bạn đã được ra tù!", "OK", () => { });
        }
    }

    public static void rollTheDice()
    {
        foreach(Dice item in dice)
        {
            item.startRolling();
        }
    }

    public static Player playerInTurn()
    {
        return player[turn];
    }

    public static void tradingModeOn()
    {
        tradingMode = true;
    }

    public static void tradingModeOff()
    {
        tradingMode = false;
        InfoPanel.instance().closeInfo();
    }

    public static string getPlayerRepresentative(int represent)
    {
        return avatar[represent].name.ToLower();
    }

    public static void jumpPlayerInTurn(int i)
    {
        playerInTurn().position = i;
        moveActor(50f);
    }

    public static void movePlayerInTurn(int i)
    {
        playerInTurn().position = i;
        moveActor();
        activated = false;
    }

    private static void moveActor(float speed = 5f)
    {
        FollowThePath curActor = actor[player[turn].represent].GetComponent<FollowThePath>();
        curActor.setSpeed(speed);
        curActor.moveAllowed = true;
        turnLock = true;
    }
}
