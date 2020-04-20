using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static Board board;
    public GameObject[] blocks;

    public static Board instance()
    {
        return board;
    }

    private void Awake()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
    }

    public Block getBlock(int i)
    {
        return blocks[i].GetComponent<Block>();
    }

    public List<Buyable> getBlockOwnedByPlayer(Player player)
    {
        List<Buyable> ownedBlocks = new List<Buyable>();
        foreach (GameObject block in blocks)
        {
            if (block.GetComponent<Buyable>() != null)
            {
                if (block.GetComponent<Buyable>().getOwner() == player)
                {
                    ownedBlocks.Add(block.GetComponent<Buyable>());
                }
            }
        }
        return ownedBlocks;
    }

    public Block getPlayerInTurnBlock()
    {
        return getBlock(GameController.playerInTurn().position);
    }
}
