using UnityEngine;

public class AnderBed : MonoBehaviour
{
    public camera cameraFollowScript; // 独自のCameraクラスを想定

    public Vector3 newOffset = new Vector3(0f, 5f, -2f);
    public float smoothSpeed = 5f; // ★追加：切り替えの速さ（大きいほど速い）

    private Vector3 originalOffset;
    private Vector3 targetOffset; // ★追加：現在目指しているオフセット
    private bool isPlayerInside = false;
    private Transform playerTransform;

    void Start()
    {
        if (cameraFollowScript == null)
        {
            Debug.LogError("CameraFollowScriptが設定されていません。");
            enabled = false;
            return;
        }

        // 初期状態のオフセットを保存し、それをターゲットにする
        originalOffset = cameraFollowScript.offset;
        targetOffset = originalOffset;
    }

    void Update()
    {
        // 1. プレイヤーが消えた場合のチェック
        if (isPlayerInside && (playerTransform == null || !playerTransform.gameObject.activeInHierarchy))
        {
            ResetCameraOffset();
        }

        // 2. ★重要：カメラの現在のオフセットを目標値へ向かって滑らかに動かす
        // Vector3.Lerp(現在の値, 目標の値, 変化率)
        cameraFollowScript.offset = Vector3.Lerp(
            cameraFollowScript.offset,
            targetOffset,
            smoothSpeed * Time.deltaTime
        );
    }

    private void ResetCameraOffset()
    {
        targetOffset = originalOffset; // 直接代入せず、目標を戻す
        isPlayerInside = false;
        playerTransform = null;
        Debug.Log("カメラを元の位置へ戻し始めます");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerTransform = other.transform;

            targetOffset = newOffset; // 目標を新しいオフセットに設定
            Debug.Log("カメラを新しいオフセットへ動かし始めます");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetCameraOffset();
        }
    }
}
