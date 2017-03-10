using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionXPManager : MonoBehaviour {

    private SaveManager saveManager;
    private LoadManager loadManager;

    public static int[] levelXPs = new int[500];
    
    public static int currLevel = 0;
    public static int totalXP = 0;
    public static int currSessXP = 0;
    public static int remainingXP = 0;
    public static int xpPastLevel = 0;

    private int xpForWrong = 1;
    private int xpForCorrect = 5;

    private void Awake()
    {
        levelXPs[0] = 0;
        levelXPs[1] = 50;
        levelXPs[2] = 150;
        levelXPs[3] = 300;
        levelXPs[4] = 525;
        levelXPs[5] = 800;
        levelXPs[6] = 1150;
        levelXPs[7] = 1500;
        levelXPs[8] = 1925;
        levelXPs[9] = 2400;
        levelXPs[10] = 2950;
        levelXPs[11] = 3600;
        levelXPs[12] = 4300;
        levelXPs[13] = 5100;
        levelXPs[14] = 6000;
        levelXPs[15] = 7000;
        levelXPs[16] = 8100;
        levelXPs[17] = 9300;
        levelXPs[18] = 10600;
        levelXPs[19] = 12000;
        levelXPs[20] = 13500;

        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();
    }

    // Calculates current xp, whenever a question is answered wrongly
    public void AddXPOnWrong()
    {
        currSessXP += xpForWrong;
    }

    // Calculates current xp, whenever a question is answered correctly
    public void AddXPOnCorrect(int bonusXP)
    {
        currSessXP += xpForCorrect + bonusXP;
    }

    // Calculates remaining xp to the next level
    public int CalcRemainingXP()
    {
        remainingXP = levelXPs[currLevel + 1] - totalXP;
        
        return remainingXP;
    }

    // Calculates remaining xp to the next level
    public int CalcXPPastLevel()
    {
        xpPastLevel = totalXP - levelXPs[currLevel];

        return xpPastLevel;
    }

    // Loads the total XP and the current level
    public void LoadTotalXPNLevels()
    {
        totalXP = loadManager.LoadTotalXP();
        currLevel = loadManager.LoadCurrentLevel();
    }

    // Saves the total XP and the current level
    public void SaveTotalXPNLevels()
    {
        saveManager.SaveTotalXP(totalXP);
        saveManager.SaveCurrentLevel(currLevel);
    }

}
