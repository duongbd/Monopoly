using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    private Dropdown[] dropdowns;
    private InputField[] inputs;
    private static int[] represents;
    private static string[] names;

    void Awake()
    {
        dropdowns = new Dropdown[]
        {
            GameObject.Find("Player1/Dropdown").GetComponent<Dropdown>(),
            GameObject.Find("Player2/Dropdown").GetComponent<Dropdown>(),
            GameObject.Find("Player3/Dropdown").GetComponent<Dropdown>(),
            GameObject.Find("Player4/Dropdown").GetComponent<Dropdown>()
        };
        inputs = new InputField[]
        {
            GameObject.Find("Player1/InputField").GetComponent<InputField>(),
            GameObject.Find("Player2/InputField").GetComponent<InputField>(),
            GameObject.Find("Player3/InputField").GetComponent<InputField>(),
            GameObject.Find("Player4/InputField").GetComponent<InputField>()
        };
        represents = new int[4] { 4, 4, 4, 4 };
        names = new string[4] { "", "", "", "" };
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Dropdown dd in dropdowns)
        {
            dd.onValueChanged.AddListener((val) => {
                checkAvailable(val, dd);
            });
        }
        foreach (InputField ip in inputs)
        {
            ip.onEndEdit.AddListener((val) => {
                names[Array.IndexOf(inputs, ip)] = val;
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkAvailable(int val, Dropdown dd)
    {
        if (val != 4 && represents.Contains(val))
        {
            dd.value = 4;
            Modal.instance().showModal("Đại diện đã được người chơi khác chọn!", "OK", () => {});
        }
        else
        {
            represents[Array.IndexOf(dropdowns, dd)] = val;
        }
    }

    public static bool completed()
    {
        foreach (string name in names)
        {
            if (name == "")
            {
                return false;
            }
        }
        foreach (int rp in represents)
        {
            if (rp == 4)
            {
                return false;
            }
        }
        return true;
    }

    public static string getName(int i)
    {
        return names[i];
    }

    public static int getRepresent(int i)
    {
        return represents[i];
    }
}
