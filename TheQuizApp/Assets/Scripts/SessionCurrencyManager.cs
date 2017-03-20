using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SessionCurrencyManager : MonoBehaviour
{   
    [SerializeField]
    private Text totalMoneyText;
    [SerializeField]
    private Text currMoneyText;
    [SerializeField]
    private Text multiplierText;

    private SessionXPManager sessionXPManager;
    private LoadManager loadManager;
    
    private int currQPrice = 3; // After the perk upgrade, it will change depending on Q difficulty
    private int consecutiveCorrAnss = 0;
    private int questionNumber = 0;
    private int wrongAnswerCount = 0;
    public static float moneyMultiplier = 1;
    public static float comboMultiplier = 0.2f;
    public static float protectedAmount = 0.0f; 

    [HideInInspector]
    public int sessTotalMoney = 0;
    [HideInInspector]
    public int maxConCorAns = 0;

    private int currMoney; 
    private float multiplier;
    private int deduceAmount;
    private QIncomeReductionInfo[] qirInfo = new QIncomeReductionInfo[8];

    private void Awake()
    {
        sessionXPManager = gameObject.GetComponent<SessionXPManager>();
        loadManager = gameObject.GetComponent<LoadManager>();

        AllQIncomeReductionInfo();
    }

    private void AllQIncomeReductionInfo()
    {
        qirInfo[0] = new QIncomeReductionInfo(100, .90f);
        qirInfo[1] = new QIncomeReductionInfo(200, .85f);
        qirInfo[2] = new QIncomeReductionInfo(300, .75f);
        qirInfo[3] = new QIncomeReductionInfo(400, .65f);
        qirInfo[4] = new QIncomeReductionInfo(500, .50f);
        qirInfo[5] = new QIncomeReductionInfo(600, .40f);
        qirInfo[6] = new QIncomeReductionInfo(700, .30f);
        qirInfo[7] = new QIncomeReductionInfo(800, .20f);
    }

    // Do the calculations for the case of answering correctly
    public void CurrencyOnCorrAns()
    {
        sessionXPManager.AddXPOnCorrect(consecutiveCorrAnss);

        sessTotalMoney += (int) Math.Ceiling(currMoney * multiplier);
        consecutiveCorrAnss++;
        maxConCorAns = consecutiveCorrAnss > maxConCorAns ? consecutiveCorrAnss : maxConCorAns;
    }
    
    // Do the calculations for the case of answering incorrectly
    public void CurrencyOnIncorrAns()
    {
        sessionXPManager.AddXPOnWrong();

        sessTotalMoney = (int) Math.Ceiling( Math.Ceiling(protectedAmount * sessTotalMoney) + Math.Ceiling((1 - protectedAmount) * sessTotalMoney) / deduceAmount );
        wrongAnswerCount++;
        consecutiveCorrAnss = 0;
    }

    // Do the calculations for the current question
    public void CurrencyCalculations(string topicName)
    {
        // The amount of money that the user can get this round by answering correctly
        currMoney = (int) Math.Ceiling(Mathf.Sqrt(currQPrice * Mathf.Pow(questionNumber + 1, currQPrice) + 2 * currQPrice) * moneyMultiplier);
        currMoney = (int) (0.5f + ReduceIncome(topicName) * currMoney); // Reduce income if the same topic is answered too much

        // The multiplier for the amount of currency for the current question
        multiplier = 1 + consecutiveCorrAnss * comboMultiplier;

        // The amount that will be reduces if answered wrongly
        deduceAmount = wrongAnswerCount + 2;

        ChangeVisualStuff();

        questionNumber++;
    }

    public void ChangeVisualStuff()
    {
        totalMoneyText.text = sessTotalMoney.ToString();
        currMoneyText.text = currMoney.ToString();
        multiplierText.text = multiplier.ToString();
    }

    public void BeforeANewGame()
    {
        consecutiveCorrAnss = 0;
        questionNumber = 0;
        wrongAnswerCount = 0;
        sessTotalMoney = 0;
        maxConCorAns = 0;
    }

    private float ReduceIncome(string topicName)
    {
        int totalQAnsCount = -1;
        totalQAnsCount = loadManager.LoadQCountForTopics(topicName);

        for (int i = qirInfo.Length - 1; i >= 0; i--)
        {
            if (totalQAnsCount != -1)
            {
                if (totalQAnsCount > qirInfo[i].questionCount)
                {
                    return qirInfo[i].incomePercent;
                }
            }
        }

        return 1f;
    }
}

class QIncomeReductionInfo
{
    public int questionCount;
    public float incomePercent;

    public QIncomeReductionInfo(int questionCount, float incomePercent)
    {
        this.questionCount = questionCount;
        this.incomePercent = incomePercent;
    }
}