﻿using System;
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

    public void SaveTotalGems(int totalGems)
    {
        ES2.Save(totalGems, "svt.txt?tag=totalGems");
    }

    public void SavePerkLevel(int perkLvl, int perkID)
    {
        ES2.Save(perkLvl, "svt.txt?tag=perkLvl" + perkID);
    }
    // End of - Save Store Class Objects

    // Save Achievements Class Objects    
    public void SaveAchievementValues(int achValue, int achID)
    {
        ES2.Save(achValue, "svt.txt?tag=achValue" + achID);
    }

    public void SaveTotalPlayTime(int quizTime)
    {
        ES2.Save(quizTime, "svt.txt?tag=quizTime");
    }

    public void SaveQCountForTopics(int eachQAnswerCount, string topicName)
    {
        ES2.Save(eachQAnswerCount, "svt.txt?tag=eachQAnswerCount" + topicName);
    }
    // End of - Save Achievements Class Objects
}
