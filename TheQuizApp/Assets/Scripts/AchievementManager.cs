using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public GameObject mainMenuPage;
    public GameObject achievementsPage;

    public Transform achObjContainer;
    private Transform[] achievementObjs = new Transform[20];

    private AchievementItemInfo[] achMoneyMakerInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achSwiftSwiftInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achLearnerInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achProgressInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achToughInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achTougherInfo = new AchievementItemInfo[3];
    private AchievementItemInfo[] achMasterQuizerInfo = new AchievementItemInfo[3];

    private void Awake()
    {
        AllAchievementInfo();
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
