using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class FinalDecisionMiniGame : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text messageText;
    public GameObject miniGameCanvas;

    [Header("Event")]
    public BathroomEventController bathroomEvent;

    [Header("SE")]
    public AudioClip yesSE1;
    public AudioClip yesSE2;
    public AudioClip finalYesSE;
    public AudioClip noSE;

    int step = 0;
    bool isFinished = false;   // ★ 追加：終了フラグ

    void OnEnable()
    {
        step = 0;
        isFinished = false;
        UpdateMessage();
    }

    public void OnYes()
    {
        // ★ もう終わってたら何もしない
        if (isFinished) return;

        if (step == 0)
            SEManager.Instance.PlaySE(yesSE1);
        else if (step == 1)
            SEManager.Instance.PlaySE(yesSE2);
        else if (step == 2)
            SEManager.Instance.PlaySE(finalYesSE);

        step++;

        if (step >= 3)
        {
            isFinished = true;                 // ★ ここで完全終了
            miniGameCanvas.SetActive(false);
            bathroomEvent.PlayBathroomEvent();
            GameManager.Instance.FinishMiniGame();
            return;
        }

        UpdateMessage();
    }

    public MiniGameTrigger miniGameTrigger;

    public void OnNo()
    {
        SEManager.Instance.PlaySE(noSE);

        GameManager.Instance.FinishMiniGame();

        MiniGameResetManager.Instance.ResetAllMiniGames(false);

        // ★ これを必ず呼ぶ
        miniGameTrigger.EndMiniGame();
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.SetActive(false);
    }




    void UpdateMessage()
    {
        if (step == 0)
            messageText.text = "ゲームクリア！\n次へ進みますか？";
        else if (step == 1)
            messageText.text = "本当に進みますか？";
        else if (step == 2)
            messageText.text = "後悔ないですね？";
    }
}
