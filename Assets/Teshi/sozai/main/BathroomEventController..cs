using UnityEngine;
using System.Collections;

public class BathroomEventController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject bathWater;
    public GameObject path;

    [Header("Water Settings")]
    public float verticalSpeed = 10f;
    public Vector3 targetWaterScale = new Vector3(35f, 35f, 13f);

    [Header("Shower Control")]
    public ShowerController showerController;

    [Header("Shower Sound")]
    public AudioClip showerLoopSE;

    bool isPlayed = false;
    bool pathShown = false;

    public void PlayBathroomEvent()
    {
        if (isPlayed) return;
        isPlayed = true;

        // 水 初期化
        bathWater.transform.localScale = new Vector3(
            targetWaterScale.x,
            0.01f,
            targetWaterScale.z
        );

        bathWater.SetActive(true);
        path.SetActive(false);
        pathShown = false;

        // シャワー開始（見た目）
        if (showerController != null)
            showerController.StartShower();

        // ★ シャワー音スタート
        if (showerLoopSE != null && SEManager.Instance != null)
            SEManager.Instance.PlayLoopSE(showerLoopSE, 0.6f);

        StartCoroutine(FillBath());
    }

    IEnumerator FillBath()
    {
        Vector3 scale = bathWater.transform.localScale;

        while (scale.y < targetWaterScale.y)
        {
            scale.y = Mathf.MoveTowards(
                scale.y,
                targetWaterScale.y,
                verticalSpeed * Time.unscaledDeltaTime
            );

            bathWater.transform.localScale = scale;

            // ★ 水が十分たまったら道を出す
            if (!pathShown && scale.y >= targetWaterScale.y)
            {
                pathShown = true;
                path.SetActive(true);
                Debug.Log("水がたまった → 道を表示");
            }

            yield return null;
        }

        // ★ 満タン処理
        bathWater.transform.localScale = targetWaterScale;

        if (showerController != null)
            showerController.StopShower();

        // ★ シャワー音停止（重要）
        if (SEManager.Instance != null)
            SEManager.Instance.StopLoopSE();

        Debug.Log("水が満タン → シャワー停止");
    }
}
