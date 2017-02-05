﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPageManager : MonoBehaviour
{
    private SaveManager saveManager;
    private LoadManager loadManager;
    private InfoManager infoManager;
    private SessionXPManager sessionXPManager;

    [SerializeField]
    private GameObject topicsPanel;
    [SerializeField]
    private GameObject questionPanel;
    [SerializeField]
    private GameObject resultsPagePanel;

    [Space(4)]

    [SerializeField]
    private Button closeResultsButton;
    [SerializeField]
    private Text scoreNewIndicator;
    [SerializeField]
    private Text totalMoneyText;
    [SerializeField]
    private Text corrWrongRatioText;
    [SerializeField]
    private Text maxConsecutiveAnssText;

    [Space(4)]

    [SerializeField]
    private Slider XPSlider;
    [SerializeField]
    private Text SessionXPText;
    [SerializeField]
    private Text CurrLevelText;
    [SerializeField]
    private Text RemainingXPText;
    [SerializeField]
    private Text TotalXPText;

    private bool canStartXPAnimation = false;
    private float curInterPolTime = 0f;
    private float interpolAdder = 0.3f;

    private float totalXPCopy;
    private float currSessXPCopy;
    private float remainingXPCopy;
    private float xpPastLevelCopy;

    private void Awake()
    {
        closeResultsButton.onClick.AddListener(OnResultsPageClose);

        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();
        infoManager = gameObject.GetComponent<InfoManager>();
        sessionXPManager = gameObject.GetComponent<SessionXPManager>();
    }

    private void Update()
    {
        UpdateXPInfo();
    }

    /*
    float time = 0f;

    private void Update()
    {
        UpdateXPSlider(150);
    }

    private void UpdateXPSlider(int addedXP)
    {
        XPSlider.value = Mathf.Lerp(XPSlider.value, addedXP, time);
        time += Time.deltaTime / 20;
    }
    */
    
    // Update the results in the Results panel
    public void UpdateResultsPage(string topicName, int totalMoney, int sessionQCount, int numOfCorrAnss, int maxConsCorrAnss)
    {
        ShowXPInfo();
        SaveSessionInfo(topicName);       

        scoreNewIndicator.gameObject.SetActive(DetermineIfIsNewScore(totalMoney) == true);
        totalMoneyText.text = totalMoney.ToString();
        corrWrongRatioText.text = numOfCorrAnss + " : " + (sessionQCount - numOfCorrAnss);
        maxConsecutiveAnssText.text = maxConsCorrAnss.ToString();

        questionPanel.SetActive(false);
        resultsPagePanel.SetActive(true);
    }

    // Check if the user got a new high score
    private bool DetermineIfIsNewScore(int totalMoney)
    {
        int highestScore = loadManager.LoadAllTimeMaxScore();

        if (totalMoney > highestScore)
        {
            saveManager.SaveAllTimeMaxScore(totalMoney);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnResultsPageClose()
    {
        resultsPagePanel.SetActive(false);
        topicsPanel.SetActive(true);
    }

    private void SaveSessionInfo(string topicName)
    {
        List<int> notAnsweredQNos = (infoManager.notAnsweredQs).Select(item => item.QuestionNumber).ToList();
        saveManager.SaveNotAnsweredQNumbers(notAnsweredQNos, topicName);

        List<int> wrongAnsweredQNos = (infoManager.wrongAnsweredQs).Select(item => item.QuestionNumber).ToList();
        saveManager.SaveWrongAnsweredQNumbers(wrongAnsweredQNos, topicName);

        List<int> answeredQNos = (infoManager.answeredQs).Select(item => item.QuestionNumber).ToList();
        saveManager.SaveAnsweredQNumbers(answeredQNos, topicName);
    }

    // Get and show XP panel information
    private void ShowXPInfo()
    {
        sessionXPManager.LoadTotalXPNLevels();

        totalXPCopy = SessionXPManager.totalXP;
        currSessXPCopy = SessionXPManager.currLevel;
        remainingXPCopy = sessionXPManager.CalcRemainingXP();
        xpPastLevelCopy = sessionXPManager.CalcXPPastLevel();   

        XPSlider.maxValue = SessionXPManager.levelXPs[SessionXPManager.currLevel + 1] - SessionXPManager.levelXPs[SessionXPManager.currLevel];
        XPSlider.value = xpPastLevelCopy;

        TotalXPText.text = SessionXPManager.totalXP.ToString() + " XP";
        CurrLevelText.text = SessionXPManager.currLevel.ToString();
        SessionXPText.text = "+ " + SessionXPManager.currSessXP.ToString() + " XP";
        RemainingXPText.text = SessionXPManager.remainingXP.ToString() + " XP\n" + "to Level " + (SessionXPManager.currLevel + 1).ToString();

        StartCoroutine(WaitBeforeXPAnim());
    }

    // Update the XP info by adding current session xp to the total one and updating other values accordingly
    private void UpdateXPInfo()
    {
        if (canStartXPAnimation == true)
        {
            int nextLvlXP = SessionXPManager.levelXPs[SessionXPManager.currLevel + 1];
            int currLvlXP = SessionXPManager.levelXPs[SessionXPManager.currLevel];

            currSessXPCopy = Mathf.Lerp(SessionXPManager.currSessXP, 0f, curInterPolTime);
            totalXPCopy = Mathf.Lerp(SessionXPManager.totalXP, SessionXPManager.totalXP + SessionXPManager.currSessXP, curInterPolTime);
            remainingXPCopy = Mathf.Lerp(SessionXPManager.remainingXP, SessionXPManager.remainingXP - SessionXPManager.currSessXP, curInterPolTime);
            xpPastLevelCopy = Mathf.Lerp(SessionXPManager.xpPastLevel, SessionXPManager.xpPastLevel + SessionXPManager.currSessXP, curInterPolTime);

            XPSlider.value = xpPastLevelCopy;
            TotalXPText.text = (Mathf.Ceil(totalXPCopy)).ToString() + " XP";           
            SessionXPText.text = "+ " + (Mathf.Ceil(currSessXPCopy)).ToString() + " XP";
            RemainingXPText.text = (Mathf.Ceil(remainingXPCopy)).ToString() + " XP\n" + "to Level " + (SessionXPManager.currLevel + 1).ToString();

            curInterPolTime += interpolAdder * Time.deltaTime;

            // If the player levels up
            if (totalXPCopy >= nextLvlXP)
            {
                SessionXPManager.currLevel++;
                nextLvlXP = SessionXPManager.levelXPs[SessionXPManager.currLevel + 1];
                currLvlXP = SessionXPManager.levelXPs[SessionXPManager.currLevel];

                SessionXPManager.xpPastLevel = 0;
                SessionXPManager.remainingXP = nextLvlXP - currLvlXP;
                SessionXPManager.currSessXP = SessionXPManager.totalXP + SessionXPManager.currSessXP - currLvlXP;
                SessionXPManager.totalXP = currLvlXP;

                XPSlider.value = 0;            
                XPSlider.maxValue = SessionXPManager.remainingXP;
                CurrLevelText.text = SessionXPManager.currLevel.ToString();
                RemainingXPText.text = SessionXPManager.remainingXP.ToString() + " XP\n" + "to Level " + (SessionXPManager.currLevel + 1).ToString();

                // T.past = S / V.0 , T.rem =  1 / V.0 , V.1 = 1 / T.rem => V.1 = V.0 / (1 - S) 
                //      we take 1, as interpolation is between 0 and 1, thus our new S is 1
                interpolAdder = interpolAdder / (1f - curInterPolTime);
                curInterPolTime = 0;

                canStartXPAnimation = false;
                StartCoroutine(WaitBeforeXPAnim());
            }
            
            // If the interpolation finished 
            if (curInterPolTime > 1f)
            {
                SessionXPManager.totalXP += SessionXPManager.currSessXP;
                SessionXPManager.remainingXP = sessionXPManager.CalcRemainingXP();
                SessionXPManager.xpPastLevel = sessionXPManager.CalcXPPastLevel();

                XPSlider.value = SessionXPManager.xpPastLevel;
                TotalXPText.text = SessionXPManager.totalXP.ToString() + " XP";
                SessionXPText.text = ""; 
                RemainingXPText.text = SessionXPManager.remainingXP.ToString() + " XP\n" + "to Level " + (SessionXPManager.currLevel + 1).ToString();

                sessionXPManager.SaveTotalXPNLevels();
                canStartXPAnimation = false;
                SessionXPManager.currSessXP = 0;

                interpolAdder = 0.3f;
                curInterPolTime = 0f;
            }
        }
    }

    // Wait for some time before playing the XP update animation
    IEnumerator WaitBeforeXPAnim()
    {
        yield return new WaitForSeconds(0.5f);
        canStartXPAnimation = true;
    }
}