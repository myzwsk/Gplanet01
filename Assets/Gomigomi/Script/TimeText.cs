using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeText : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI Text; 

    [Header("タイピング設定")]
    [SerializeField] private float TypingSpeed = 0.05f; 
    [SerializeField] private float WaitTime = 2.0f; // 次の文章へ行くまでの待ち時間

    [Header("表示する文章リスト")]
    [TextArea(3, 10)] // インスペクターの入力欄を広くして入力しやすくする
    [SerializeField] private List<string> Sentences;

    public GameObject osimai;
    public GameObject end;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // セットし忘れチェック
        if (Text != null && Sentences.Count > 0)
        {
            StartCoroutine(PlaySubtitles());
        }
    }
    // 全ての文章を順番に再生
    IEnumerator PlaySubtitles()
    {
        // sentencesリストの中身を一つずつ取り出して line に入れる
        foreach (string line in Sentences)
        {
            // 1文字ずつ出す演出が終わるまで「待つ」
            yield return StartCoroutine(TypeEffect(line));

            // 文章が出終わったら、設定した秒数だけ「待つ」
            yield return new WaitForSeconds(WaitTime);
        }

        // 全てのリストが終わったらテキストを空にする
        Text.text = "";
        if (osimai != null & end != null)
        {
            yield return new WaitForSeconds(1);
            osimai.SetActive(true);
            end.SetActive(true);
        }
    }

    // 実際に「1文字ずつ」表示する処理
    IEnumerator TypeEffect(string line)
    {
        Text.text = line;

        Text.maxVisibleCharacters = 0;

        for (int i = 0; i <= line.Length; i++)
        {
            Text.maxVisibleCharacters = i; 
            yield return new WaitForSeconds(TypingSpeed); // 設定した秒だけ待機
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
