using System.Collections;
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
}
