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
        typeWriter,
        floatingSlowFade
    }

    public DISPLAYMETHOD displayMethod = DISPLAYMETHOD.instant;
    public float fadeSpeed = 1f;
    
    public void Show(string displayTitle)
    {
        title = displayTitle;
        if(isRevealing)
            StopCoroutine(revealing);


        if(chachedBannerPos)
            chachedBannerOriginalPosition = banner.transform.position;
            chachedBannerPos = true;
        revealing = StartCoroutine(Revealing());

        
    }

    public void Hide()
    {
        if(isRevealing)
            StopCoroutine(revealing);
        revealing = null;

        banner.enabled = false;
        titleText.enabled = false;

        if(chachedBannerPos)
            banner.transform.position = chachedBannerOriginalPosition;
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
                yield return SlowFade();
                break;
            case DISPLAYMETHOD.floatingSlowFade:
                yield return FloatingSlowFade();
                break;
            case DISPLAYMETHOD.typeWriter:
                yield return TypeWriter();
                break;

        }

        revealing = null;
    }

    IEnumerator SlowFade()
    {
        banner.color = GlobalF.SetAlpha(banner.color, 0);
        titleText.color = GlobalF.SetAlpha(titleText.color, 0);
        while(banner.color.a < 1)
        {
            banner.color = GlobalF.SetAlpha(banner.color, Mathf.MoveTowards(banner.color.a, 1, fadeSpeed * Time.unscaledDeltaTime));
            titleText.color = GlobalF.SetAlpha(titleText.color, banner.color.a);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator TypeWriter()
    {
        banner.color = GlobalF.SetAlpha(banner.color, 1);
        titleText.color = GlobalF.SetAlpha(titleText.color, 1);
        TextArchitect architect = new TextArchitect(titleText, title);
        while(architect.isConstructing)
            yield return new WaitForEndOfFrame();
    }

    bool chachedBannerPos = false;
    Vector3 chachedBannerOriginalPosition = Vector3.zero;
    IEnumerator FloatingSlowFade()
    {
        banner.color = GlobalF.SetAlpha(banner.color, 0);
        titleText.color = GlobalF.SetAlpha(titleText.color, 0);

        float amount = 25f * ((float)Screen.height / 720f);
        Vector3 downPos = new Vector3(0, amount, 0);
        banner.transform.position = chachedBannerOriginalPosition - downPos;

        while(banner.color.a < 1 || banner.transform.position != chachedBannerOriginalPosition)
        {
            banner.color = GlobalF.SetAlpha(banner.color, Mathf.MoveTowards(banner.color.a, 1, fadeSpeed * Time.unscaledDeltaTime));
            titleText.color = GlobalF.SetAlpha(titleText.color, banner.color.a);

            banner.transform.position = Vector3.MoveTowards(banner.transform.position, chachedBannerOriginalPosition, 11*fadeSpeed * Time.unscaledDeltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}

