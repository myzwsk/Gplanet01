using UnityEngine;
using System.Collections;
public class HeartRate : MonoBehaviour
{
    public float rotateDuration = 1.0f; // 回転する時間
    public float waitDuration = 1.0f;   // 待機する時間
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 baseRotation;

    void Start()
    {
        baseRotation = transform.localEulerAngles;
        // ずっと繰り返す処理をスタート
        StartCoroutine(RotateLoop());
    }

    IEnumerator RotateLoop()
    {
        while (true) // 無限ループ
        {
            float timer = 0f;

            // --- 1. 回転フェーズ ---
            while (timer < rotateDuration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / rotateDuration);
                float angle = curve.Evaluate(progress) * 360f;

                transform.localEulerAngles = new Vector3(baseRotation.x, baseRotation.y + angle, baseRotation.z);
                yield return null; // 1フレーム待つ
            }

            // 回転終了時にピタッと角度を合わせる
            transform.localEulerAngles = new Vector3(baseRotation.x, baseRotation.y + 360f, baseRotation.z);

            // --- 2. 待機フェーズ ---
            yield return new WaitForSeconds(waitDuration);
        }
    }
}
