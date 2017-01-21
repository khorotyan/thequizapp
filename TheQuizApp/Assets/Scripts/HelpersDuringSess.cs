using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpersDuringSess : MonoBehaviour
{
    [SerializeField]
    private GameObject messagePanel;
    [SerializeField]
    private Text messageTitleText;
    private Image messageTitleImage;
    [SerializeField]
    private Text messageText;

    private SessionTimerManager sessionTimerManager;

    private void Awake()
    {
        sessionTimerManager = gameObject.GetComponent<SessionTimerManager>();
        messageTitleImage = messageTitleText.transform.parent.GetComponent<Image>();
    }

    public void OpenCloseSessionHelpers()
    {
        // If the active panel is Questions Panel
        if (messagePanel.transform.parent.gameObject.activeSelf == true)
        {
            // If message panel is active make it unactive and the opposite
            messagePanel.SetActive(!messagePanel.activeSelf);
        }
    }

    public void AnsweredTooQuickly()
    {      
        messageTitleText.text = "Warning";
        messageTitleImage.color = Color.yellow;
        messageText.text = "The Question Was Answered Too Quickly, Please Take Your Time and Think Carefully";

        OpenCloseSessionHelpers();
    }

    public void ClickedNextWithoutAnswering()
    {
        messageTitleText.text = "Info";
        messageTitleImage.color = sessionTimerManager.beautyGreen;
        messageText.text = "Please Answer the Question and Then Click Next";

        OpenCloseSessionHelpers();
    }

    public void OtherButtonClickedAfterAnswering()
    {
        messageTitleText.text = "Info";
        messageTitleImage.color = sessionTimerManager.beautyGreen;
        messageText.text = "Please Click the Next Button Below to Move On to the Next Question";

        OpenCloseSessionHelpers();
    }
}
