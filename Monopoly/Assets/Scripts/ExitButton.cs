using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            Modal.instance().showModal("Bạn có chắc muốn thoát?", "Có", "Không",
                    () => {
                        Application.Quit();
                    },
                    () => { },
                    true
                );
        });
    }
}
