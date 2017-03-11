using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkEffectManager : MonoBehaviour
{
    private CurrSessionManager currSessionManager;
    private StoreManager storeManager;
    private SessionTimerManager sessionTimerManager;
    public GameObject helpersMain;

    private Color32 defButtonCol;
    public Text bonusTimeText;

    private int storedTime = 0;
    private float t = 0;
    private float helperPageCounter = 0;
    private int corrAnsNum = -1;
    private bool helperIsActive = false;
    private bool isInterpolating = false;
    private bool perk5050Activated = false;

    private void Awake()
    {
        currSessionManager = gameObject.GetComponent<CurrSessionManager>();
        storeManager = gameObject.GetComponent<StoreManager>();
        sessionTimerManager = gameObject.GetComponent<SessionTimerManager>();

        defButtonCol = currSessionManager.buttons[0].image.color;
    }

    private void Update()
    {
        OpenHelpersAnimation();
    }

    private void OpenHelpersAnimation()
    {
        if (isInterpolating == true && helperIsActive == false)
        {
            float currPosY = Mathf.Lerp(1150, 650, t);
            t += 3f * Time.deltaTime;

            helpersMain.GetComponent<RectTransform>().anchoredPosition = new Vector3(helpersMain.GetComponent<RectTransform>().anchoredPosition.x, currPosY, 0);          

            if (t > 1)
            {
                t = 0;
                isInterpolating = false;
                helperIsActive = true;
            }
        }
        else if (isInterpolating == true && helperIsActive == true)
        {
            float currPosY = Mathf.Lerp(650, 1150, t);
            t += 3f * Time.deltaTime;

            helpersMain.GetComponent<RectTransform>().anchoredPosition = new Vector3(helpersMain.GetComponent<RectTransform>().anchoredPosition.x, currPosY, 0);

            if (t > 1)
            {
                t = 0;
                helperPageCounter = 0;
                isInterpolating = false;
                helperIsActive = false;
            }
        }
        
        // If the helpers panel is open for 10 seconds, close it

        if (helperIsActive == true)
        {
            helperPageCounter += 1 * Time.deltaTime;

            if (helperPageCounter > 6)
            {
                helperPageCounter = 0;
                isInterpolating = true;     
            }
        }
    }

    // When the user clicks on the helpers button, open/close it
    public void OpenTheHelpers()
    {
        if (helperIsActive == false)
        {           
            isInterpolating = true;
        }
        else
        {
            isInterpolating = true;
        }
    }

    // Gets the correct answer for the current question
    public void GetCorrectAnswer(int corrAnsNum)
    {
        this.corrAnsNum = corrAnsNum;
    }

    // Update the perk used statuses
    public void UpdatePerksAfterGameEnds()
    {
        storedTime = 0;
        perk5050Activated = false;
        storeManager.perk5050Button.interactable = true;
        Color32 imgCol = storeManager.perk5050Button.image.color;
        storeManager.perk5050Button.image.color = new Color32(imgCol.r, imgCol.g, imgCol.b, 100);
    }

    // When the user clicks on the 50/50 perk, activate it if not previously done
    public void On5050PerkClick()
    {
        if (corrAnsNum != -1 && perk5050Activated == false)
        {
            List<int> answerNums = new List<int>();
            answerNums.Add(1);
            answerNums.Add(2);
            answerNums.Add(3);
            answerNums.Add(4);

            answerNums.Remove(corrAnsNum);

            int wrong1 = answerNums[UnityEngine.Random.Range(0, 3)];
            answerNums.Remove(wrong1);
            int wrong2 = answerNums[UnityEngine.Random.Range(0, 2)];

            Color32 imageColor = currSessionManager.buttons[0].image.color;

            currSessionManager.buttons[wrong1 - 1].interactable = false;
            currSessionManager.buttons[wrong1 - 1].image.color = new Color32(imageColor.r, imageColor.g, imageColor.b, 20);

            currSessionManager.buttons[wrong2 - 1].interactable = false;
            currSessionManager.buttons[wrong2 - 1].image.color = new Color32(imageColor.r, imageColor.g, imageColor.b, 20);

            perk5050Activated = true;
            storeManager.perk5050Button.interactable = false;
            Color32 imgCol = storeManager.perk5050Button.image.color;
            storeManager.perk5050Button.image.color = new Color32(imgCol.r, imgCol.g, imgCol.b, 20);
        }

        isInterpolating = true; // DO or undo the helper panel animation
    }

    // Removes the effects of 5050
    private void De5050fy()
    {
        if (currSessionManager.buttons[0].interactable == false)
            currSessionManager.buttons[0].interactable = true;
        if (currSessionManager.buttons[1].interactable == false)
            currSessionManager.buttons[1].interactable = true;
        if (currSessionManager.buttons[2].interactable == false)
            currSessionManager.buttons[2].interactable = true;
        if (currSessionManager.buttons[3].interactable == false)
            currSessionManager.buttons[3].interactable = true;

        if (currSessionManager.buttons[0].image.color != defButtonCol)
            currSessionManager.buttons[0].image.color = defButtonCol;
        if (currSessionManager.buttons[1].image.color != defButtonCol)
            currSessionManager.buttons[1].image.color = defButtonCol;
        if (currSessionManager.buttons[2].image.color != defButtonCol)
            currSessionManager.buttons[2].image.color = defButtonCol;
        if (currSessionManager.buttons[3].image.color != defButtonCol)
            currSessionManager.buttons[3].image.color = defButtonCol;
    }

    public void RemovePerkEffects()
    {
        De5050fy();
    }

    public void AccumulateTime(float currTime, float totalTime, float answerPercent, float bonusPercent)
    {
        if (1 - currTime / totalTime < answerPercent)
        {
            storedTime += (int) Math.Round((currTime) * bonusPercent);
        }
        
        bonusTimeText.text = "Adds " + storedTime + " seconds to the timer";
    }

    public void OnBonusTimePerkClick()
    {
        sessionTimerManager.totalTime += storedTime;
        storedTime = 0;
        bonusTimeText.text = "Adds " + storedTime + " seconds to the timer";

        isInterpolating = true; // DO or undo the helper panel animation
    }
}
