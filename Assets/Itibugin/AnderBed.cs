using UnityEngine;

public class AnderBed : MonoBehaviour
{
    //  Inspectorで設定：直接カメラコンポーネントをアタッチ
    public camera cameraFollowScript; // FindObjectOfTypeの代わりにこれを使う！

    public Vector3 newOffset = new Vector3(0f, 5f, -2f);
    private Vector3 originalOffset;

    void Start()
    {
        // 参照が設定されているか確認するだけで済む（検索は不要）
        if (cameraFollowScript == null)
        {
            Debug.LogError("CameraFollowScriptがInspectorで設定されていません。");
            enabled = false;
            return;
        }

        // 元のオフセットを保存する処理
        originalOffset = cameraFollowScript.offset;
    }

    // ... (OnTriggerEnter, OnTriggerExit のロジックは変更なし)

    // プレイヤーが判定エリアに入ったとき
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーのタグ("Player")を持つオブジェクトか確認
        if (other.CompareTag("player") && cameraFollowScript != null)
        {
            // 1. エリアに入る前の現在のオフセットを保存する
            originalOffset = cameraFollowScript.offset;

            // 2. カメラのオフセットを新しい値に変更する
            cameraFollowScript.offset = newOffset;

            Debug.Log("カメラオフセットを新しい値に切り替えました: " + newOffset);
        }
    }

    // プレイヤーが判定エリアから出たとき
    private void OnTriggerExit(Collider other)
    {
        // プレイヤーのタグ("Player")を持つオブジェクトか確認
        if (other.CompareTag("player") && cameraFollowScript != null)
        {
            // 3. 保存しておいた元のオフセットに戻す
            cameraFollowScript.offset = originalOffset;

            Debug.Log("カメラオフセットを元の値に戻しました: " + originalOffset);
        }
    }
}
