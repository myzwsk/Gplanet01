using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [Header("Clear Settings")]
    public int clearTarget = 3;          // 何回クリアでOKか
    int clearCount = 0;

    [Header("State")]
    public bool isAllCleared = false;    // 全クリア済みか

    [Header("Bathroom Event")]
    public BathroomEventController bathroomEvent;

    // ★ ミニゲーム1回クリア時に呼ばれる
    public void OnMiniGameCleared()
    {
        // すでに全クリアなら何もしない
        if (isAllCleared) return;

        clearCount++;
        Debug.Log("MiniGame Clear Count: " + clearCount);

        // 目標回数に達したら
        if (clearCount >= clearTarget)
        {
            isAllCleared = true;
            AllClear();
        }
    }

    // ★ 全クリア時の処理（STEP4）
    void AllClear()
    {
        Debug.Log("ALL MINI GAMES CLEARED!");

        // 風呂場演出を開始
        if (bathroomEvent != null)
        {
            bathroomEvent.PlayBathroomEvent();
        }
        else
        {
            Debug.LogWarning("BathroomEventController が設定されていません");
        }
    }

    // （任意）デバッグ用：最初からやり直したい時
    public void ResetProgress()
    {
        clearCount = 0;
        isAllCleared = false;
        Debug.Log("MiniGame Progress Reset");
    }
}
