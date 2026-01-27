using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeText : MonoBehaviour
{
    [Header("UI設定")]
    [SerializeField] private TextMeshProUGUI Text; // インスペクターでText(TMP)をドラッグ＆ドロップする場所

    [Header("タイピング設定")]
    [SerializeField] private float TypingSpeed = 0.05f;    // 1文字が出る速度（小さいほど速い）
    [SerializeField] private float WaitTime = 2.0f; // 文章が全部出終わった後に、次の文章へ行くまでの待ち時間

    [Header("表示する文章リスト")]
    [TextArea(3, 10)] // インスペクターの入力欄を広くして入力しやすくする
    [SerializeField] private List<string> Sentences; // ここに好きなだけ文章を追加できる

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // セットし忘れ防止チェック
        if (Text != null && Sentences.Count > 0)
        {
            // 文章表示のコルーチン（時間差処理）を開始
            StartCoroutine(PlaySubtitles());
        }
    }
    // 全ての文章を順番に再生する司令塔
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
    }

    // 実際に「1文字ずつ」表示する処理
    IEnumerator TypeEffect(string line)
    {
        // 1. まずテキストコンポーネントに全文をセットする（この時点ではまだ見えない）
        Text.text = line;

        // 2. 見える文字数を 0 にリセット
        Text.maxVisibleCharacters = 0;

        // 3. 文字数分だけループを回して、1文字ずつ表示文字数を増やしていく
        for (int i = 0; i <= line.Length; i++)
        {
            Text.maxVisibleCharacters = i; // 表示する文字数を更新

            // 設定したスピード（秒）だけ待機
            yield return new WaitForSeconds(TypingSpeed);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
