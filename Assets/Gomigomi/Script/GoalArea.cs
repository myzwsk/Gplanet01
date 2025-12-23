using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerに必要
using System.Collections; // コルーチンに必要
public class GoalArea : MonoBehaviour
{
    // フェードアウト用のCanvas Groupコンポーネントをインスペクタから設定
    [SerializeField] private CanvasGroup fadePanel;
    // フェードにかける時間（秒）
    [SerializeField] private float fadeDuration = 1.0f;
    // 遷移先のシーン名
    [SerializeField] private string nextSceneName = "NextLevelScene";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // プレイヤーがトリガーに入った時に呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        // 接触したオブジェクトがプレイヤーかどうかをタグで判定
        // (プレイヤーオブジェクトに "Player" タグが設定されている前提)
        if (other.CompareTag("Player")|| other.CompareTag("player"))
        {
            // プレイヤーがゴールしたので、フェードアウト処理を開始
            StartCoroutine(FadeAndLoadScene());
        }
    }

    // フェードアウトとシーン遷移を順番に行うコルーチン
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
