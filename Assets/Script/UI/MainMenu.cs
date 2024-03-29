using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Transform gameLogo;
    [SerializeField]
    private Transform tutorPanel;
    [SerializeField]
    private Transform guideLine;

    [SerializeField]
    private Transform playerUI;

    private void Start()
    {
        tutorPanel.gameObject.SetActive(false);

        gameLogo.GetComponent<CanvasGroup>().alpha = 0f;
        gameLogo.GetComponent<CanvasGroup>().DOFade(1, 2f).SetUpdate(true);
        gameLogo.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1200, 100, 0);
        gameLogo.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 100), 1f, false).SetEase(Ease.InOutBack);
        
        StartUpUI();
    }

    public void CloseScene()
    {
        playerUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(600, -250), 1f, false).SetEase(Ease.OutQuint).SetUpdate(true);
        playerUI.GetComponent<CanvasGroup>().DOFade(0, 1f).SetEase(Ease.OutQuint).SetUpdate(true);  
    }

    private void StartUpUI()
    {
        playerUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(-700, 200, 0);
        playerUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(150, 0), 1.5f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    public void ShowTutorPanel()
    {
        tutorPanel.gameObject.SetActive(true);
        guideLine.gameObject.SetActive(true);
        FadeIn(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>());

    }

    public void HideTutorPanel()
    {
        StartCoroutine(FadeOut(tutorPanel.GetComponent<CanvasGroup>(), guideLine.GetComponent<RectTransform>()));

    }   

    private void FadeIn(CanvasGroup canvasGroup ,RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 700, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 700), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);

        yield return new WaitForSecondsRealtime(.3f);
        guideLine.gameObject.SetActive(true);
        tutorPanel.gameObject.SetActive(false);

    }

}
