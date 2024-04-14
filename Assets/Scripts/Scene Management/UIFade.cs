using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeSpeed = 1f;

    private Coroutine fadeCoroutine;

    private void Start()
    {
        // Ensure that the fade screen image is assigned
        if (fadeScreen == null)
        {
            Debug.LogError("FadeScreen Image is not assigned in the UIFade script.");
            enabled = false; // Disable the script to prevent errors
        }
    }

    public void FadeToBlack()
    {
        StartFadeRoutine(1);
    }

    public void FadeToClear()
    {
        StartFadeRoutine(0);
    }

    private void StartFadeRoutine(float targetAlpha)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        Color startColor = fadeScreen.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (!Mathf.Approximately(fadeScreen.color.a, targetAlpha))
        {
            fadeScreen.color = Color.Lerp(fadeScreen.color, targetColor, fadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
