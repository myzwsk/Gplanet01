using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MemoryMiniGame : MonoBehaviour
{
    [Header("UI")]
    public Slider slider;
    public RectTransform targetZone;

    [Header("Settings")]
    public float showTime = 1.0f;
    public float successRange = 0.1f;

    int successCount = 0;
    float targetValue;
    bool canInput = false;

    void OnEnable()
    {
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        canInput = false;

        targetValue = Random.Range(0.2f, 0.8f);
        SetTargetZone(targetValue);

        targetZone.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);

        targetZone.gameObject.SetActive(false);
        canInput = true;
    }

    void Update()
    {
        if (!canInput) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckResult();
        }
    }

    void CheckResult()
    {
        canInput = false;

        float diff = Mathf.Abs(slider.value - targetValue);

        if (diff <= successRange)
        {
            successCount++;

            if (successCount >= 3)
            {
                Debug.Log("記憶ミニゲーム クリア");
                gameObject.SetActive(false);
                GameManager.Instance.FinishMiniGame();
                return;
            }
        }
        else
        {
            successCount = 0;
        }

        StartCoroutine(StartRound());
    }

    void SetTargetZone(float value)
    {
        RectTransform sliderRect = slider.GetComponent<RectTransform>();
        float width = sliderRect.rect.width;

        Vector2 pos = targetZone.anchoredPosition;
        pos.x = (value - 0.5f) * width;
        targetZone.anchoredPosition = pos;
    }
}
