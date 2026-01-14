using UnityEngine;

public class MiniGameLogic : MonoBehaviour
{
    public float clearTime = 3f;
    float timer;

    void Update()
    {
        Debug.Log("MiniGame Update");
        if (!GameManager.Instance.isMiniGame) return;

        timer += Time.deltaTime;

        if (timer >= clearTime)
        {
            timer = 0f;
            GameManager.Instance.FinishMiniGame();
        }
    }
}
