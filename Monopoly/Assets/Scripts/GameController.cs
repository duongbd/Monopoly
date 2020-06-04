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

    public static Stack<string> chart;
    public static bool gameOver = false;
    public static bool activated = false;
    public static int rollValue = 0;
    public static bool rolled = false;
    public static bool waitModal = false;
    public static bool tradingMode = false;
    public static bool turnLock = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        activated = false;
        rollValue = 0;
        rolled = false;
        waitModal = false;
        tradingMode = false;
        turnLock = false;
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
        chart = new Stack<string>();
        for (int i = 0; i < player.Length; i++)
        {
            try {
                player[i].playerName = GameSetting.getName(i);
                player[i].represent = GameSetting.getRepresent(i);
                Destroy(GameObject.Find("GameSetting"));
            }
            catch (System.NullReferenceException)
            {
                player[i].playerName = "Player " + (i + 1);
                player[i].represent = i;
            }
            avatar[player[i].represent].transform.position = playerPanel[i].transform.position + new Vector3(-129,50,0);
            playerPanel[i].updatePanel(player[i].playerName, player[i].getFund(), player[i].calNetWorth());
        }
        turn = 0;
        //for (int i = 0; i < 3; i++)
        //{
        //    player[i].declareBankrupt();
        //}
        StartCoroutine("determineTurn");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }
        if (dice[0].ifCoroutineAllowed() && dice[1].ifCoroutineAllowed() && rolled == false && rollValue > 0)
        {
            StartCoroutine("displayRollValue");
            rolled = true;
            if (playerInTurn().inPrison >= 0)
            {
                activated = true;
                if (dice[0].getDiceSideThrown() == dice[1].getDiceSideThrown())
                {
                    playerInTurn().inPrison = -1;
                    Modal.instance().showModal("<size=150%><color=#00d8ff><b>Chúc mừng</b></color></size>\nBạn đã được ra tù do đổ được 2 mặt giống nhau!", "OK", () => {
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
                movePlayerInTurn(rollValue, false);
            }
        }
        if (rolled && playerInTurnActor().moveAllowed == false && activated == false)
        {
            if (player[turn].position - rollValue < 0) Board.instance().getBlock(0).activate();
            else Board.instance().getPlayerInTurnBlock().activate();
            activated = true;
        }
        if (!playerInTurnActor().backward() && playerInTurnActor().target == player[turn].position + 1)
        {
            playerInTurnActor().moveAllowed = false;
            turnLock = false;
        }
        if (playerInTurnActor().backward() && playerInTurnActor().target == player[turn].position - 1)
        {
            playerInTurnActor().moveAllowed = false;
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
                    chart.Push(player[i].playerName);
                };
                player[i].needUpdate = false;
            }
        }
        if(playersLeft() == 1)
        {
            gameOver = true;
            chart.Push(playerInTurn().playerName);
            Modal.instance().showModal("<size=150%><color=#00d8ff><b>Game Over!", "OK", 
                () => {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
                },
                true
            );
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
        iTween.MoveTo(turnFrame, playerPanel[turn].transform.position, 1);
        rolled = false;
        rollValue = 0;
        activated = false;
        if (playerInTurn().inPrison == 0)
        {
            playerInTurn().inPrison = -1;
            Modal.instance().showModal("<size=150%><b>Hết 3 lượt trong tù</b></size>\nBạn đã được ra tù!", "OK", () => { });
        }
        if (playerInTurn().inPrison > 0)
        {
            playerInTurn().inPrison--;
            Modal.instance().showModal("<size=150%><b>Trong tù</b></size>\nCòn <b>" + playerInTurn().inPrison + " lượt</b>! Bạn có thể trả <color=#aa0115><b>50Đ</b></color> để ra ngay hoặc đổ xúc xắc.", "Trả <color=#aa0115><b>50Đ</b></color>", "Đổ xúc xắc", 
                () => {
                    if (playerInTurn().getFund() < 50)
                    {
                        Modal.instance().showModal("Bạn không có đủ <color=#aa0115><b>50Đ</b></color> tiền mặt!", "Đổ xúc xắc",
                            () =>
                            {
                                rollTheDice();
                            });
                    }
                    else
                    {
                        playerInTurn().inPrison = -1;
                        playerInTurn().pay(50);
                        Modal.instance().showModal("Bạn đã được ra tù!", "OK", () => { });
                    }
                },
                () => {
                    rollTheDice();
                }
            );
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

    public static void movePlayerInTurn(int steps, bool backward = false)
    {
        if (!backward) playerInTurn().move(steps);
        else playerInTurn().moveBack(steps);
        moveActor(5f, backward);
    }

    public static List<Player> getPlayersNotInTurn()
    {
        List<Player> players = new List<Player>();
        foreach (Player player in player)
        {
            if (player != playerInTurn() && player.bankrupt == false)
            {
                players.Add(player);
            }
        }
        return players;
    }

    public static FollowThePath playerInTurnActor()
    {
        return actor[player[turn].represent].GetComponent<FollowThePath>();
    }

    private static void moveActor(float speed = 5f, bool backward = false)
    {
        FollowThePath curActor = playerInTurnActor();
        curActor.setSpeed(speed);
        curActor.setDirection(backward);
        curActor.moveAllowed = true;
        turnLock = true;
    }

    private IEnumerator determineTurn()
    {
        turnLock = true;
        int[] values = new int[4] { 0, 0, 0, 0 };
        for (int i=0; i<4; i++)
        {
            activated = true;
            rolled = true;
            Modal.instance().showModal("<size=150%><b>Xác định lượt</b></size>\n" +
                "- " + player[0].playerName + ": <i>" + values[0] + "</i>\n" +
                "- " + player[1].playerName + ": <i>" + values[1] + "</i>\n" +
                "- " + player[2].playerName + ": <i>" + values[2] + "</i>\n" +
                "- " + player[3].playerName + ": <i>" + values[3] + "</i>\n",
                "Đổ xúc xắc",
                () => {
                    rollTheDice();
                }
            );
            yield return new WaitUntil(() => dice[0].ifCoroutineAllowed() && dice[1].ifCoroutineAllowed() && rollValue > 0);
            StartCoroutine("displayRollValue");
            values[turn] = rollValue;
            endTurn();
        }
        System.Array.Sort(values, player, new myCompare());
        for (int i = 0; i < player.Length; i++)
        {
            iTween.MoveTo(avatar[player[i].represent], playerPanel[i].transform.position + new Vector3(-129, 50, 0), 1);
            playerPanel[i].updatePanel(player[i].playerName, player[i].getFund(), player[i].calNetWorth());
        }
        Modal.instance().showModal("<size=150%><b>Thứ tự chơi</b></size>\n" +
                "<b>1.</b> " + player[0].playerName + ": <i>" + values[0] + "</i>\n" +
                "<b>2.</b> " + player[1].playerName + ": <i>" + values[1] + "</i>\n" +
                "<b>3.</b> " + player[2].playerName + ": <i>" + values[2] + "</i>\n" +
                "<b>4.</b> " + player[3].playerName + ": <i>" + values[3] + "</i>\n",
                "OK",
                () => {
                }
            );
        turnLock = false;
    }

    private int playersLeft()
    {
        int count = 4;
        foreach(Player pl in player)
        {
            if (pl.bankrupt)
            {
                count--;
            }
        }
        return count;
    }

    private IEnumerator displayRollValue()
    {
        GameObject newGO = new GameObject("RollValue");
        newGO.transform.SetParent(Board.instance().transform);
        TextMeshProUGUI valText = newGO.AddComponent<TextMeshProUGUI>();
        valText.transform.position = Board.instance().transform.position + new Vector3(0, 270, 0);
        valText.alignment = TextAlignmentOptions.Center;
        valText.fontStyle = FontStyles.Bold;
        valText.fontSize = 150f;
        valText.color = new Color32(0, 216, 255, 255);
        valText.SetText(rollValue.ToString());

        iTween.PunchScale(valText.gameObject, iTween.Hash("x", 0.015, "y", 0.015, "time", 1));

        yield return new WaitForSeconds(1.5f);

        Destroy(valText.gameObject);
    }
}

public class myCompare : IComparer<int>
{
    public int Compare(int x, int y)
    {
        return y - x;
    }
}

