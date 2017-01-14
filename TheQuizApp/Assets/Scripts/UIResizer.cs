using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIResizer : MonoBehaviour
{
    private Button[] buttons = new Button[4];
    private Text[] texts = new Text[4];

    private CurrSessionManager csm;
    private int textLineLength = 60;
    private int charsPerLine = 70;

    void Awake ()
    {
        csm = gameObject.GetComponent<CurrSessionManager>();

        buttons = csm.buttons;

        texts[0] = buttons[0].GetComponentInChildren<Text>();
        texts[1] = buttons[1].GetComponentInChildren<Text>();
        texts[2] = buttons[2].GetComponentInChildren<Text>();
        texts[3] = buttons[3].GetComponentInChildren<Text>();
    }

    public void ResizeUI()
    {
        ResizeTextUI();
        ResizeButtonUI();
    }

    public void ResizeTextUI()
    {
        for (int i = 0; i < 4; i++)
        {
            int heightMultiplier = (int) Mathf.Floor(texts[i].text.Length / charsPerLine);

            int totalHeight = Mathf.Max(textLineLength * heightMultiplier, 200);
            texts[i].GetComponent<RectTransform>().sizeDelta = new Vector2(850, totalHeight);
        }
    }

    public void ResizeButtonUI ()
    {
        for (int i = 0; i < 4; i++)
        {          
            buttons[i].GetComponent<RectTransform>().sizeDelta = new Vector2(buttons[i].GetComponent<RectTransform>().rect.width, buttons[i].transform.GetChild(0).GetComponent<RectTransform>().rect.height);
        }
        
    }
}
