using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manage the questions that the user answers during the session
public class CurrSessionManager : MonoBehaviour
{   
    [SerializeField]
    private GameObject topicPanel;
    [SerializeField]
    private GameObject questionPanel;
    [SerializeField]
    private Text questionText;
    [SerializeField]
    private Button nextButton;
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
    private SessionTimerManager sessionTimerManager;
    private SessionCurrencyManager sessionCurrencyManager;
    private ResultsPageManager resultsPageManager;
    private HelpersDuringSess helpersDuringSess;
    private InfoManager infoManager;
    private List<QuestionData> qsFromDivisions;
    private string topicName;
    private int sessionCorrAnsNum;
    private int currButtonNum;
    private int sessionQCount = 0;
    private int currQNumber = 0;
    private int numOfCorrAnss = 0;

    private void Awake()
    {
        uiResizer = gameObject.GetComponent<UIResizer>();
        sessionTimerManager = gameObject.GetComponent<SessionTimerManager>();
        sessionCurrencyManager = gameObject.GetComponent<SessionCurrencyManager>();
        resultsPageManager = gameObject.GetComponent<ResultsPageManager>();
        helpersDuringSess = gameObject.GetComponent<HelpersDuringSess>();
        infoManager = gameObject.GetComponent<InfoManager>();

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

    // Gets information about the questions for the session consisting of "sessionQCount" questions
    public void ManageCurrSession(string topicName, List<QuestionData> qsFromDivisions, int sessionQCount)
    {
        this.topicName = topicName;

        currQNumber = 0;
        numOfCorrAnss = 0;
        sessionCurrencyManager.BeforeANewGame();

        this.qsFromDivisions = qsFromDivisions;
        this.sessionQCount = sessionQCount;

        CurrQManager();
        ManageQShuffle();
        sessionCurrencyManager.CurrencyCalculations();
    }

    // When an answer is clicked, the this method is run
    public void OnAnswerClick(int currButtonNum)
    {
        this.currButtonNum = currButtonNum;

        // The question was normally answered
        if (canClickAnswer == true && answerSelected == false)
        {
            CurrQManager();
            answerSelected = true;
            sessionTimerManager.CancelInvoke("TimerCalculator");
        }
        // The question was answered again 
        else if (canClickAnswer == false && answerSelected == true)
        {
            helpersDuringSess.OtherButtonClickedAfterAnswering();
        }

        // The question was answered too quickly
        if (sessionTimerManager.answerIsFake == true)
        {
            helpersDuringSess.AnsweredTooQuickly();
        }     
    }

    // When the Next Button is clicked, the question is changed
    public void OnNextClick()
    {
        // If the user is allowed to click next
        if (canClickNext == true)
        {
            ManageQShuffle();

            answerSelected = false;
            canClickNext = false;

            sessionCurrencyManager.CurrencyCalculations();

            // When the user finishes the current game, the Results page opens
            if (currQNumber == sessionQCount + 1)
            {
                resultsPageManager.UpdateResultsPage(topicName, sessionCurrencyManager.sessTotalMoney, sessionQCount, numOfCorrAnss, sessionCurrencyManager.maxConCorAns);
                SessionTimerManager.canStartTimerSubtracting = false;
            }
        }
        // The user clicked next without answering the question
        else
        {         
            helpersDuringSess.ClickedNextWithoutAnswering();
        }
        
    }

    // Determines whether the question answer is correct or wrong
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
                buttons[sessionCorrAnsNum - 1].GetComponent<Image>().color = sessionTimerManager.beautyGreen;

                numOfCorrAnss++;
                sessionCurrencyManager.CurrencyOnCorrAns();

                infoManager.notAnsweredQs.Remove(qsFromDivisions[currQNumber - 1]);
                infoManager.wrongAnsweredQs.Remove(qsFromDivisions[currQNumber - 1]);
                infoManager.answeredQs.Remove(qsFromDivisions[currQNumber - 1]);

                infoManager.answeredQs.Add(qsFromDivisions[currQNumber - 1]);            
            }
            else // Incorrect Answer
            {
                buttons[sessionCorrAnsNum - 1].GetComponent<Image>().color = sessionTimerManager.beautyGreen;

                for (int i = 0; i < 4; i++)
                {
                    if (i != sessionCorrAnsNum - 1)
                    {
                        buttons[i].GetComponent<Image>().color = sessionTimerManager.beautyRed;
                    }
                }

                sessionCurrencyManager.CurrencyOnIncorrAns();

                infoManager.notAnsweredQs.Remove(qsFromDivisions[currQNumber - 1]);
                infoManager.wrongAnsweredQs.Remove(qsFromDivisions[currQNumber - 1]);
                infoManager.answeredQs.Remove(qsFromDivisions[currQNumber - 1]);

                infoManager.wrongAnsweredQs.Add(qsFromDivisions[currQNumber - 1]);
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
            sessionTimerManager.DoTimerCalculations(qsFromDivisions[currQNumber]);   
        }
        else
        {
            // The end of the questions
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
