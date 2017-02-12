using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    private SaveManager saveManager;
    private LoadManager loadManager;
    private StoreManager storeManager;

    public Button categoriesButton;
    public Button storeButton;
    public GameObject mainPage;
    public GameObject categoriesPage;
    public GameObject storePage;

    private void Awake()
    {
        saveManager = gameObject.GetComponent<SaveManager>();
        loadManager = gameObject.GetComponent<LoadManager>();
        storeManager = gameObject.GetComponent<StoreManager>();

        categoriesButton.onClick.AddListener(OpenCategories);
        storeButton.onClick.AddListener(OpenStore);
    }

    public void OpenCategories()
    {
        mainPage.SetActive(!mainPage.activeSelf);
        categoriesPage.SetActive(!categoriesPage.activeSelf);
    }

    public void OpenStore()
    {
        mainPage.SetActive(!mainPage.activeSelf);
        storePage.SetActive(!storePage.activeSelf);

        storeManager.OnStoreOpen();
    }
	
}
