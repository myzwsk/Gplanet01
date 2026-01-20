using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public int clearTarget = 3;
    int clearCount = 0;

    public bool isAllCleared = false; 

    public void OnMiniGameCleared()
    {
        if (isAllCleared) return;

        clearCount++;
        Debug.Log("MiniGame Clear Count: " + clearCount);

        if (clearCount >= clearTarget)
        {
            isAllCleared = true;
            AllClear();
        }
    }

    void AllClear()
    {
        Debug.Log("ALL MINI GAMES CLEARED!");
        // 次のSTEPで演出を書く
    }
}
