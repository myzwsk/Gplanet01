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

    public bool isMiniGame = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // ===== ミニゲーム開始 =====
    public void StartMiniGame(Vector3 cameraPos)
    {
        if (isMiniGame) return;

        // CharacterController を一瞬だけ無効化
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // 元の位置・カメラを保存
        returnPlayerPos = player.position;
        returnCameraPos = mainCamera.transform.position;
        returnCameraRot = mainCamera.transform.rotation;

        // カメラ固定
        mainCamera.transform.position = cameraPos;

        // プレイヤーをミニゲームへ
        player.position = miniGamePoint.position;

        // CharacterController を戻す
        if (cc != null) cc.enabled = true;

        isMiniGame = true;
        Debug.Log("StartMiniGame");
    }

    // ===== ミニゲーム終了 =====
    public void FinishMiniGame()
    {
        if (!isMiniGame) return;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        // プレイヤーを元の場所へ戻す
        player.position = returnPlayerPos;

        // カメラも元に戻す
        mainCamera.transform.position = returnCameraPos;
        mainCamera.transform.rotation = returnCameraRot;

        if (cc != null) cc.enabled = true;

        isMiniGame = false;
        Debug.Log("FinishMiniGame");
    }
}
