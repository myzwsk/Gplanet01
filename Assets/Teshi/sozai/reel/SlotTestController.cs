using UnityEngine;

public class SlotTestController : MonoBehaviour
{
    public ReelScroll leftReel;
    public ReelScroll centerReel;
    public ReelScroll rightReel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // スペースキーで全部スタート
        if (Input.GetKeyDown(KeyCode.Space))
        {
            leftReel.StartSpin();
            centerReel.StartSpin();
            rightReel.StartSpin();
        }
        // Enterキーで全部ストップ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            leftReel.StopSpin();
            centerReel.StopSpin();
            rightReel.StopSpin();
        }
    }
}
