using System.Collections;
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

    private int currQPrice = 2; // After the perk upgrade, it will defer depending on Q difficulty
    private int consecutiveCorrAnss = 0;
    private float comboMultiplier = 1;
    private int questionNumber = 0;
    private int wrongAnswerCount = 0;
    private float protectedAmount = 0.0f; 

    private int totalMoney = 0;
    private int currMoney; 
    private float multiplier;
    private int deduceAmount;

    // Do the calculations for the case of answering correctly
    public void CurrencyOnCorrAns()
    {
        totalMoney += Mathf.CeilToInt(currMoney * multiplier);
        consecutiveCorrAnss++;
    }
    
    // Do the calculations for the case of answering incorrectly
    public void CurrencyOnIncorrAns()
    {  
        totalMoney = Mathf.CeilToInt(protectedAmount * totalMoney) + Mathf.CeilToInt((1 - protectedAmount) * totalMoney) / deduceAmount;
        wrongAnswerCount++;
        consecutiveCorrAnss = 0;
    }

    // Do the calculations for the current question
    public void CurrencyCalculations()
    {
        // The amount of money that the user can get this round by answering correctly
        currMoney = Mathf.CeilToInt(Mathf.Sqrt(currQPrice * Mathf.Pow(questionNumber + 1, currQPrice) + 2 * currQPrice));

        // The multiplier for the amount of currency for the current question
        multiplier = 1 + (comboMultiplier * consecutiveCorrAnss) * 0.2f;

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
}
