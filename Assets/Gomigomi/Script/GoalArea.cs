using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerに必要
using System.Collections; // コルーチンに必要
public class GoalArea : MonoBehaviour
{
    // フェードアウト用のCanvas Groupコンポーネントをインスペクタから設定
    [SerializeField] private CanvasGroup fadePanel;// フェード時間（秒）
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private string nextSceneName = "NextLevelScene";
   
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player")|| other.CompareTag("player"))
        {
            // フェードアウト処理
            StartCoroutine(FadeAndLoadScene());
        }
    }

    // フェードアウトとシーン遷移を順番に行うコルーチン
    public IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;

        // フェードアウト処理
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Alpha値を 0 から 1 に徐々に変化
            fadePanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null; // 1フレーム待つ
        }

        // 完全に白くなったらシーンをロード
        SceneManager.LoadScene(nextSceneName);
    }
}
