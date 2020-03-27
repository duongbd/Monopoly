using System.Collections;
using UnityEngine;

public class Dice : MonoBehaviour {

	// Array of dice sides sprites to load from Resources folder
    // mang dice sides sprites de load tu Rosources folder  
	private Sprite[] diceSides;

	// Reference to sprite renderer to change sprites
	// tham chieu den Sprite render de thay doi sprite 
	private SpriteRenderer rend;

	// Use this for initialization ( khoi tao )
	private void Start () {

		// Assign Renderer component (gan phan tu rederer)
		rend = GetComponent<SpriteRenderer>();

		// Load dice sides sprites to array from DiceSides subfolder of Resources folder(load dice sides den mang )
		diceSides = Resources.LoadAll<Sprite>("DiceSides/");
	}

	// If you left click over the dice then RollTheDice coroutine is started()
	//(khi click chuot traii vao Dice thi chay rollthedice)
	private void OnMouseDown()
	{
		StartCoroutine("RollTheDice");
	}

	// Coroutine that rolls the dice
	private IEnumerator RollTheDice()
	{
		// Variable to contain random dice side number.(bien de chua dung so cac dice side ngau nhien )
		// It needs to be assigned. Let it be 0 initially (khoi tao 0)
		int randomDiceSide = 0;

		// Final side or value that dice reads in the end of coroutine
		int finalSide = 0;

		// Loop to switch dice sides ramdomly(vong lap de chuyen doi cac slide side ngau nhien  )
		// before final side appears. 20 itterations here.(truoc khi mat cuoi xuat hien thif cos 20 lan lap  )
		for (int i = 0; i <= 20; i++)
		{
			// Pick up random value from 0 to 5 (All inclusive) LAY NGAU NHIEN CAC GIA TRI TU 0-5
			randomDiceSide = Random.Range(0, 5);

			// Set sprite to upper face of dice from array according to random value (Đặt sprite lên mặt trên của súc sắc từ mảng theo giá trị ngẫu nhiên

			rend.sprite = diceSides[randomDiceSide];

			// Pause before next itteration (PAUSE TRUOC LAN LAP TIEP THEO )
			yield return new WaitForSeconds(0.05f);
		}

		// Assigning final side so you can use this value later in your game ( GAN SIDE CUOI CUNG DE SU DUNG GIA TRI NAY TRONG GAME )
		// for player movement for example
		finalSide = randomDiceSide + 1;

		// Show final dice value in Console
		Debug.Log(finalSide);
	}
}
