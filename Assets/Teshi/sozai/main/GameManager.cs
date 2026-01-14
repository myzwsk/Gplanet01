using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Transform player;
    public Transform miniGamePoint;
    public Camera mainCamera;

    Vector3 returnPlayerPos;
    Vector3 returnCameraPos;
    Quaternion returnCameraRot;

    CharacterController cc;
    SmoothFollowCamera cameraFollow;   // ← カメラ追従スクリプト

    public bool isMiniGame = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cc = player.GetComponent<CharacterController>();
        cameraFollow = mainCamera.GetComponent<SmoothFollowCamera>();
    }

    // ===== ミニゲーム開始 =====
    public void StartMiniGame(Vector3 cameraPos)
    {
        if (isMiniGame) return;

        // 元の状態を保存
        returnPlayerPos = player.position;
        returnCameraPos = mainCamera.transform.position;
        returnCameraRot = mainCamera.transform.rotation;

        // カメラ追従を止める（固定）
        if (cameraFollow != null)
            cameraFollow.enabled = false;

        // Player テレポート（CharacterController対策）
        if (cc != null) cc.enabled = false;
        player.position = miniGamePoint.position;
        if (cc != null) cc.enabled = true;

        // カメラを固定位置へ
        mainCamera.transform.position = cameraPos;

        isMiniGame = true;
        Debug.Log("StartMiniGame");
    }

    // ===== ミニゲーム終了 =====
    public void FinishMiniGame()
    {
        if (!isMiniGame) return;

        // Player を元の場所へ戻す
        if (cc != null) cc.enabled = false;
        player.position = returnPlayerPos;
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        // カメラを元の状態へ
        mainCamera.transform.position = returnCameraPos;
        mainCamera.transform.rotation = returnCameraRot;

        // カメラ追従を再開
        if (cameraFollow != null)
            cameraFollow.enabled = true;

        isMiniGame = false;
        Debug.Log("FinishMiniGame");
    }
}
