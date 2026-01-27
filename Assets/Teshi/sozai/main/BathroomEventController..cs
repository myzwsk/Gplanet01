using UnityEngine;
using System.Collections;

public class BathroomEventController : MonoBehaviour
{
    [Header("Objects")]
    public GameObject bathWater;
    public GameObject path;   // 出したい道

    [Header("Water Settings")]
    public float verticalSpeed = 10f;
    public Vector3 targetWaterScale = new Vector3(35f, 35f, 13f);

    [Header("Shower Control")]
    public ShowerController showerController;


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
        path.SetActive(false);   // ★最初は道を消す
        showerController.StartShower();
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

            // ★ 水がたまったら道を出す
            if (!pathShown && scale.y >= targetWaterScale.y * 1.0f)
            {
                pathShown = true;
                path.SetActive(true);
                Debug.Log("水がたまった → 道を表示");
            }
            // 水が満タンになったらシャワー停止
            


            yield return null;
        }
        if (showerController != null)
        {
            showerController.StopShower();
            Debug.Log("水が満タン → シャワー停止");
        }
        bathWater.transform.localScale = targetWaterScale;
    }
}
