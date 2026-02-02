
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEngine.GraphicsBuffer;

public class TitleHardCom : MonoBehaviour
{
    // フェードアウト用Canvas Groupコンポーネントをインスペクタから設定
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1.0f;              // フェードにかける時間（秒）
    [SerializeField] private string nextSceneName = "NextLevelScene";// 遷移先のシーン名
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            if (Input.GetKey(KeyCode.O))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    StartCoroutine(FadeAndLoadScene());
                }
            }
        }
    }
    private IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;

        // フェードアウト処理
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Alpha値を 0 から 1 に徐々に変化させる (透明 -> 白)
            fadePanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null; // 1フレーム待つ
        }

        // 完全に白くなったらシーンをロード
        SceneManager.LoadScene(nextSceneName);
    }
}
