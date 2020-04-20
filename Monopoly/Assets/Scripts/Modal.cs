using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public struct Message
{
    public string text;
    public string yesText;
    public string noText;
    public UnityAction yesEvent;
    public UnityAction noEvent;
    public int type;
    public Message(string text, string yesText, string noText, UnityAction yesEvent, UnityAction noEvent)
    {
        this.type = 0;
        this.text = text;
        this.noText = noText;
        this.yesText = yesText;
        this.noEvent = noEvent;
        this.yesEvent = yesEvent;
    }
    public Message(string text, string okText, UnityAction okEvent)
    {
        this.type = 1;
        this.text = text;
        this.noText = okText;
        this.yesText = "";
        this.noEvent = okEvent;
        this.yesEvent = null;
    }
}

public class Modal : MonoBehaviour
{
    private static GameObject modalController;
    private static GameObject modal;
    private GameObject noButton;
    private GameObject yesButton;
    private TextMeshProUGUI yesButtonText;
    private TextMeshProUGUI noButtonText;
    private TextMeshProUGUI text;
    private Queue<Message> messages;
    private Message? displayMessage;

    public static Modal instance()
    {
        return modalController.GetComponent<Modal>();
    }

    // Start is called before the first frame update
    void Awake()
    {
        messages = new Queue<Message>();
        modalController = GameObject.Find("ModalController");
        modal = GameObject.Find("Modal");
        noButton = GameObject.Find("Modal/NoButton");
        yesButton = GameObject.Find("Modal/YesButton");
        yesButtonText = GameObject.Find("Modal/YesButton/YesButtonText").GetComponent<TextMeshProUGUI>();
        noButtonText = GameObject.Find("Modal/NoButton/NoButtonText").GetComponent<TextMeshProUGUI>();
        text = GameObject.Find("Modal/Text").GetComponent<TextMeshProUGUI>();
        modal.SetActive(false);
    }

    void Update()
    {
        if (GameController.waitModal == false && messages.Count > 0)
        {
            if (messages.Peek().type == 0)
            {
                showModal(messages.Peek().text, messages.Peek().yesText, messages.Peek().noText, messages.Peek().yesEvent, messages.Peek().noEvent);
            } else
            {
                showModal(messages.Peek().text, messages.Peek().noText, messages.Peek().noEvent);
            }
            messages.Dequeue();
        }
    }

    public void showModal(string modalText, string yesText, string noText, UnityAction yesEvent, UnityAction noEvent, bool prior = false)
    {
        Message message = new Message(modalText, yesText, noText, yesEvent, noEvent);
        if (GameController.waitModal == true && !displayMessage.Equals(message))
        {
            if (!messages.Contains(message) && !prior) messages.Enqueue(message);
            if (!messages.Contains(message) && prior)
            {
                addToFrontOfQueue(message);
                GameController.waitModal = false;
            }
            return;
        }
        GameController.waitModal = true;
        displayMessage = message;
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

    public void showModal(string modalText, string okText, UnityAction okEvent, bool prior = false)
    {
        Message message = new Message(modalText, okText, okEvent);
        if (GameController.waitModal == true && !displayMessage.Equals(message))
        {
            if (!messages.Contains(message) && !prior) messages.Enqueue(message);
            if (!messages.Contains(message) && prior)
            {
                addToFrontOfQueue(message);
                GameController.waitModal = false;
            }
            return;
        }
        GameController.waitModal = true;
        displayMessage = message;
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
        displayMessage = null;
        GameController.waitModal = false;
    }

    private void addToFrontOfQueue(Message mess)
    {
        Message[] clone = new Message[messages.Count];
        messages.CopyTo(clone, 0);
        messages.Clear();
        messages.Enqueue(mess);
        if (displayMessage != null)
        {
            messages.Enqueue((Message)displayMessage);
        }
        for (int i = 0; i< clone.Length; i++)
        {
            messages.Enqueue(clone[i]);
        }
    }
}
