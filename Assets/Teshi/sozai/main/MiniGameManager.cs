using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [Header("Clear Settings")]
    public int clearTarget = 3;
    int clearCount = 0;

    public bool isAllCleared = false;

    [Header("Bathroom Event")]
    public BathroomEventController bathroomEvent;

    // ★ ミニゲーム1つクリアしたら呼ぶ
    public void OnMiniGameCleared()
    {
        if (isAllCleared) return;

        clearCount++;
        Debug.Log("MiniGame Clear Count: " + clearCount);

        // 次のミニゲームを表示
        GameManager.Instance.ShowNextMiniGame();

        // 全部クリア判定
        if (clearCount >= clearTarget)
        {
            isAllCleared = true;
            AllClear();
        }
    }

    void AllClear()
    {
        Debug.Log("ALL MINI GAMES CLEARED!");

        if (bathroomEvent != null)
        {
            Debug.Log("BathroomEventController 呼ぶ");
            bathroomEvent.PlayBathroomEvent();
        }
        else
        {
            Debug.LogError("BathroomEventController が設定されていません");
        }
    }

    // デバッグ用
    public void ResetProgress()
    {
        clearCount = 0;
        isAllCleared = false;
        Debug.Log("MiniGame Progress Reset");
    }
}
