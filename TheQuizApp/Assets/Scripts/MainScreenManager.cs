using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPage;
    [SerializeField]
    private GameObject categoriesPage;

    public void OpenCloseCategories()
    {
        mainPage.SetActive(!mainPage.activeSelf);
        categoriesPage.SetActive(!categoriesPage.activeSelf);
    }
	
}
