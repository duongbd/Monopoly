using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Text[] chart;
    private Button playAgainButton;
    private Button exitButton;

    void Awake()
    {
        chart = new Text[]
        {
            GameObject.Find("First").GetComponent<Text>(),
            GameObject.Find("Second").GetComponent<Text>(),
            GameObject.Find("Third").GetComponent<Text>(),
            GameObject.Find("Forth").GetComponent<Text>()
        };
        playAgainButton = GameObject.Find("PlayAgainButton").GetComponent<Button>();
        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playAgainButton.onClick.AddListener(() => {
            Destroy(Modal.instance());
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameSetting");
        });
        exitButton.onClick.AddListener(() => {
            Modal.instance().showModal("Bạn có chắc muốn thoát?", "Có", "Không",
                    () => {
                        Application.Quit();
                    },
                    () => {},
                    true
                );
        });
        activate(GameController.chart);
    }

    void activate(Stack<string> names)
    {
        chart[0].text = names.Pop() + " (Winner!)";
        for (int i=1; i<4; i++)
        {
            chart[i].text = names.Pop();
        }
    }
}
