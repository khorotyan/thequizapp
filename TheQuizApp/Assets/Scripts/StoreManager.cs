using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    private SaveManager saveManager;
    private LoadManager loadManager;

    public Button backButton;
    public Text totalMoneyText;
    public GameObject mainPanel;
    public GameObject storePagePanel;

    [Space(4)]

    public GameObject Store5050Obj;
    public GameObject StoreVaultObj;
    public GameObject StoreComboMultObj;
    public GameObject StoreMoreMoneyObj;
    public GameObject StoreBonusTimeObj;

    public static int totalMoney = 0;

    private void Awake()
    {
        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();

        backButton.onClick.AddListener(OnStorePageClose);
    }
    
    public void OnStoreOpen()
    {
        totalMoney = loadManager.LoadTotalMoney();
        UpdateTotalMoneyText();
        CheckAllPerkLevels();
    }

    public void ManageStore5050()
    {
        int price = 75;

        
    }

    public void ManageStoreVault()
    {
        StoreItemInfo[] storeVaultInfo = new StoreItemInfo[10];

        storeVaultInfo[0] = new StoreItemInfo(100, 20, 0);
        storeVaultInfo[1] = new StoreItemInfo(250, 30, 0);
        storeVaultInfo[2] = new StoreItemInfo(475, 40, 0);
        storeVaultInfo[3] = new StoreItemInfo(800, 45, 0);
        storeVaultInfo[4] = new StoreItemInfo(1250, 50, 0);
    }

    public void ManageStoreComboMult()
    {
        StoreItemInfo[] storeComboMultInfo = new StoreItemInfo[10];

        storeComboMultInfo[0] = new StoreItemInfo(0, 0.2f, 0);
        storeComboMultInfo[1] = new StoreItemInfo(225, 0.35f, 0);
        storeComboMultInfo[2] = new StoreItemInfo(525, 0.45f, 0);
        storeComboMultInfo[3] = new StoreItemInfo(900, 0.5f, 0);
    }

    public void ManageStoreMoreMoney()
    {
        StoreItemInfo[] storeMoreMoneyInfo = new StoreItemInfo[10];

        storeMoreMoneyInfo[0] = new StoreItemInfo(650, 1.5f, 0);
        storeMoreMoneyInfo[1] = new StoreItemInfo(1050, 1.75f, 0);
        storeMoreMoneyInfo[2] = new StoreItemInfo(1500, 2f, 0);
    }

    public void ManageStoreBonusTime()
    {
        StoreItemInfo[] storeBonusTimeInfo = new StoreItemInfo[10];

        storeBonusTimeInfo[0] = new StoreItemInfo(875, 0.3f, 0.1f);
        storeBonusTimeInfo[1] = new StoreItemInfo(1450, 0.35f, 0.15f);
        storeBonusTimeInfo[2] = new StoreItemInfo(2100, 0.4f, 0.2f);
    }

    private void CheckAllPerkLevels()
    {
        // 50/50 Perk
        int reqlvl5050 = 1;
        CheckPerkLevel(reqlvl5050, Store5050Obj);

        // Vault Perk
        int reqlvlVault = 3;
        CheckPerkLevel(reqlvlVault, StoreVaultObj);

        // ComboMult Perk
        int reqlvlComboMult = 6;
        CheckPerkLevel(reqlvlComboMult, StoreComboMultObj);

        // MoreMoney Perk
        int reqlvlMoreMoney = 9;
        CheckPerkLevel(reqlvlMoreMoney, StoreMoreMoneyObj);

        // BonusTime Perk
        int reqlvlBonusTime = 12;
        CheckPerkLevel(reqlvlBonusTime, StoreBonusTimeObj);
    }

    // If the perk is open, disable the perk hider
    private void CheckPerkLevel(int requiredLevel, GameObject perkHider)
    {
        int currLevel = loadManager.LoadCurrentLevel();

        if (currLevel >= requiredLevel)
        {
            perkHider.transform.GetChild(1).gameObject.SetActive(false);
        }

    }

    // Update the money that the user has, after the end of the game
    public void UpdateTMoneyOnResults(int sessionMoney)
    {
        totalMoney = loadManager.LoadTotalMoney();
        totalMoney += sessionMoney;
        saveManager.SaveTotalMoney(totalMoney);
    }

    private void UpdateTotalMoneyText()
    {
        totalMoneyText.text = "Money: " + totalMoney.ToString();
    }

    public void OnStorePageClose()
    {
        storePagePanel.SetActive(false);
        mainPanel.SetActive(true);

        saveManager.SaveTotalMoney(totalMoney);
    }
}

class StoreItemInfo
{
    public int price { get; set; }
    public float value { get; set; }
    public float value2 { get; set; }

    public StoreItemInfo(int price, float value, float value2)
    {
        this.price = price;
        this.value = value;
        this.value2 = value2;
    }
}
