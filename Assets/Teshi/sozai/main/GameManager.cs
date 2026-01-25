using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Transform player;
    public Transform miniGamePoint;
    public Camera mainCamera;

    [Header("Paths")]
    public GameObject[] paths;   // ★追加

    Vector3 returnPlayerPos;
    Vector3 returnCameraPos;
    Quaternion returnCameraRot;

    CharacterController cc;
    SmoothFollowCamera cameraFollow;

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

        returnPlayerPos = player.position;
        returnCameraPos = mainCamera.transform.position;
        returnCameraRot = mainCamera.transform.rotation;

        if (cameraFollow != null)
            cameraFollow.enabled = false;

        if (cc != null) cc.enabled = false;
        player.position = miniGamePoint.position;
        if (cc != null) cc.enabled = true;

        mainCamera.transform.position = cameraPos;

        isMiniGame = true;
    }

    // ===== ミニゲーム終了 =====
    public void FinishMiniGame()
    {
        if (!isMiniGame) return;

        if (cc != null) cc.enabled = false;
        player.position = returnPlayerPos;
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        mainCamera.transform.position = returnCameraPos;
        mainCamera.transform.rotation = returnCameraRot;

        if (cameraFollow != null)
            cameraFollow.enabled = true;

        isMiniGame = false;
    }

    // ===== 道を全部出す =====
    public void ShowAllPaths()
    {
        foreach (var p in paths)
        {
            if (p != null)
                p.SetActive(true);
        }
    }
}
