using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public GameObject score_object = null; // Textオブジェクト
    public int Score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトからTextコンポーネントを取得
        Text score_text = score_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        //score_text.text = Score;
    }
}
