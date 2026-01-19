using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Image bgImage;
    public float fadeTime = 1.0f;

    public void ChangeBackground(Sprite nextSprite)
    {
        StartCoroutine(FadeRoutine(nextSprite));
    }

    IEnumerator FadeRoutine(Sprite next)
    {
        // フェードアウト
        for (float t = 1; t > 0; t -= Time.deltaTime / fadeTime)
        {
            bgImage.color = new Color(1, 1, 1, t);
            yield return null;
        }

        bgImage.sprite = next;

        // フェードイン
        for (float t = 0; t < 1; t += Time.deltaTime / fadeTime)
        {
            bgImage.color = new Color(1, 1, 1, t);
            yield return null;
        }
    }
}
