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
    public Button[] buttons = new Button[4];
    private Text[] texts = new Text[4];

    [HideInInspector]
    public bool topicSelected = false;

    private UIResizer uiResizer;
    private List<QuestionData> qsFromDivisions;
    private int sessionCorrAnsNum;
    private int sessionQCount = 0;
    private int currQNumber = 0;
    private bool waitedForAnswer = false;

    private void Awake()
    {
        uiResizer = gameObject.GetComponent<UIResizer>();

        texts[0] = buttons[0].GetComponentInChildren<Text>();
        texts[1] = buttons[1].GetComponentInChildren<Text>();
        texts[2] = buttons[2].GetComponentInChildren<Text>();
        texts[3] = buttons[3].GetComponentInChildren<Text>();

        buttons[0].onClick.AddListener(() => CurrQManager(1));
        buttons[1].onClick.AddListener(() => CurrQManager(2));
        buttons[2].onClick.AddListener(() => CurrQManager(3));
        buttons[3].onClick.AddListener(() => CurrQManager(4));
    }

    private void Update()
    {
        if (waitedForAnswer == true)
        {
            ManageQShuffle();
            waitedForAnswer = false;
        }
    }

    public void ManageCurrSession(List<QuestionData> qsFromDivisions, int sessionQCount)
    {
        currQNumber = 0;

        this.qsFromDivisions = qsFromDivisions;
        this.sessionQCount = sessionQCount;

        CurrQManager(1);
    }

    public void CurrQManager(int buttonNumber)
    {
        if (topicPanel.activeSelf == true && questionPanel.activeSelf == false)
        {
            topicPanel.SetActive(false);
            questionPanel.SetActive(true);
        }

        if (currQNumber != 0)
        {
            if (buttonNumber == sessionCorrAnsNum)
            {
                //buttons[buttonNumber - 1].GetComponent<Image>().color = new Color(99, 199, 108);
                buttons[sessionCorrAnsNum - 1].GetComponent<Image>().color = Color.green;

                //Debug.Log("Correct");
            }
            else
            {
                //buttons[buttonNumber - 1].GetComponent<Image>().color = new Color(99, 199, 108);
                buttons[sessionCorrAnsNum - 1].GetComponent<Image>().color = Color.green;

                for (int i = 0; i < 4; i++)
                {
                    if (i != sessionCorrAnsNum - 1)
                    {
                        //buttons[i].GetComponent<Image>().color = new Color(255, 120, 120);
                        buttons[i].GetComponent<Image>().color = Color.red;
                    }
                }

                //Debug.Log("Incorrect");
            }
        }

        StartCoroutine(AnswerWaiter());
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

            sessionCorrAnsNum = answerNums[answerNums.IndexOf(sessionCorrAnsNum)];
            Debug.Log("sessionCorrAnsNum : " + sessionCorrAnsNum);

            for (int i = 0; i < 4; i++)
            {
                newAnswers[i] = answers[answerNums[i] - 1];
                texts[i].text = newAnswers[i];
            }
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
            buttons[i].gameObject.GetComponent<Image>().color = new Color(225, 225, 225);
        }

        // Resize the buttons and the texts
        uiResizer.ResizeUI();

        currQNumber++;
    } 

    IEnumerator AnswerWaiter()
    {
        yield return new WaitForSeconds(0.5f);
        waitedForAnswer = true;
    }
}
