using UnityEngine;
using TMPro;

public class FinalDecisionMiniGame : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text messageText;
    public GameObject miniGameCanvas;
    public GameObject firstMiniGamePanel;

    [Header("Event")]
    public BathroomEventController bathroomEvent;

    [Header("SE")]
    public AudioSource audioSource;
    public AudioClip yesSE1;
    public AudioClip yesSE2;
    public AudioClip finalYesSE;
    public AudioClip noSE;

    int step = 0;

    void OnEnable()
    {
        step = 0;
        UpdateMessage();

        // ★ AudioSource 自動取得（入れ忘れ防止）
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void OnYes()
    {
        if (step == 0)
            SEManager.Instance.PlaySE(yesSE1);
        else if (step == 1)
            SEManager.Instance.PlaySE(yesSE2);
        else if (step >= 2)
            SEManager.Instance.PlaySE(finalYesSE);

        step++;

        if (step >= 3)
        {
            miniGameCanvas.SetActive(false);
            bathroomEvent.PlayBathroomEvent();
            GameManager.Instance.FinishMiniGame();
            return;
        }

        UpdateMessage();
    }


    public void OnNo()
    {
        SEManager.Instance.PlaySE(noSE);
        MiniGameResetManager.Instance.ResetAllMiniGames();
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
