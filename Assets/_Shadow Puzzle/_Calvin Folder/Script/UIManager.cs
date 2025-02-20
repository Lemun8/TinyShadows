using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private CanvasGroup _canvasGroup;
    // [SerializeField] private SpriteRenderer _spriteRenderer;
    private Tween fadeTween;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    void Update()
    {

    }
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickPlay()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            //_canvasGroup.interactable = true;
            //_canvasGroup.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        });
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = _canvasGroup.DOFade(endValue, duration);
        fadeTween.onComplete = onEnd;
        //fadeTween = _spriteRenderer.DOFade(endValue, duration);
        fadeTween.onComplete = onEnd;
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.2f);
        FadeOut(1f);


    }

    public IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1);
        FadeIn(1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

}
