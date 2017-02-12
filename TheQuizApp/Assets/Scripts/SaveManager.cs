using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public void SaveQuestionLoadChecker(bool theChecker, string topicName)
    {
        ES2.Save(theChecker, "svt.txt?tag=loadChecker" + topicName);
    }

    public void SaveNumberOfQuestions(int totalQuestions, string topicName)
    {
        ES2.Save(totalQuestions, "svt.txt?tag=totalQuestions" + topicName);
    }

    public void SaveNotAnsweredQNumbers(List<int> notAnsweredQNos, string topicName)
    {
        ES2.Save(notAnsweredQNos, "svt.txt?tag=notAnsweredQNos" + topicName);       
    }

    public void SaveWrongAnsweredQNumbers(List<int> wrongAnsweredQNos, string topicName)
    {
        ES2.Save(wrongAnsweredQNos, "svt.txt?tag=wrongAnsweredQNos" + topicName);
    }

    public void SaveAnsweredQNumbers(List<int> answeredQNos, string topicName)
    {
        ES2.Save(answeredQNos, "svt.txt?tag=answeredQNos" + topicName);
    }

    public void SaveAllTimeMaxScore(int allTimeMaxScore)
    {
        ES2.Save(allTimeMaxScore, "svt.txt?tag=allTimeMaxScore");
    }

    // Save XP Class Objects
    public void SaveCurrentLevel(int currLevel)
    {
        ES2.Save(currLevel, "svt.txt?tag=currLevel");
    }

    public void SaveTotalXP(int totalXP)
    {
        ES2.Save(totalXP, "svt.txt?tag=totalXP");
    }
    // End of - Save XP Class Objects

    // Save Store Class Objects
    public void SaveTotalMoney(int totalMoney)
    {
        ES2.Save(totalMoney, "svt.txt?tag=totalMoney");
    }
    // End of - Save Store Class Objects
}
