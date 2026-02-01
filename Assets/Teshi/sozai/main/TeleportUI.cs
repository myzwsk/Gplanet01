using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TeleportUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;      // TeleportPanel
    public Image fadeImage;       // FadeImage（白・Alpha0）

    [Header("Fade Setting")]
    public float fadeTime = 0.15f;

    private Transform player;
    private Transform destination;

    // WarpPoint から呼ばれる
    public void Open(Transform playerTransform, Transform dest)
    {
        player = playerTransform;
        destination = dest;

        panel.SetActive(true);

        // UI操作用にカーソルを出す
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 「はい」ボタン
    public void OnYes()
    {
        StartCoroutine(TeleportSequence());
    }

    // 「いいえ」ボタン
    public void OnNo()
    {
        panel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // フェード → テレポート → フェード戻し
    private IEnumerator TeleportSequence()
    {
        // カーソルを戻す
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // うっすらフェードイン
        yield return Fade(0f, 0.9f);

        // CharacterController 対策
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 位置と向きを揃える
        player.position = destination.position;
        player.rotation = destination.rotation;

        if (cc != null) cc.enabled = true;

        // フェードアウト
        yield return Fade(0.9f, 0f);

        panel.SetActive(false);
    }

    // フェード処理
    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / fadeTime);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}
