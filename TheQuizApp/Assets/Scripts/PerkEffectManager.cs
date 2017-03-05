using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEffectManager : MonoBehaviour
{
    private CurrSessionManager currSessionManager;
    private StoreManager storeManager;
    public GameObject helpersPanel;

    private int corrAnsNum = -1;
    private bool perk5050Activated = false;

    private void Awake()
    {
        currSessionManager = gameObject.GetComponent<CurrSessionManager>();
        storeManager = gameObject.GetComponent<StoreManager>();
    }

    // When the user clicks on the helpers button, open/close it
    public void OpenTheHelpers()
    {
        helpersPanel.SetActive(!helpersPanel.activeSelf);
    }

    // Gets the correct answer for the current question
    public void GetCorrectAnswer(int corrAnsNum)
    {
        this.corrAnsNum = corrAnsNum;
    }

    // Update the perk used statuses
    public void UpdatePerksAfterGameEnds()
    {
        perk5050Activated = false;
        storeManager.perk5050Button.interactable = true;
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

            var rand = new System.Random();

            int wrong1 = answerNums[Random.Range(0, 3)];
            answerNums.Remove(wrong1);
            int wrong2 = answerNums[Random.Range(0, 2)];

            currSessionManager.buttons[wrong1 - 1].interactable = false;
            currSessionManager.buttons[wrong2 - 1].interactable = false;

            perk5050Activated = true;
            storeManager.perk5050Button.interactable = false;
        }

        helpersPanel.SetActive(false);
    }
}
