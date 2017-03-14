using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainScreenManager : MonoBehaviour
{
    private StoreManager storeManager;
    private AchievementManager achievementManager;

    public Animator mainAnimator;
    public Button categoriesButton;
    public Button storeButton;
    public GameObject mainPage;
    public GameObject categoriesPage;
    public GameObject storePage;
    public GameObject achievementsPage;

    private void Awake()
    {
        storeManager = gameObject.GetComponent<StoreManager>();
        achievementManager = gameObject.GetComponent<AchievementManager>();

        categoriesButton.onClick.AddListener(OpenCategories);
        storeButton.onClick.AddListener(OpenStore);
    }

    public void PlayCloseAnimation()
    {
        mainAnimator.SetTrigger("ClosePage");
    }

    private void PlayOpenAnimation()
    {
        mainAnimator.SetTrigger("OpenPage");
    }

    IEnumerator WaitUntillClosed()
    {
        yield return new WaitForSeconds(0.8f);
    }

    IEnumerator WaitUntillOpened(GameObject objToClose)
    {
        yield return new WaitForSeconds(0.8f);
        objToClose.SetActive(false);
    }

    private void DoTheAnimations(GameObject pageObj)
    {
        if (pageObj.activeSelf == false)
        {
            pageObj.SetActive(true);
            PlayCloseAnimation();
        }
        else
        {
            PlayOpenAnimation();
            StartCoroutine(WaitUntillOpened(pageObj));
            
        }
    }

    public void OpenCategories()
    {
        DoTheAnimations(categoriesPage);
    }

    public void OpenStore()
    {
        DoTheAnimations(storePage);

        storeManager.OnStoreOpen();
    }

    public void OpenAchievements()
    {
        DoTheAnimations(achievementsPage);

        achievementManager.UpdateAllAchInfo();
    }

}
