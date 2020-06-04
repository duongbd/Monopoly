using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int diceSideThrown;
    private bool coroutineAllowed = true;

	// Use this for initialization
	private void Start () {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("Dice/");
	}

    public void startRolling()
    {
        if (coroutineAllowed) StartCoroutine("rolling");
    }

    private IEnumerator rolling()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);        //lấy từ 0 đến 5
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        diceSideThrown = randomDiceSide + 1;
        //diceSideThrown = 1;

        GameController.rollValue += diceSideThrown;
        //GameController.rollValue = 1;

        yield return new WaitForSeconds(1.5f);

        coroutineAllowed = true;
    }

    public bool ifCoroutineAllowed()
    {
        return coroutineAllowed;
    }

    public void resetDice()
    {
        rend.sprite = null;
        diceSideThrown = 0;
    }

    public int getDiceSideThrown()
    {
        return diceSideThrown;
    }
}
