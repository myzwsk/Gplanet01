using UnityEngine;
using TMPro;

public class FinalDecisionMiniGame : MonoBehaviour
{
    public TMP_Text messageText;
    public GameObject miniGameCanvas;
    public GameObject firstMiniGamePanel;
    public BathroomEventController bathroomEvent;

    int step = 0;

    void OnEnable()
    {
        step = 0;
        UpdateMessage();
    }

    public void OnYes()
    {
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
        Debug.Log("いいえ → 最初からやり直し");

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
