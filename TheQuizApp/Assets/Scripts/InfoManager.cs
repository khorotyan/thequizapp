using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    private SaveManager saveManager;
    private LoadManager loadManager;

    private List<QuestionData> notAnsweredQs = new List<QuestionData>();
    private List<QuestionData> wrongAnsweredQs = new List<QuestionData>();
    private List<QuestionData> answeredQs = new List<QuestionData>();

    // Later Save to a file in order to load the question numbers only once (Later, include for updates too)
    bool questionNosLoaded = false; // Later, Make True as the bool is going to be loaded from a text file

    private void Awake()
    {
        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();
    }

    public void ConstructQuestions(string topicName, int numOfQuestions, int quesPerRound, List<QuestionData> questionData)
    {
        List<int> qsFromDivisions = new List<int>();

        questionData.Sort();
        questionNosLoaded = loadManager.LoadQuestionLoadChecker(topicName);

        // Load "notAnsweredQNos" List
        if (questionNosLoaded == false)
        {   
            notAnsweredQs = questionData;

            // Returns the question numbers from the "notAnsweredQs" list of custom data
            List<int> notAnsweredQNos = notAnsweredQs.Select(item => item.QuestionNumber).ToList();

            questionNosLoaded = true;
            saveManager.SaveQuestionLoadChecker(questionNosLoaded, topicName); // Save "questionNosLoaded" bool
            // Save all questio numbers since the topic is played only once
            saveManager.SaveNotAnsweredQNumbers(notAnsweredQNos, topicName); 
        }
        else
        {
            List<int> notAnsweredQNos = loadManager.LoadNotAnsweredQNumbers(topicName);
            List<int> wrongAnsweredQNos = loadManager.LoadWrongAnsweredQNumbers(topicName);
            List<int> answeredQNos = loadManager.LoadAnsweredQNumbers(topicName);

            // Get the data from "questionData" such that its "QuestionNumber" is in the "notAnsweredQNos" list
            //      Thus, using the question numbers, we reconstruct our custom list of not answered questions from the whole list
            notAnsweredQs = questionData.Select( item => questionData[notAnsweredQNos.FindIndex(x => x == item.QuestionNumber)] ).ToList();

            wrongAnsweredQs = questionData.Select( item => questionData[wrongAnsweredQNos.FindIndex(x => x == item.QuestionNumber)] ).ToList();
            answeredQs = questionData.Select( item => questionData[answeredQNos.FindIndex(x => x == item.QuestionNumber)] ).ToList();
        }

        // Select the questions for the current session
        //
        // If the nonanswered question list contains questions equal or greater than the questions for the round
        //      divide the questions into parts equal to the question number for the session and choose questions from them
        //      / -- Game Design : Book 1 - Page 28, (1-a) --\ 
        if (notAnsweredQs.Count >= quesPerRound)
        {
            DivideAndGetQs(notAnsweredQs.Count, quesPerRound, qsFromDivisions, notAnsweredQs);
        }
        else // If there are less questions in the not answered question list, (1-b) 
        {
            int remainingQs = quesPerRound - notAnsweredQs.Count;

            for (int i = 0; i < notAnsweredQs.Count; i++)
            {
                qsFromDivisions.Add(notAnsweredQs[i].QuestionNumber);
            }

            // If the wrong answered question list contains questions equal or greater than the 
            //      remaining questions from the list, (2-a)
            if (wrongAnsweredQs.Count >= remainingQs)
            {
                DivideAndGetQs(wrongAnsweredQs.Count, remainingQs, qsFromDivisions, wrongAnsweredQs);
            }
            else // Page 28 - (2-b)
            {
                int finalRemainingQs = quesPerRound - (notAnsweredQs.Count + wrongAnsweredQs.Count);

                for (int i = 0; i < wrongAnsweredQs.Count; i++)
                {
                    qsFromDivisions.Add(wrongAnsweredQs[i].QuestionNumber);
                }

                DivideAndGetQs(answeredQs.Count, finalRemainingQs, qsFromDivisions, answeredQs);
            }
        }
    }

    private void DivideAndGetQs(int totalNumberOfQs, int sessionQs, List<int> qsFromDivisions, List<QuestionData> currQData)
    {
        int startPoint = 0;

        // Make the question divisions and select the question numbers
        for (int i = 1; i <= sessionQs; i++)
        {
            System.Random r = new System.Random();

            int endPoint = (int)Math.Ceiling((1.0 * (totalNumberOfQs - 1) / sessionQs) * i);

            if (i == sessionQs)
            {
                endPoint++;
            }

            // Choose a random question from each division of the not answered question list
            qsFromDivisions.Add(currQData[r.Next(startPoint, endPoint)].QuestionNumber);

            startPoint = endPoint;
        }
    }
}
