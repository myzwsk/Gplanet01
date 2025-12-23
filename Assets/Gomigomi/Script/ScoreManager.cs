using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    static public int Score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // テキストの表示を入れ替える
        ScoreText.text = Score.ToString();
        //Debug.Log(ScoreText.text);
    }
}
