using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private UnityEngine.UI.Dropdown button;

    void Awake()
    {
        button = GetComponent<UnityEngine.UI.Dropdown>();
        button.onValueChanged.AddListener(onValueChange);
    }

    void onValueChange(int val)
    {
        switch (val) {
            case 0:
                Modal.instance().showModal("Chả có gì.", "OK",
                    () => {
                        button.value = 3;
                    },
                    true
                );
                break;

            case 1:
                Modal.instance().showModal("Bạn có chắc muốn về trang chủ?", "Có", "Không",
                    () => {
                        UnityEngine.SceneManagement.SceneManager.LoadScene("GameSetting");
                    },
                    () => {
                        button.value = 3;
                    },
                    true
                );
                break;

            case 2:
                Modal.instance().showModal("Bạn có chắc muốn thoát?", "Có", "Không", 
                    () => {
                        Application.Quit();
                    },
                    () => {
                        button.value = 3;
                    },
                    true
                );
                break;
        }
    }
}
