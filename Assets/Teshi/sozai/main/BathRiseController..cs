using UnityEngine;
using System.Collections;

public class PathRiseController : MonoBehaviour
{
    public float startY = -10f;   // 最初は下に隠す
    public float endY = 0f;       // 出したい高さ
    public float riseSpeed = 2f;

    bool isRising = false;

    void Start()
    {
        Vector3 pos = transform.position;
        pos.y = startY;
        transform.position = pos;
    }

    public void StartRise()
    {
        if (isRising) return;
        StartCoroutine(RiseRoutine());
    }

    IEnumerator RiseRoutine()
    {
        isRising = true;

        Vector3 pos = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * riseSpeed;
            pos.y = Mathf.Lerp(startY, endY, t);
            transform.position = pos;
            yield return null;
        }

        pos.y = endY;
        transform.position = pos;
        isRising = false;
    }
}
