using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(startGame);
    }

    private void startGame()
    {
        if (GameSetting.completed())
        {
            Destroy(Modal.instance());
            SceneManager.LoadScene("MainGame");
        } else
        {
            Modal.instance().showModal("Bạn chưa hoàn thành cấu hình game!", "OK", () => { });
        }
    }
}
