using UnityEngine;
using System.Collections; // コルーチンを使うために必要
using TMPro;

public class Text１ : MonoBehaviour
{
    public TMP_Text collisionText;

    // テキストを表示する時間（秒）
    public float displayDuration = 2.0f;

    // 衝突を検出したときに一度だけ呼ばれる関数
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 衝突した相手が「Wall」タグを持っているか確認
        if (hit.gameObject.CompareTag("Stealth_Wall"))
        {
            // デバッグログを追加して衝突検出を確認
             Debug.Log("CharacterControllerがWallに衝突しました。"); 

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
            // TextMeshProコンポーネントの表示/非表示を切り替える
            collisionText.gameObject.SetActive(true);
        }

        // 2. 指定された時間だけ待機
        yield return new WaitForSeconds(displayDuration);

        // 3. テキストを非表示
        if (collisionText != null)
        {
            collisionText.gameObject.SetActive(false);
        }
    }
}
