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
    
    private int currQPrice = 3; // After the perk upgrade, it will change depending on Q difficulty
    private int consecutiveCorrAnss = 0;
    private float comboMultiplier = 0.2f;
    private float moneyMultiplier = 1;
    private int questionNumber = 0;
    private int wrongAnswerCount = 0;
    private float protectedAmount = 0.0f; 

    [HideInInspector]
    public int totalMoney = 0;
    [HideInInspector]
    public int maxConCorAns = 0;
    private int currMoney; 
    private float multiplier;
    private int deduceAmount;

    private void Awake()
    {
        sessionXPManager = gameObject.GetComponent<SessionXPManager>();
    }

    // Do the calculations for the case of answering correctly
    public void CurrencyOnCorrAns()
    {
        sessionXPManager.AddXPOnCorrect(consecutiveCorrAnss);

        totalMoney += (int) Math.Ceiling(currMoney * multiplier);
        consecutiveCorrAnss++;
        maxConCorAns = consecutiveCorrAnss > maxConCorAns ? consecutiveCorrAnss : maxConCorAns;
    }
    
    // Do the calculations for the case of answering incorrectly
    public void CurrencyOnIncorrAns()
    {
        sessionXPManager.AddXPOnWrong();

        totalMoney = (int) Math.Ceiling( Math.Ceiling(protectedAmount * totalMoney) + Math.Ceiling((1 - protectedAmount) * totalMoney) / deduceAmount );
        wrongAnswerCount++;
        consecutiveCorrAnss = 0;
    }

    // Do the calculations for the current question
    public void CurrencyCalculations()
    {
        // The amount of money that the user can get this round by answering correctly
        currMoney = (int) Math.Ceiling(Mathf.Sqrt(currQPrice * Mathf.Pow(questionNumber + 1, currQPrice) + 2 * currQPrice) * moneyMultiplier);

        // The multiplier for the amount of currency for the current question
        multiplier = 1 + consecutiveCorrAnss * comboMultiplier;

        // The amount that will be reduces if answered wrongly
        deduceAmount = wrongAnswerCount + 2;

        ChangeVisualStuff();

        questionNumber++;
    }

    public void ChangeVisualStuff()
    {
        totalMoneyText.text = totalMoney.ToString();
        currMoneyText.text = currMoney.ToString();
        multiplierText.text = multiplier.ToString();
    }

    public void BeforeANewGame()
    {
        consecutiveCorrAnss = 0;
        comboMultiplier = 0.2f; // Later assign the value from the saved file
        questionNumber = 0;
        wrongAnswerCount = 0;
        totalMoney = 0;
        maxConCorAns = 0;
    }
}
