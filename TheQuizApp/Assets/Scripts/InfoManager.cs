using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    private SaveManager saveManager;
    private LoadManager loadManager;
    private CurrSessionManager currSessionManager;

    public List<QuestionData> notAnsweredQs = new List<QuestionData>();
    public List<QuestionData> wrongAnsweredQs = new List<QuestionData>();
    public List<QuestionData> answeredQs = new List<QuestionData>();

    // Later Save to a file in order to load the question numbers only once (Later, include for updates too)
    private bool questionNosLoaded = false; // Later, Make True as the bool is going to be loaded from a text file

    private void Awake()
    {
        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();
        currSessionManager = gameObject.GetComponent<CurrSessionManager>();
    }

    public void ConstructQuestions(string topicName, int numOfQuestions, int quesPerRound, List<QuestionData> questionData)
    {
        List<QuestionData> qsFromDivisions = new List<QuestionData>();

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

            int prevTotalNumOfQs = loadManager.LoadNumberOfQuestions(topicName);

            // If new questions are added to the topic, add them to the question numbers
            if (numOfQuestions > prevTotalNumOfQs)
            {
                for (int i = prevTotalNumOfQs + 1; i <= numOfQuestions; i++)
                {
                    notAnsweredQNos.Add(i);
                }
            }

            // Get the data from "questionData" such that its "QuestionNumber" is in the "notAnsweredQNos" list
            //      Thus, using the question numbers, we reconstruct our custom list of not answered questions from the whole list
            notAnsweredQs = new List<QuestionData>();
            for (int i = 0; i < notAnsweredQNos.Count; i++)
            {
                notAnsweredQs.Add(questionData.Find(item => item.QuestionNumber == notAnsweredQNos[i]));
            }

            wrongAnsweredQs = new List<QuestionData>();
            for (int i = 0; i < wrongAnsweredQNos.Count; i++)
            {
                wrongAnsweredQs.Add(questionData.Find(item => item.QuestionNumber == wrongAnsweredQNos[i]));
            }

            answeredQs = new List<QuestionData>();
            for (int i = 0; i < answeredQNos.Count; i++)
            {
                answeredQs.Add(questionData.Find(item => item.QuestionNumber == answeredQNos[i]));
            }

            if (notAnsweredQs.Count == 0 && wrongAnsweredQs.Count < quesPerRound)
            {
                notAnsweredQs = answeredQs;
                answeredQs = new List<QuestionData>();
            }
        }

        // Save the total number question count of the selected topic
        saveManager.SaveNumberOfQuestions(numOfQuestions, topicName);

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
                qsFromDivisions.Add(notAnsweredQs[i]);
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
                    qsFromDivisions.Add(wrongAnsweredQs[i]);
                }

                DivideAndGetQs(answeredQs.Count, finalRemainingQs, qsFromDivisions, answeredQs);
            }
        }

        // Manage the questions that the user answers during the session
        currSessionManager.ManageCurrSession(topicName, qsFromDivisions, quesPerRound);
        currSessionManager.topicSelected = true; // Let the class know that it can do its job
    }

    private void DivideAndGetQs(int totalNumberOfQs, int sessionQs, List<QuestionData> qsFromDivisions, List<QuestionData> currQData)
    {
        int startPoint = 0;

        // Make the question divisions and select the question numbers
        for (int i = 1; i <= sessionQs; i++)
        {
            System.Random r = new System.Random();

            int endPoint = (int) Math.Ceiling((1.0 * (totalNumberOfQs - 1) / sessionQs) * i);

            if (i == sessionQs)
            {
                endPoint++;
            }

            // Choose a random question from each division of the not answered question list
            qsFromDivisions.Add(currQData[r.Next(startPoint, endPoint)]);

            startPoint = endPoint;
        }   
    }
}
