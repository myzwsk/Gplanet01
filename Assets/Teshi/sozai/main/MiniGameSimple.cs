using UnityEngine;

public class MiniGameSimple : MonoBehaviour
{
    public MiniGameTrigger trigger;
    public MiniGameManager manager;

    public void ClearMiniGame()
    {
        // カウント加算
        manager.OnMiniGameCleared();
        // ミニゲーム終了
        trigger.EndMiniGame();
    }
}
