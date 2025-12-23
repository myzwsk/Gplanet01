using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class GoalZone : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private string nextSceneName = "NextLevelScene";
    [SerializeField] private string targetTag = "Key";
   
    public float textDisplayTime = 2.0f; // テキストを表示する時間

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 指定したタグ（Keyなど）が残っているか探す
            GameObject remainingObject = GameObject.FindWithTag(targetTag);

            if (remainingObject == null)
            {
                // 全て取っていたら次へ
                StartCoroutine(FadeAndLoadScene());
            }
            else
            {
                
            }
        }
    }

    // テキストを表示して一定時間で消す処理

    private IEnumerator FadeAndLoadScene()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }
        SceneManager.LoadScene(nextSceneName);
    }
}
