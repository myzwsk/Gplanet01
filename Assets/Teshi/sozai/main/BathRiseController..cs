using UnityEngine;
using System.Collections;

public class BathRiseController : MonoBehaviour
{
    public Transform riseGroup;  
    public float startY = -2f;
    public float endY = 0f;
    public float riseSpeed = 0.5f;

    bool isRising = false;

    public void StartRise()
    {
        if (isRising) return;
        StartCoroutine(RiseRoutine());
    }

    IEnumerator RiseRoutine()
    {
        isRising = true;

        Vector3 pos = riseGroup.position;
        pos.y = startY;
        riseGroup.position = pos;

        float t = 0f;

        while (t < 1f)
        {
            t += riseSpeed * Time.unscaledDeltaTime;
            t = Mathf.Clamp01(t);

            pos.y = Mathf.Lerp(startY, endY, t);
            riseGroup.position = pos;

            yield return null;
        }

        isRising = false;
    }
}
