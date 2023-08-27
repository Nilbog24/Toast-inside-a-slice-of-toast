using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleHeader : MonoBehaviour
{
    public Image banner;
    public TextMeshProUGUI titleText;
    public string title {get{return titleText.text;} set{titleText.text = value;}}

    public enum DISPLAYMETHOD
    {
        instant,
        slowFade,
        typeWriter
    }

    public DISPLAYMETHOD displayMethod = DISPLAYMETHOD.instant;
    public float fadeSpeed = 1f;
    
    public void Show(string displayTitle)
    {
        title = displayTitle;
        if(isRevealing)
            StopCoroutine(revealing);

        revealing = StartCoroutine(Revealing());
    }

    public void Hide()
    {
        if(isRevealing)
            StopCoroutine(revealing);
        revealing = null;

        banner.enabled = false;
        titleText.enabled = false;
    }

    public bool isRevealing {get{return revealing != null;}}
    Coroutine revealing = null;
    IEnumerator Revealing()
    {
        banner.enabled = true;
        titleText.enabled = true;

        switch(displayMethod)
        {
            case DISPLAYMETHOD.instant:
                banner.color = GlobalF.SetAlpha(banner.color, 1);
                titleText.color = GlobalF.SetAlpha(titleText.color, 1);
                break;
            case DISPLAYMETHOD.slowFade:
                banner.color = GlobalF.SetAlpha(banner.color, 0);
                titleText.color = GlobalF.SetAlpha(titleText.color, 0);
                while(banner.color.a < 1)
                {
                    banner.color = GlobalF.SetAlpha(banner.color, Mathf.MoveTowards(banner.color.a, 1, fadeSpeed * Time.unscaledDeltaTime));
                    titleText.color = GlobalF.SetAlpha(titleText.color, banner.color.a);
                    yield return new WaitForEndOfFrame();
                }
                break;
            case DISPLAYMETHOD.typeWriter:
                banner.color = GlobalF.SetAlpha(banner.color, 1);
                titleText.color = GlobalF.SetAlpha(titleText.color, 1);
                TextArchitect architect = new TextArchitect(titleText, title);
                while(architect.isConstructing)
                    yield return new WaitForEndOfFrame();
                break;
        }

        revealing = null;
    }
}

