using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Modal : MonoBehaviour
{
    public static GameObject modal;
    public GameObject noButton;
    public GameObject yesButton;
    public TextMeshProUGUI yesButtonText;
    public TextMeshProUGUI noButtonText;
    public TextMeshProUGUI text;

    public static Modal instance()
    {
        return modal.GetComponent<Modal>();
    }

    // Start is called before the first frame update
    void Start()
    {
        modal = GameObject.Find("Modal");
        noButton = GameObject.Find(gameObject.name + "/NoButton");
        yesButton = GameObject.Find(gameObject.name + "/YesButton");
        yesButtonText = GameObject.Find(gameObject.name + "/YesButton/YesButtonText").GetComponent<TextMeshProUGUI>();
        noButtonText = GameObject.Find(gameObject.name + "/NoButton/NoButtonText").GetComponent<TextMeshProUGUI>();
        text = GameObject.Find(gameObject.name + "/Text").GetComponent<TextMeshProUGUI>();
        modal.SetActive(false);
    }

    public void showModal(string modalText, string yesText, string noText, UnityAction yesEvent, UnityAction noEvent)
    {
        GameController.waitModal = true;
        text.SetText(modalText);
        noButtonText.SetText(noText);
        yesButtonText.SetText(yesText);
        
        yesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        yesButton.GetComponent<Button>().onClick.AddListener(closeModal);
        yesButton.GetComponent<Button>().onClick.AddListener(yesEvent);

        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(closeModal);
        noButton.GetComponent<Button>().onClick.AddListener(noEvent);


        yesButton.SetActive(true);
        noButton.SetActive(true);
        modal.SetActive(true);
    }

    public void showModal(string modalText, string okText, UnityAction okEvent)
    {
        GameController.waitModal = true;
        text.SetText(modalText);
        noButtonText.SetText(okText);

        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(closeModal);
        noButton.GetComponent<Button>().onClick.AddListener(okEvent);

        yesButton.SetActive(false);
        noButton.SetActive(true);
        modal.SetActive(true);
    }

    private void closeModal()
    {
        modal.SetActive(false);
        GameController.waitModal = false;
    }
}
