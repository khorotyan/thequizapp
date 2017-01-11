using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manage the questions that the user answers during the session
public class CurrSessionManager : MonoBehaviour
{
    public GameObject topicPanel;
    public GameObject questionPanel;

    public Text questionText;
    public Button nextButton;
    public Button[] buttons = new Button[4];
    private Text[] texts = new Text[4];
    private Color32 defButtonColor;

    [HideInInspector]
    public bool topicSelected = false;
    [HideInInspector]
    public bool canClickAnswer = true;
    [HideInInspector]
    public bool canClickNext = false;
    [HideInInspector]
    public bool answerSelected = false;

    private UIResizer uiResizer;
    private SessionTimerManager qpageInfoMger;
    private SessionCurrencyManager sessionCurrencyManager;
    private List<QuestionData> qsFromDivisions;
    private int sessionCorrAnsNum;
    private int currButtonNum;
    private int sessionQCount = 0;
    private int currQNumber = 0;

    private void Awake()
    {
        uiResizer = gameObject.GetComponent<UIResizer>();
        qpageInfoMger = gameObject.GetComponent<SessionTimerManager>();
        sessionCurrencyManager = gameObject.GetComponent<SessionCurrencyManager>();

        nextButton.onClick.AddListener(() => OnNextClick());

        texts[0] = buttons[0].GetComponentInChildren<Text>();
        texts[1] = buttons[1].GetComponentInChildren<Text>();
        texts[2] = buttons[2].GetComponentInChildren<Text>();
        texts[3] = buttons[3].GetComponentInChildren<Text>();

        buttons[0].onClick.AddListener(() => OnAnswerClick(1));
        buttons[1].onClick.AddListener(() => OnAnswerClick(2));
        buttons[2].onClick.AddListener(() => OnAnswerClick(3));
        buttons[3].onClick.AddListener(() => OnAnswerClick(4));

        defButtonColor = buttons[0].GetComponent<Image>().color;
    }

    private void Update()
    {
        
    }

    // Gets information about the questions for the session consisting of "sessionQCount" questions
    public void ManageCurrSession(List<QuestionData> qsFromDivisions, int sessionQCount)
    {
        currQNumber = 0;

        this.qsFromDivisions = qsFromDivisions;
        this.sessionQCount = sessionQCount;

        CurrQManager();
        ManageQShuffle();
        sessionCurrencyManager.CurrencyCalculations();
    }

    public void OnAnswerClick(int currButtonNum)
    {
        this.currButtonNum = currButtonNum;

        if (canClickAnswer == true && answerSelected == false)
        {
            CurrQManager();
            answerSelected = true;
            qpageInfoMger.CancelInvoke("TimerCalculator");
        }
        else
        {
            // Say that the question was answered too quickly
        }        
    }

    public void OnNextClick()
    {
        if (canClickNext == true)
        {
            ManageQShuffle();

            answerSelected = false;
            canClickNext = false;

            sessionCurrencyManager.CurrencyCalculations();
        }
        
    }

    public void CurrQManager()
    {
        if (topicPanel.activeSelf == true && questionPanel.activeSelf == false)
        {
            topicPanel.SetActive(false);
            questionPanel.SetActive(true);
        }

        if (currQNumber != 0)
        {
            // Correct Answer
            if (currButtonNum == sessionCorrAnsNum)
            {
                buttons[sessionCorrAnsNum - 1].GetComponent<Image>().color = Color.green;

                sessionCurrencyManager.CurrencyOnCorrAns();
            }
            else // Incorrect Answer
            {
                buttons[sessionCorrAnsNum - 1].GetComponent<Image>().color = Color.green;

                for (int i = 0; i < 4; i++)
                {
                    if (i != sessionCorrAnsNum - 1)
                    {
                        buttons[i].GetComponent<Image>().color = Color.red;
                    }
                }

                sessionCurrencyManager.CurrencyOnIncorrAns();
            }

            canClickNext = true;
            canClickAnswer = false;
        }
    }

    private void ManageQShuffle()
    {
        System.Random rand = new System.Random();
        
        List<string> answers = new List<string>();
        string[] newAnswers = new string[4];      

        if (currQNumber < sessionQCount)
        {
            answers.Add(qsFromDivisions[currQNumber].A1);
            answers.Add(qsFromDivisions[currQNumber].A2);
            answers.Add(qsFromDivisions[currQNumber].A3);
            answers.Add(qsFromDivisions[currQNumber].A4);

            sessionCorrAnsNum = qsFromDivisions[currQNumber].AnswerNumber;

            questionText.text = qsFromDivisions[currQNumber].Question;

            List<int> answerNums = new List<int>();

            for (int i = 1; i <= 4; i++)
            {
                answerNums.Add(i);
            }

            // Shuffle the numbers of the list randomly
            answerNums = answerNums.OrderBy(item => rand.Next()).ToList();

            // Get the correct answer number after the shuffle
            sessionCorrAnsNum = answerNums.IndexOf(sessionCorrAnsNum) + 1;
            Debug.Log("Correct Answer : " + sessionCorrAnsNum);

            for (int i = 0; i < 4; i++)
            {
                newAnswers[i] = answers[answerNums[i] - 1];
                texts[i].text = newAnswers[i];
            }

            // Calculate the amount of time given for the current question and manage the timer
            qpageInfoMger.DoTimerCalculations(qsFromDivisions[currQNumber]);   
        }
        else
        {
            questionPanel.SetActive(false);
            topicPanel.SetActive(true);

            // The end of the questions
            // A new page can open and write the session score, earned coins, ...
        }

        for (int i = 0; i < 4; i++)
        {
            buttons[i].gameObject.GetComponent<Image>().color = defButtonColor;
        }

        // Resize the buttons and the texts
        uiResizer.ResizeUI();      

        currQNumber++;
    } 
}
