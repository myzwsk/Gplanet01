using UnityEngine;

public class HackAutoStart : MonoBehaviour
{
    public GameObject hackCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hackCanvas.SetActive(true);   // Playした瞬間に表示
        Time.timeScale = 0f;          // ゲームを止める（ハッキング中）
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
