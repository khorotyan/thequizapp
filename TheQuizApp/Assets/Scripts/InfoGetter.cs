using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoGetter : MonoBehaviour
{
    public Animator infoGetAnimator;
    public GameObject questionPanel;

    private string[] textLines;
    private List<QuestionData> questionData = new List<QuestionData>();

    private InfoManager infoManager;
    private int quesPerRound = 6;
    private string topicName;
    private int numOfQuestions;
    private int startLine;
    private int endLine;

    void Awake()
    {
        GetInfo();
        infoManager = gameObject.GetComponent<InfoManager>();
    }

    // Get information about the topic that the user chose
    public void OnTopicClick(string info)
    {
        questionPanel.SetActive(true);

        var timeMeasurer = System.Diagnostics.Stopwatch.StartNew();

        string[] splitInfo = info.Split('/');

        topicName = splitInfo[0];
        int.TryParse(splitInfo[1], out numOfQuestions);
        int.TryParse(splitInfo[2], out startLine);
        //int.TryParse(splitInfo[3], out endLine);
        startLine += 2;
        endLine = startLine + numOfQuestions * 5 - 1;

        QuestionAnswerExtractor();       

        infoManager.ConstructQuestions(topicName, numOfQuestions, quesPerRound, questionData);

        infoGetAnimator.SetTrigger("ClosePage");

        Debug.Log("InfoTime: " + timeMeasurer.ElapsedMilliseconds);    
    }

    // Extracts and manages the information from the text file containing the quiz questions
    private void GetInfo()
    {
        TextAsset inFile = Resources.Load("Questions") as TextAsset; // Loads the questions text file from the resources folder
        textLines = inFile.text.Split('\n');
    }

    // Extracts the questions and the answers to them from the text lines
    private void QuestionAnswerExtractor()
    {
        questionData.Clear();

        for (int i = startLine - 1, j = 0; i < endLine; i+=5, j++)
        {
            string currLine = textLines[i];

            // Every 5th line contains the question, difficulty and interest rates, and the correct answer number
            // Get the question, which is located inside the following characters "[ ... ]"
            string question = currLine.Substring(currLine.IndexOf('[') + 1, currLine.IndexOf(']') - currLine.IndexOf('[') - 1);   
            string a1 = textLines[i + 1];
            string a2 = textLines[i + 2];
            string a3 = textLines[i + 3];
            string a4 = textLines[i + 4];
            int questionNumber;
            int answerNumber;
            int difficultyRate;
            int interestRate;

            int.TryParse(currLine.Substring(0, currLine.IndexOf('[')), out questionNumber);
            int.TryParse(currLine.Substring(currLine.IndexOf(";a") + 2, 1), out answerNumber);
            int.TryParse(currLine.Substring(currLine.IndexOf(";d") + 2, 1), out difficultyRate);
            int.TryParse(currLine.Substring(currLine.IndexOf(";i") + 2, 1), out interestRate);

            questionData.Add(new QuestionData(question, a1, a2, a3, a4, questionNumber, answerNumber, difficultyRate, interestRate));
        }
    }	
}
