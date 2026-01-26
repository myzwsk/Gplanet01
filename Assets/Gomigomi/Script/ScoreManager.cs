using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    static public int Score = 0;

    private escape Esc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Esc = FindObjectOfType<escape>();
        Score = Esc.Score;
    }

    // Update is called once per frame
    void Update()
    {

        // テキストの表示を入れ替える
        ScoreText.text = Score.ToString();
        Esc.Score = Score;
        //Debug.Log(ScoreText.text);
    }
}
