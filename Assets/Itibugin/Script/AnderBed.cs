using UnityEngine;

public class AnderBed : MonoBehaviour
{
    // Inspectorで設定：直接カメラコンポーネントをアタッチ
    public camera cameraFollowScript;

    public Vector3 newOffset = new Vector3(0f, 5f, -2f);
    private Vector3 originalOffset;

    private bool isPlayerInside = false; // プレイヤーがエリア内にいるかどうかのフラグ
    private Transform playerTransform;   // エリアに入ったプレイヤーを記録

    void Start()
    {
        if (cameraFollowScript == null)
        {
            Debug.LogError("CameraFollowScriptがInspectorで設定されていません。");
            enabled = false;
            return;
        }
        originalOffset = cameraFollowScript.offset;
    }

    void Update()
    {
        // ★追加：エリア内にいるはずのプレイヤーが消えた（死亡した）場合、カメラを戻す
        if (isPlayerInside && (playerTransform == null || !playerTransform.gameObject.activeInHierarchy))
        {
            ResetCameraOffset();
        }
    }

    // ★追加：カメラオフセットを安全に戻すための関数
    private void ResetCameraOffset()
    {
        if (cameraFollowScript != null)
        {
            cameraFollowScript.offset = originalOffset;
            Debug.Log("プレイヤーの消失を検知したため、カメラオフセットをリセットしました");
        }
        isPlayerInside = false;
        playerTransform = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && cameraFollowScript != null)
        {
            // エリアに入る前の値を保存し、フラグを立てる
            isPlayerInside = true;
            playerTransform = other.transform;
            originalOffset = cameraFollowScript.offset;

            cameraFollowScript.offset = newOffset;
            Debug.Log("カメラオフセットを切り替えました: " + newOffset);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && cameraFollowScript != null)
        {
            // 通常の退出時もリセット関数を呼ぶ
            ResetCameraOffset();
        }
    }
}
