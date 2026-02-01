using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player & Camera")]
    public Transform player;
    public Transform miniGamePoint;
    public Camera mainCamera;
    public AudioClip mainBGM;

    [Header("MiniGame Panels（順番に）")]
    public GameObject[] miniGamePanels;

    int currentIndex = 0;
    Vector3 returnPlayerPos;

    CharacterController cc;
    SimpleFollowCamera cameraFollow;

    public bool isMiniGame = false;
    void Start()
    {
        if (mainBGM != null)
            BGMManager.Instance.PlayBGM(mainBGM);
    }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cc = player.GetComponent<CharacterController>();
        cameraFollow = mainCamera.GetComponent<SimpleFollowCamera>();
    }

    // ===== 最初のミニゲーム開始 =====
    public void StartFirstMiniGame()
    {
        currentIndex = 0;
        StartMiniGameBase();

        ShowOnlyCurrentMiniGame();
        Debug.Log("First MiniGame Start");
    }

    // ===== 次のミニゲームへ =====
    public void ShowNextMiniGame()
    {
        miniGamePanels[currentIndex].SetActive(false);
        currentIndex++;

        if (currentIndex < miniGamePanels.Length)
        {
            ShowOnlyCurrentMiniGame();
            Debug.Log("Next MiniGame Start");
        }
        else
        {
            // 全ミニゲーム終了
            FinishMiniGame();
        }
    }

    void ShowOnlyCurrentMiniGame()
    {
        for (int i = 0; i < miniGamePanels.Length; i++)
            miniGamePanels[i].SetActive(i == currentIndex);
    }

    void StartMiniGameBase()
    {
        returnPlayerPos = player.position;

        if (cameraFollow != null)
            cameraFollow.follow = false;

        if (cc != null) cc.enabled = false;
        player.position = miniGamePoint.position;
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        isMiniGame = true;
    }

    // ===== 全ミニゲーム終了 =====
    public void FinishMiniGame()
    {
        if (!isMiniGame) return;

        if (cc != null) cc.enabled = false;
        player.position = returnPlayerPos;
        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;

        if (cameraFollow != null)
            cameraFollow.follow = true;

        isMiniGame = false;
        Debug.Log("All MiniGames Finished");
    }
}
