﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    public void Load()
    {

    }

    public bool LoadQuestionLoadChecker(string topicName)
    {
        bool theChecker = false;

        if (ES2.Exists("svt.txt?tag=loadChecker" + topicName))
        {
            theChecker = ES2.Load<bool>("svt.txt?tag=loadChecker" + topicName);
        }

        return theChecker;
    }

    public int LoadNumberOfQuestions(string topicName)
    {
        int totalQuestions = 0;

        if (ES2.Exists("svt.txt?tag=totalQuestions" + topicName))
        {
            totalQuestions = ES2.Load<int>("svt.txt?tag=totalQuestions" + topicName);
        }

        return totalQuestions;
    }

    public List<int> LoadNotAnsweredQNumbers(string topicName)
    {
        List<int> notAnsweredQNos = new List<int>();

        if (ES2.Exists("svt.txt?tag=notAnsweredQNos" + topicName))
        {
            notAnsweredQNos = ES2.LoadList<int>("svt.txt?tag=notAnsweredQNos" + topicName);
        }

        return notAnsweredQNos;
    }

    public List<int> LoadWrongAnsweredQNumbers(string topicName)
    {
        List<int> wrongAnsweredQNos = new List<int>();

        if (ES2.Exists("svt.txt?tag=wrongAnsweredQNos" + topicName))
        {
            wrongAnsweredQNos = ES2.LoadList<int>("svt.txt?tag=wrongAnsweredQNos" + topicName);
        }

        return wrongAnsweredQNos;
    }

    public List<int> LoadAnsweredQNumbers(string topicName)
    {
        List<int> answeredQNos = new List<int>();

        if (ES2.Exists("svt.txt?tag=answeredQNos" + topicName))
        {
            answeredQNos = ES2.LoadList<int>("svt.txt?tag=answeredQNos" + topicName);
        }

        return answeredQNos;
    }

    public int LoadAllTimeMaxScore()
    {
        int allTimeMaxScore = 0;

        if (ES2.Exists("svt.txt?tag=allTimeMaxScore"))
        {
            allTimeMaxScore = ES2.Load<int>("svt.txt?tag=allTimeMaxScore");
        }

        return allTimeMaxScore;       
    }

    // Load XP Class Objects
    public int LoadCurrentLevel()
    {
        int currLevel = 0;

        if (ES2.Exists("svt.txt?tag=currLevel"))
        {
            currLevel = ES2.Load<int>("svt.txt?tag=currLevel");
        }

        return currLevel;
    }

    public int LoadTotalXP()
    {
        int totalXP = 0;

        if (ES2.Exists("svt.txt?tag=totalXP"))
        {
            totalXP = ES2.Load<int>("svt.txt?tag=totalXP");
        }

        return totalXP;
    }
    // End of - Load XP Class Objects

    // Load Store Class Objects
    public int LoadTotalMoney()
    {
        int totalMoney = 0;

        if (ES2.Exists("svt.txt?tag=totalMoney"))
        {
            totalMoney = ES2.Load<int>("svt.txt?tag=totalMoney");
        }

        return totalMoney;
    }

    public int LoadTotalGems()
    {
        int totalGems = 0;

        if (ES2.Exists("svt.txt?tag=totalGems"))
        {
            totalGems = ES2.Load<int>("svt.txt?tag=totalGems");
        }

        return totalGems;
    }

    public int LoadPerkLevel(int perkID)
    {
        int perkLvl = 0;

        if (ES2.Exists("svt.txt?tag=perkLvl" + perkID))
        {
            perkLvl = ES2.Load<int>("svt.txt?tag=perkLvl" + perkID);
        }

        return perkLvl;
    }
    // End of - Load Store Class Objects

    // Load Achievements Class Objects    
    public int LoadAchievementValues(int achID)
    {
        int achValue = 0;

        if (ES2.Exists("svt.txt?tag=achValue" + achID))
        {
            achValue = ES2.Load<int>("svt.txt?tag=achValue" + achID);
        }

        return achValue;
    }

    public int LoadTotalPlayTime()
    {
        int quizTime = 0;

        if (ES2.Exists("svt.txt?tag=quizTime"))
        {
            quizTime = ES2.Load<int>("svt.txt?tag=quizTime");
        }

        return quizTime;
    }

    public int LoadQCountForTopics(string topicName)
    {
        int eachQAnswerCount = 0;

        if (ES2.Exists("svt.txt?tag=eachQAnswerCount" + topicName))
        {
            eachQAnswerCount = ES2.Load<int>("svt.txt?tag=eachQAnswerCount" + topicName);
        }

        return eachQAnswerCount;
    }
    // End of - Load Achievements Class Objects
}
