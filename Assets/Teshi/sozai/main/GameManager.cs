using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Transform player;
    public Transform miniGamePoint;
    public Camera mainCamera;

    Vector3 returnPlayerPos;

    CharacterController cc;
    SimpleFollowCamera cameraFollow;

    public bool isMiniGame = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cc = player.GetComponent<CharacterController>();
        cameraFollow = mainCamera.GetComponent<SimpleFollowCamera>();
    }

    // ===== ミニゲーム開始 =====
    public void StartMiniGame()
    {
        if (isMiniGame) return;

        // プレイヤー位置を保存
        returnPlayerPos = player.position;

        // カメラ追従停止
        if (cameraFollow != null)
            cameraFollow.follow = false;

        // プレイヤー移動（CharacterController対策）
        if (cc != null) cc.enabled = false;
        player.position = miniGamePoint.position;
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        isMiniGame = true;
        Debug.Log("StartMiniGame");
    }

    // ===== ミニゲーム終了 =====
    public void FinishMiniGame()
    {
        if (!isMiniGame) return;

        // プレイヤーを元の位置へ
        if (cc != null) cc.enabled = false;
        player.position = returnPlayerPos;
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        // カメラ追従再開
        if (cameraFollow != null)
            cameraFollow.follow = true;

        isMiniGame = false;
        Debug.Log("FinishMiniGame");
    }
}
