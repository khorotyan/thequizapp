using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public GameObject mainMenuPage;
    public GameObject achievementsPage;

    public Transform achObjContainer;
    private Transform[] achievementObjs = new Transform[20];

    private SaveManager saveManager;
    private LoadManager loadManager;

    private AchievementItemInfo[] achMoneyMakerInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achSwiftSwiftInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achLearnerInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achProgressInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achToughInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achTougherInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achMasterQuizerInfo = new AchievementItemInfo[3];

    public static int achMoneyMakerValue = 0;
    public static int achSwiftSwiftValue = 0;
    public static int achLearnerValue = 0;
    public static int achProgressValue = 0;
    public static int achToughValue = 0;
    public static int achTougherValue = 0;
    public static int achMasterQuizerValue = 0;

    public bool canEnableTimer = false;
    private int quizTime = 0;
    private float timerStartTime = 0;
    private float timerEndtTime = 0;

    private void Awake()
    {
        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();

        AllAchievementInfo();
        UpdateAllAchInfo();
    }

    private void AllAchievementInfo()
    {
        for (int i = 0; i < achObjContainer.childCount; i++)
        {
            achievementObjs[i] = achObjContainer.GetChild(i);
        }

        // MoneyMaker Achievement
        achMoneyMakerInfo[0] = new AchievementItemInfo(1000, 50, 2);
        achMoneyMakerInfo[1] = new AchievementItemInfo(20000, 1250, 6);
        achMoneyMakerInfo[2] = new AchievementItemInfo(150000, 7625, 15);

        // SwiftSwift Achievement
        achSwiftSwiftInfo[0] = new AchievementItemInfo(175, 65, 3);
        achSwiftSwiftInfo[1] = new AchievementItemInfo(550, 1425, 8);
        achSwiftSwiftInfo[2] = new AchievementItemInfo(1125, 8500, 20);

        // Learner Achievement
        achLearnerInfo[0] = new AchievementItemInfo(600, 50, 2);
        achLearnerInfo[1] = new AchievementItemInfo(2000, 1250, 6);
        achLearnerInfo[2] = new AchievementItemInfo(7000, 7625, 15);

        // Progress Achievement
        achProgressInfo[0] = new AchievementItemInfo(3, 50, 2);
        achProgressInfo[1] = new AchievementItemInfo(10, 1250, 6);
        achProgressInfo[2] = new AchievementItemInfo(20, 7625, 15);

        // Tough Achievement
        achToughInfo[0] = new AchievementItemInfo(45, 65, 3);
        achToughInfo[1] = new AchievementItemInfo(240, 1425, 8);
        achToughInfo[2] = new AchievementItemInfo(850, 8500, 20);

        // Tougher Achievement
        achTougherInfo[0] = new AchievementItemInfo(20, 80, 4);
        achTougherInfo[1] = new AchievementItemInfo(115, 1600, 12);
        achTougherInfo[2] = new AchievementItemInfo(400, 9750, 25);

        // MasterQuizer Achievement
        achMasterQuizerInfo[0] = new AchievementItemInfo(3, 120, 5);
        achMasterQuizerInfo[1] = new AchievementItemInfo(12, 1450, 15);
        achMasterQuizerInfo[2] = new AchievementItemInfo(30, 9999, 30);
    }

    // Load the real achievements data, which is used during the game (not the visual)
    private void LoadAchievementsData()
    {
        achMoneyMakerValue = loadManager.LoadAchievementValues(0);
        achSwiftSwiftValue = loadManager.LoadAchievementValues(1);
        achLearnerValue = loadManager.LoadAchievementValues(2);
        achProgressValue = loadManager.LoadAchievementValues(3);
        achToughValue = loadManager.LoadAchievementValues(4);
        achTougherValue = loadManager.LoadAchievementValues(5);
        achMasterQuizerValue = loadManager.LoadAchievementValues(6);
    }

    private int GetAchItemLevel(AchievementItemInfo[] achInfo, int achValue)
    {
        int level = 0;

        if (achValue >= achInfo[2].goalValue)
        {
            level = 3;
        }
        else if (achValue >= achInfo[1].goalValue)
        {
            level = 2;
        }
        else if (achValue >= achInfo[0].goalValue)
        {
            level = 1;
        }

        return level;
    }

    public void UpdateAllAchInfo()
    {
        LoadAchievementsData();

        int achMoneyMakerLvl = GetAchItemLevel(achMoneyMakerInfo, achMoneyMakerValue);
        int achSwiftSwiftLvl = GetAchItemLevel(achSwiftSwiftInfo, achSwiftSwiftValue);
        int achLearnerLvl = GetAchItemLevel(achLearnerInfo, achLearnerValue);
        int achProgressLvl = GetAchItemLevel(achProgressInfo, achProgressValue);
        int achToughLvl = GetAchItemLevel(achToughInfo, achToughValue);
        int achTougherLvl = GetAchItemLevel(achTougherInfo, achTougherValue);
        int achMasterQuizerLvl = GetAchItemLevel(achMasterQuizerInfo, achMasterQuizerValue);

        UpdateAchievementVisuals(0, achievementObjs[0].GetChild(0), achMoneyMakerInfo, achMoneyMakerValue, achMoneyMakerLvl);
        UpdateAchievementVisuals(1, achievementObjs[1].GetChild(0), achSwiftSwiftInfo, achSwiftSwiftValue, achSwiftSwiftLvl);
        UpdateAchievementVisuals(2, achievementObjs[2].GetChild(0), achLearnerInfo, achLearnerValue, achLearnerLvl);
        UpdateAchievementVisuals(3, achievementObjs[3].GetChild(0), achProgressInfo, achProgressValue, achProgressLvl);
        UpdateAchievementVisuals(4, achievementObjs[4].GetChild(0), achToughInfo, achToughValue, achToughLvl);
        UpdateAchievementVisuals(5, achievementObjs[5].GetChild(0), achTougherInfo, achTougherValue, achTougherLvl);
        UpdateAchievementVisuals(6, achievementObjs[6].GetChild(0), achMasterQuizerInfo, achMasterQuizerValue, achMasterQuizerLvl);
    }

    private void UpdateAchievementVisuals(int ID, Transform achObj, AchievementItemInfo[] achInfo, int achValue, int achLevel)
    {
        Color32 activeColor = achObj.GetChild(0).GetChild(0).GetComponent<Image>().color;
        
        // Setup The Level indicator of the achievement
        if (achLevel == 1)
        {        
            achObj.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(activeColor.r, activeColor.g, activeColor.b, 175);
        }
        else if (achLevel == 2)
        {
            achObj.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(activeColor.r, activeColor.g, activeColor.b, 175);
            achObj.GetChild(0).GetChild(1).GetComponent<Image>().color = new Color32(activeColor.r, activeColor.g, activeColor.b, 175);
        }
        else if (achLevel == 3)
        {
            achObj.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(activeColor.r, activeColor.g, activeColor.b, 175);
            achObj.GetChild(0).GetChild(1).GetComponent<Image>().color = new Color32(activeColor.r, activeColor.g, activeColor.b, 175);
            achObj.GetChild(0).GetChild(2).GetComponent<Image>().color = new Color32(activeColor.r, activeColor.g, activeColor.b, 175);
        }
        // End of - Setup The Level indicator of the achievement

        if (achLevel != 3)
        { 
            // Update the information about the perk
            if (ID == 0)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Make a total of " + achInfo[achLevel].goalValue + " gold";
            }               
            else if (ID == 1)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Answer " + achInfo[achLevel].goalValue + " questions using less than 25% of the time";
            }
            else if (ID == 2)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Answer a total of " + achInfo[achLevel].goalValue + " questions";
            }
            else if (ID == 3)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Answer 100 questions in " + achInfo[achLevel].goalValue + " topics";
            }
            else if (ID == 4)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Answer 3 consecutive questions correctly " + achInfo[achLevel].goalValue + " times";
            }
            else if (ID == 5)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Answer 5 consecutive questions correctly " + achInfo[achLevel].goalValue + " times";
            }
            else if (ID == 6)
            {
                achObj.GetChild(1).GetChild(0).GetComponent<Text>().text = "Have a total of " + achInfo[achLevel].goalValue + " hours of quiztime";
            }
            // End of - Update the information about the perk

            achObj.GetChild(2).GetChild(0).GetChild(0).GetComponent<Text>().text = achValue + " / " + achInfo[achLevel].goalValue;
            achObj.GetChild(2).GetChild(0).GetChild(1).GetComponent<Text>().text = achInfo[achLevel].xpReward + " XP";
            achObj.GetChild(2).GetChild(0).GetChild(2).GetComponent<Text>().text = achInfo[achLevel].gemReward + " Gems";

        }
        else // Show that the achievement was completed
        {
            achObj.GetChild(2).GetChild(0).gameObject.SetActive(false);
            achObj.GetChild(2).GetChild(1).gameObject.SetActive(true);
        }
    }

    //
    // The Actual Achievement System
    // 

    public void UpdateMoneyMakerValue(int addition) // ID = 0
    {
        achMoneyMakerValue += addition;

        saveManager.SaveAchievementValues(achMoneyMakerValue, 0);
    }

    public void UpdateSwiftSwiftValue() // ID 1
    {
        achSwiftSwiftValue++;

        saveManager.SaveAchievementValues(achSwiftSwiftValue, 1);
    }

    public void UpdateLearnerValue(int noQs) // ID = 2
    {
        achLearnerValue += noQs;

        saveManager.SaveAchievementValues(achLearnerValue, 2);
    }

    public void UpdateProgressValue(int noQs, string topicName) // ID = 3
    {
        noQs += loadManager.LoadQCountForTopics(topicName);
        saveManager.SaveQCountForTopics(noQs, topicName);

        string[] tagsInFile = ES2.GetTags("svt.txt");
        List<string> topics = new List<string>();
        int topics100Ans = 0;

        // Load all the topic names associated with Progress achievement
        foreach (string item in tagsInFile)
        {
            if (item.Contains("eachQAnswerCount"))
            {
                topics.Add(item.Substring("eachQAnswerCount".Length));
            }
        }

        // Check the number of topics having 100+ questions answered
        for (int i = 0; i < topics.Count; i++)
        {
            int qCount = loadManager.LoadQCountForTopics(topics[i]);

            if (qCount >= 100)
            {
                topics100Ans++;
            }
        }

        achProgressValue = topics100Ans;

        saveManager.SaveAchievementValues(achProgressValue, 3);
    }

    public void UpdateToughValue() // ID = 4
    {
        achToughValue++;

        saveManager.SaveAchievementValues(achToughValue, 4);
    }

    public void UpdateTougherValue() // ID = 5
    {
        achTougherValue++;

        saveManager.SaveAchievementValues(achTougherValue, 5);
    }

    public void UpdateMasterQuizerValue() // ID = 6 
    {
        quizTime = loadManager.LoadTotalPlayTime();

        quizTime += (int) (0.5f + timerEndtTime - timerStartTime);

        achMasterQuizerValue = quizTime / 3600;

        saveManager.SaveTotalPlayTime(quizTime);
        saveManager.SaveAchievementValues(achMasterQuizerValue, 6);
    }

    public void GetAchTimerStartTime(float startTime)
    {
        timerStartTime = startTime;
    }

    public void GetAchTimerEndTime(float endTime)
    {
        timerEndtTime = endTime;
    }

    //
    // End of - The Actual Achievement System
    // 
}

// Custom datatype containing each perk level information
class AchievementItemInfo
{
    public int goalValue { get; set; }
    public float xpReward { get; set; }
    public float gemReward { get; set; }

    public AchievementItemInfo(int goalValue, float xpReward, float gemReward)
    {
        this.goalValue = goalValue;
        this.xpReward = xpReward;
        this.gemReward = gemReward;
    }
}
