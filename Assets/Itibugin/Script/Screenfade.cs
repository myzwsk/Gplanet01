using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Screenfade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeTime = 1.5f;
    public string nextSceneName;
    bool isFading = false;
    public void FadeOutAndLoad()
    {
        StartCoroutine(FadeOut());
        if (isFading) return;
        isFading = true;
        Debug.Log("FadeOutAndLoad 呼ばれた");
    }

    IEnumerator FadeOut()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeTime);
            fadeImage.color = c;
            yield return null;
        }
        Debug.Log("Scene Load 実行");
        SceneManager.LoadScene(nextSceneName);
    }
}
