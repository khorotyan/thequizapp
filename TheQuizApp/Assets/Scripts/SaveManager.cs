﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    public void SaveAllQNos()
    {

    }

    public void SaveQuestionLoadChecker(bool theChecker, string topicName)
    {
        ES2.Save(theChecker, "svt.txt?tag=loadChecker" + topicName);
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
}
