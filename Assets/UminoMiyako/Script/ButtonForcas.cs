using UnityEngine;
using UnityEngine.EventSystems; // これが必要
using UnityEngine.UI;

public class ButtonForcas : MonoBehaviour
{
    [SerializeField] private Button _firstSelectButton; // 最初に選択させたいボタン（左側のボタンなど）

    // パネルがオンになった瞬間に呼ばれる
    private void OnEnable()
    {
        // 1フレーム待たないとEventSystemが認識しない場合があるための遅延実行
        StartCoroutine(SelectButtonCoroutine());
    }

    private System.Collections.IEnumerator SelectButtonCoroutine()
    {
        // UIの描画更新を少し待つ
        yield return null;

        // EventSystemに「このボタンを選択状態にせよ」と命令
        if (_firstSelectButton != null)
        {
            EventSystem.current.SetSelectedGameObject(_firstSelectButton.gameObject);
        }
    }

    // オプション：マウスで何もないところをクリックして選択が外れるのを防ぐ
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (_firstSelectButton != null)
            {
                EventSystem.current.SetSelectedGameObject(_firstSelectButton.gameObject);
            }
        }
    }
}