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
    public Text totalGemsText;
    public GameObject mainPanel;
    public GameObject storePagePanel;

    [Space(4)]

    public Button perk5050Button;
    public Button perkBonusTimeButton;

    [Space(4)]

    public GameObject store5050Obj;
    public GameObject storeVaultObj;
    public GameObject storeComboMultObj;
    public GameObject storeMoreMoneyObj;
    public GameObject storeBonusTimeObj;

    public int store5050Lvl = 0;
    int storeVaultLvl = 0;
    int storeComboMultLvl = 0;
    int storeMoreMoneyLvl = 0;
    int storeBonusTimeLvl = 0;

    int store5050Price;
    private StoreItemInfo[] storeVaultInfo = new StoreItemInfo[6]; // ID = 1
    private StoreItemInfo[] storeComboMultInfo = new StoreItemInfo[4]; // ID = 2
    private StoreItemInfo[] storeMoreMoneyInfo = new StoreItemInfo[4]; // ID = 3
    private StoreItemInfo[] storeBonusTimeInfo = new StoreItemInfo[4]; // ID = 4

    public static int totalMoney = 0;
    public static int totalGems = 0;

    private void Awake()
    {
        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();

        backButton.onClick.AddListener(OnStorePageClose);

        AllPerkInfo();
        LoadPerkData();
    }

    public void Cheat()
    {
        totalMoney += 500;
        totalGems += 15;

        UpdateTotalMoneyText();
        UpdateTotalGemsText();
        CheckAllPerkLevels();
    }

    // Creates the information of the levels of perks (prices and values)
    private void AllPerkInfo()
    {
        store5050Price = 75;

        storeVaultInfo[0] = new StoreItemInfo(0, 0, 0);
        storeVaultInfo[1] = new StoreItemInfo(100, 20, 0);
        storeVaultInfo[2] = new StoreItemInfo(250, 30, 0);
        storeVaultInfo[3] = new StoreItemInfo(475, 40, 0);
        storeVaultInfo[4] = new StoreItemInfo(800, 45, 0);
        storeVaultInfo[5] = new StoreItemInfo(1250, 50, 0);

        storeComboMultInfo[0] = new StoreItemInfo(0, 0.2f, 0);
        storeComboMultInfo[1] = new StoreItemInfo(225, 0.35f, 0);
        storeComboMultInfo[2] = new StoreItemInfo(525, 0.45f, 0);
        storeComboMultInfo[3] = new StoreItemInfo(900, 0.5f, 0);

        storeMoreMoneyInfo[0] = new StoreItemInfo(0, 1f, 0);
        storeMoreMoneyInfo[1] = new StoreItemInfo(650, 1.5f, 0);
        storeMoreMoneyInfo[2] = new StoreItemInfo(1050, 1.75f, 0);
        storeMoreMoneyInfo[3] = new StoreItemInfo(1500, 2f, 0);

        storeBonusTimeInfo[0] = new StoreItemInfo(0, 0, 0);
        storeBonusTimeInfo[1] = new StoreItemInfo(875, 30f, 10f);
        storeBonusTimeInfo[2] = new StoreItemInfo(1450, 35f, 15f);
        storeBonusTimeInfo[3] = new StoreItemInfo(2100, 40f, 20f);
    }

    // Load the real perk data, which is used during the game (not the visual)
    private void LoadPerkData()
    {
        store5050Lvl = loadManager.LoadPerkLevel(0);
        if (store5050Lvl == 0)
        {
            perk5050Button.interactable = false;
            Color32 imgCol = perk5050Button.image.color;
            perk5050Button.image.color = new Color32(imgCol.r, imgCol.g, imgCol.b, 20);
        }
        else
        {
            perk5050Button.interactable = true;
            Color32 imgCol = perk5050Button.image.color;
            perk5050Button.image.color = new Color32(imgCol.r, imgCol.g, imgCol.b, 100);
        }

        storeVaultLvl = loadManager.LoadPerkLevel(1);
        SessionCurrencyManager.protectedAmount = storeVaultInfo[storeVaultLvl].value / 100;
        
        storeComboMultLvl = loadManager.LoadPerkLevel(2);
        SessionCurrencyManager.comboMultiplier = storeComboMultInfo[storeComboMultLvl].value;

        storeMoreMoneyLvl = loadManager.LoadPerkLevel(3);
        SessionCurrencyManager.moneyMultiplier = storeMoreMoneyInfo[storeMoreMoneyLvl].value;

        storeBonusTimeLvl = loadManager.LoadPerkLevel(4);
        if (storeBonusTimeLvl == 0)
        {
            perkBonusTimeButton.interactable = false;
            Color32 imgCol = perkBonusTimeButton.image.color;
            perkBonusTimeButton.image.color = new Color32(imgCol.r, imgCol.g, imgCol.b, 20);
        }
        else
        {
            perkBonusTimeButton.interactable = true;
            Color32 imgCol = perkBonusTimeButton.image.color;
            perkBonusTimeButton.image.color = new Color32(imgCol.r, imgCol.g, imgCol.b, 100);
        }
    }
    
    // Runs whenever the store opens
    public void OnStoreOpen()
    {
        totalMoney = loadManager.LoadTotalMoney();
        totalGems = loadManager.LoadTotalGems();
        UpdateTotalMoneyText();
        CheckAllPerkLevels();

        // Load the perk levels
        store5050Lvl = loadManager.LoadPerkLevel(0);
        storeVaultLvl = loadManager.LoadPerkLevel(1);
        storeComboMultLvl = loadManager.LoadPerkLevel(2);
        storeMoreMoneyLvl = loadManager.LoadPerkLevel(3);
        storeBonusTimeLvl = loadManager.LoadPerkLevel(4);

        UpdateSingleItemVisuals(store5050Obj.transform.GetChild(0), store5050Lvl);
        UpdateStoreItemVisuals(storeVaultObj.transform.GetChild(0), storeVaultInfo, storeVaultLvl, false);
        UpdateStoreItemVisuals(storeComboMultObj.transform.GetChild(0), storeComboMultInfo, storeComboMultLvl, false);
        UpdateStoreItemVisuals(storeMoreMoneyObj.transform.GetChild(0), storeMoreMoneyInfo, storeMoreMoneyLvl, false);
        UpdateStoreItemVisuals(storeBonusTimeObj.transform.GetChild(0), storeBonusTimeInfo, storeBonusTimeLvl, true);
    }

    // Update the the texts and the level indicators of the perk
    private void UpdateStoreItemVisuals(Transform perkObj, StoreItemInfo[] perkInfo, int perkLvl, bool hasOtherValue)
    {
        if (perkLvl < perkInfo.Length - 1)
        {
            string perkCost = "Cost: " + perkInfo[perkLvl + 1].price.ToString() + " $";
            perkObj.transform.GetChild(3).GetComponent<Text>().text = perkCost;

            string perkAmount = "x: " + perkInfo[perkLvl].value + " -> " + perkInfo[perkLvl + 1].value;

            if (hasOtherValue == true)
            {
                string perkAmount2 = "y: " + perkInfo[perkLvl].value2 + " -> " + perkInfo[perkLvl + 1].value2;
                perkAmount += "\n" + perkAmount2;
            }

            perkObj.transform.GetChild(4).GetComponent<Text>().text = perkAmount;
        }
        else
        {
            perkObj.transform.GetChild(3).gameObject.SetActive(false);
            perkObj.transform.GetChild(4).GetComponent<Text>().text = "Maxed Out";
            perkObj.gameObject.GetComponent<Button>().interactable = false;
        }

        for (int i = 0; i < perkLvl; i++)
        {
            perkObj.transform.GetChild(5).GetChild(0).GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 200);
        }
    }

    private void UpdateSingleItemVisuals(Transform perkObj, int perkLvl)
    {
        if (perkLvl == 1)
        {
            perkObj.transform.GetChild(3).gameObject.SetActive(false);
            perkObj.transform.GetChild(4).GetComponent<Text>().text = "Maxed Out";
            perkObj.transform.GetChild(5).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 200);
            perkObj.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    // Runs whenever a perk is clicked
    public void OnStoreItemClick(int itemID)
    {
        if (itemID == 0) { CheckPurchase(store5050Obj.transform.GetChild(2).gameObject); }
        else if (itemID == 1) { CheckPurchase(storeVaultObj.transform.GetChild(2).gameObject); }
        else if (itemID == 2) { CheckPurchase(storeComboMultObj.transform.GetChild(2).gameObject); }
        else if (itemID == 3) { CheckPurchase(storeMoreMoneyObj.transform.GetChild(2).gameObject); }
        else if (itemID == 4) { CheckPurchase(storeBonusTimeObj.transform.GetChild(2).gameObject); }
    }

    private void CheckPurchase(GameObject ResponceCheckerObj)
    {
        ClosePurchaseChecker();
        ResponceCheckerObj.SetActive(true);
    }

    public void ClosePurchaseChecker()
    {
        if (store5050Obj.transform.GetChild(2).gameObject.activeSelf == true) { store5050Obj.transform.GetChild(2).gameObject.SetActive(false); }
        else if (storeVaultObj.transform.GetChild(2).gameObject.activeSelf == true) { storeVaultObj.transform.GetChild(2).gameObject.SetActive(false); }
        else if (storeComboMultObj.transform.GetChild(2).gameObject.activeSelf == true) { storeComboMultObj.transform.GetChild(2).gameObject.SetActive(false); }
        else if (storeMoreMoneyObj.transform.GetChild(2).gameObject.activeSelf == true) { storeMoreMoneyObj.transform.GetChild(2).gameObject.SetActive(false); }
        else if (storeBonusTimeObj.transform.GetChild(2).gameObject.activeSelf == true) { storeBonusTimeObj.transform.GetChild(2).gameObject.SetActive(false); }
    }

    public void ManageStore5050()
    {
        ClosePurchaseChecker();

        if (totalMoney >= store5050Price)
        {
            totalMoney -= store5050Price;   

            store5050Lvl++;
            saveManager.SavePerkLevel(store5050Lvl, 0);
            LoadPerkData();
        }
        else
        {  
            store5050Obj.transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(DisableNotEnMoney());
        }

        UpdateTotalMoneyText();

        UpdateSingleItemVisuals(store5050Obj.transform.GetChild(0), store5050Lvl);

    }

    public void ManageStoreVault()
    {
        ClosePurchaseChecker();

        if (totalMoney >= storeVaultInfo[storeVaultLvl + 1].price)
        {
            totalMoney -= storeVaultInfo[storeVaultLvl + 1].price;

            storeVaultLvl++;
            saveManager.SavePerkLevel(storeVaultLvl, 1);
            LoadPerkData();
        }
        else
        { 
            storeVaultObj.transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(DisableNotEnMoney());
        }

        UpdateTotalMoneyText();

        UpdateStoreItemVisuals(storeVaultObj.transform.GetChild(0), storeVaultInfo, storeVaultLvl, false);
        
    }

    public void ManageStoreComboMult()
    {
        ClosePurchaseChecker();

        if (totalMoney >= storeComboMultInfo[storeComboMultLvl + 1].price)
        {
            totalMoney -= storeComboMultInfo[storeComboMultLvl + 1].price;

            storeComboMultLvl++;
            saveManager.SavePerkLevel(storeComboMultLvl, 2);
            LoadPerkData();
        }
        else
        {       
            storeComboMultObj.transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(DisableNotEnMoney());
        }

        UpdateTotalMoneyText();

        UpdateStoreItemVisuals(storeComboMultObj.transform.GetChild(0), storeComboMultInfo, storeComboMultLvl, false);
        
    }

    public void ManageStoreMoreMoney()
    {
        ClosePurchaseChecker();

        if (totalMoney >= storeMoreMoneyInfo[storeMoreMoneyLvl + 1].price)
        {
            totalMoney -= storeMoreMoneyInfo[storeMoreMoneyLvl + 1].price;

            storeMoreMoneyLvl++;
            saveManager.SavePerkLevel(storeMoreMoneyLvl, 3);
            LoadPerkData();
        }
        else
        {
            storeMoreMoneyObj.transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(DisableNotEnMoney());
        }

        UpdateTotalMoneyText();

        UpdateStoreItemVisuals(storeMoreMoneyObj.transform.GetChild(0), storeMoreMoneyInfo, storeMoreMoneyLvl, false);
        
    }

    public void ManageStoreBonusTime()
    {
        ClosePurchaseChecker();

        if (totalMoney >= storeBonusTimeInfo[storeBonusTimeLvl + 1].price)
        {
            totalMoney -= storeBonusTimeInfo[storeBonusTimeLvl + 1].price;

            storeBonusTimeLvl++;
            saveManager.SavePerkLevel(storeBonusTimeLvl, 4);
            LoadPerkData();
        }
        else
        {
            storeBonusTimeObj.transform.GetChild(3).gameObject.SetActive(true);
            StartCoroutine(DisableNotEnMoney());
        }

        UpdateTotalMoneyText();

        UpdateStoreItemVisuals(storeBonusTimeObj.transform.GetChild(0), storeBonusTimeInfo, storeBonusTimeLvl, true);

    }

    // Checks all the perk levels
    private void CheckAllPerkLevels()
    {
        // 50/50 Perk
        int reqlvl5050 = 1;
        CheckPerkLevel(reqlvl5050, store5050Obj);

        // Vault Perk
        int reqlvlVault = 3;
        CheckPerkLevel(reqlvlVault, storeVaultObj);

        // ComboMult Perk
        int reqlvlComboMult = 6;
        CheckPerkLevel(reqlvlComboMult, storeComboMultObj);

        // MoreMoney Perk
        int reqlvlMoreMoney = 9;
        CheckPerkLevel(reqlvlMoreMoney, storeMoreMoneyObj);

        // BonusTime Perk
        int reqlvlBonusTime = 12;
        CheckPerkLevel(reqlvlBonusTime, storeBonusTimeObj);
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
        totalGems = loadManager.LoadTotalGems();
        totalMoney += sessionMoney;
        saveManager.SaveTotalMoney(totalMoney);
        saveManager.SaveTotalGems(totalGems);
    }

    // Updates total money text
    private void UpdateTotalMoneyText()
    {
        totalMoneyText.text = totalMoney.ToString();
    }

    // Updates total gems text
    private void UpdateTotalGemsText()
    {
        totalGemsText.text = totalGems.ToString();
    }

    // Gets the answer percent (value1) of the Bonus Time perk
    public float GetBTAnswerPerc()
    {
        return storeBonusTimeInfo[storeBonusTimeLvl].value / 100;
    }

    // Gets the answer percent (value1) of the Bonus Time perk
    public float GetBTBonusPerc()
    {
        return storeBonusTimeInfo[storeBonusTimeLvl].value2 / 100;
    }

    // Runs whenever the store page closes
    public void OnStorePageClose()
    {
        saveManager.SaveTotalMoney(totalMoney);
        saveManager.SaveTotalGems(totalGems);

        // Save perk levels
        saveManager.SavePerkLevel(store5050Lvl, 0);
        saveManager.SavePerkLevel(storeVaultLvl, 1);
        saveManager.SavePerkLevel(storeComboMultLvl, 2);
        saveManager.SavePerkLevel(storeMoreMoneyLvl, 3);
        saveManager.SavePerkLevel(storeBonusTimeLvl, 4);

        ClosePurchaseChecker();
    }

    IEnumerator DisableNotEnMoney()
    {
        yield return new WaitForSeconds(1);

        if (store5050Obj.transform.GetChild(3).gameObject.activeSelf == true) { store5050Obj.transform.GetChild(3).gameObject.SetActive(false); }
        else if (storeVaultObj.transform.GetChild(3).gameObject.activeSelf == true) { storeVaultObj.transform.GetChild(3).gameObject.SetActive(false); }
        else if (storeComboMultObj.transform.GetChild(3).gameObject.activeSelf == true) { storeComboMultObj.transform.GetChild(3).gameObject.SetActive(false); }
        else if (storeMoreMoneyObj.transform.GetChild(3).gameObject.activeSelf == true) { storeMoreMoneyObj.transform.GetChild(3).gameObject.SetActive(false); }
        else if (storeBonusTimeObj.transform.GetChild(3).gameObject.activeSelf == true) { storeBonusTimeObj.transform.GetChild(3).gameObject.SetActive(false); }
    }
}

// Custom datatype containing each perk level information
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
