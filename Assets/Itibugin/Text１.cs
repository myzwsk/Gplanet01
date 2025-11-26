using UnityEngine;
using System.Collections; // コルーチンを使うために必要

// TextMeshProを使う場合、この行を追加
// using TMPro;
using UnityEngine.UI; // 標準のTextを使う場合、この行を追加

public class Text１ : MonoBehaviour
{
    // Inspectorから設定できるようにTextコンポーネントを公開
    // TextMeshProを使う場合は「public TMP_Text collisionText;」
    public Text collisionText;

    // テキストを表示する時間（秒）
    public float displayDuration = 2.0f;

    // 衝突を検出したときに一度だけ呼ばれる関数
    void OnCollisionEnter(Collision collision)
    {
        // 衝突した相手が「Wall」タグを持っているか確認
        if (collision.gameObject.CompareTag("Wall"))
        {
            // すでにコルーチンが実行中でないか確認してから開始
            StopAllCoroutines();
            StartCoroutine(DisplayAndHideText());
        }
    }

    // コルーチン（一定時間待機する処理などに使う）
    IEnumerator DisplayAndHideText()
    {
        // 1. テキストを表示
        if (collisionText != null)
        {
            // Textコンポーネントの表示/非表示を切り替える場合
            collisionText.gameObject.SetActive(true);
            // もしTextコンポーネントのenabledを切り替えるなら以下
            // collisionText.enabled = true; 
        }

        // 2. 指定された時間だけ待機
        yield return new WaitForSeconds(displayDuration);

        // 3. テキストを非表示
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(false);
            // collisionText.enabled = false; 
        }
    }
}
