using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SessionTimerManager : MonoBehaviour
{
    public Color32 beautyRed = new Color32(242, 68, 57, 255);
    public Color32 beautyGreen = new Color32(0, 151, 136, 255);

    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Slider timerSlider;
    [SerializeField]
    private GameObject slideFillObj;
    private CurrSessionManager currSessionManager;

    private Color fillObjColor;

    public bool answerIsFake = false;
    private bool canStartTimerSubtracting = false;
    private bool timerWarning = false;
    private bool timerStateChanged = false; 
    private float totalTime;
    private float wordsPerSecond = 1.2f;
    private float fakeAnswerPercentage = 0.999f;

    private void Awake()
    {
        currSessionManager = gameObject.GetComponent<CurrSessionManager>();
    }

    private void Update()
    {
        TimerStateManager();
    }

    private void TimerStateManager()
    {
        if (canStartTimerSubtracting == true && currSessionManager.answerSelected == false)
        {
            RemainingTimeCalculator();

            if (totalTime <= 10f)
            {
                timerWarning = true;
            }

            if (totalTime > fakeAnswerPercentage * timerSlider.maxValue)
            {
                timerText.color = Color.white;

                answerIsFake = true;
                timerWarning = false;
                currSessionManager.canClickAnswer = false;
                // If a button is clicked, the User Randomly answered a question
                // Show that the question was answered too quickly
            }

            if (totalTime <= (fakeAnswerPercentage * timerSlider.maxValue) && totalTime > 0)
            {
                currSessionManager.canClickAnswer = true;
                answerIsFake = false;
            }

            if (totalTime <= 0)
            {
                currSessionManager.CurrQManager();
                currSessionManager.canClickAnswer = false;
                currSessionManager.canClickNext = true;
                canStartTimerSubtracting = false;
            }
        }
        
        if (timerWarning == true && timerStateChanged == false) // Enable Warning 
        {
            ChangeTimerText();
            timerStateChanged = true;
        }
        else if (timerWarning == false && timerStateChanged == true) // Disable Warning
        {  
            ChangeTimerText();
            timerStateChanged = false;
        }    
    }

    // Calculate the time necessary for answering the current question
    public void DoTimerCalculations(QuestionData currQData)
    {
        int difcRate = currQData.DifficultyRate;
        string totalString = currQData.Question + currQData.A1 + currQData.A2 + currQData.A3 + currQData.A4;

        int numOfWords = totalString.Count(item => item.Equals(' ')) + 5;

        float timeAdder = (0.2f * currQData.DifficultyRate);

        totalTime = 5 * (int) (numOfWords * (wordsPerSecond + timeAdder) / 5);
        timerText.text = totalTime.ToString();
        timerSlider.maxValue = totalTime;

        canStartTimerSubtracting = true;
    }

    // Calculates the timer for the current frame and visualises it with a slider and a text
    public void RemainingTimeCalculator()
    {
        totalTime -= 1 * Time.deltaTime;
        fillObjColor = Color.Lerp(beautyRed, beautyGreen, totalTime / timerSlider.maxValue);

        timerSlider.value = totalTime;
        timerText.text = Math.Ceiling(totalTime).ToString();
        slideFillObj.GetComponent<Image>().color = fillObjColor;
    }

    // Changes the timer text whenever the timer is about to end and back up when it is not
    public void ChangeTimerText()
    {
        Color32 textColor = beautyRed;

        if (timerText.fontSize == 30)
        {
            timerText.color = textColor;
            timerText.fontSize = 45;
        }
        else if (timerText.fontSize == 45)
        {
            timerText.color = Color.white;
            timerText.fontSize = 30;
        }
        
    }

}
