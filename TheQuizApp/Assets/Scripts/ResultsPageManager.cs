using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsPageManager : MonoBehaviour
{
    private SaveManager saveManager;
    private LoadManager loadManager;

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

    private void Awake()
    {
        closeResultsButton.onClick.AddListener(OnResultsPageClose);

        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();
    }

    // Update the results in the Results panel
    public void UpdateResultsPage(int totalMoney, int sessionQCount, int numOfCorrAnss, int maxConsCorrAnss)
    {
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
}
