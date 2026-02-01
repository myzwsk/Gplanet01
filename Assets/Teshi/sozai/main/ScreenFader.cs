using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeTime = 1f;

    public IEnumerator FadeOut()
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            SetAlpha(t / fadeTime);
            yield return null;
        }
        SetAlpha(1);
    }

    public IEnumerator FadeIn()
    {
        float t = fadeTime;
        while (t > 0)
        {
            t -= Time.deltaTime;
            SetAlpha(t / fadeTime);
            yield return null;
        }
        SetAlpha(0);
    }

    private void SetAlpha(float a)
    {
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }
}
