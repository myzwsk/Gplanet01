
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEngine.GraphicsBuffer;

public class TitleKari : MonoBehaviour
{
    // フェードアウト用のCanvas Groupコンポーネントをインスペクタから設定
    [SerializeField] private CanvasGroup fadePanel;
    // フェードにかける時間（秒）
    [SerializeField] private float fadeDuration = 1.0f;
    // 遷移先のシーン名
    [SerializeField] private string nextSceneName = "NextLevelScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnAction(InputAction.CallbackContext context) // Moveアクション
    {
        StartCoroutine(FadeAndLoadScene());
    }
    // Update is called once per frame
    void Update()
    {
        
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
