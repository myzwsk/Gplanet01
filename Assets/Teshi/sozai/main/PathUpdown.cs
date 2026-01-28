using UnityEngine;

public class PathUpdown : MonoBehaviour
{
    public float startDelay = 10f; // 最初の待機
    public float cool = 5f;        // 往復後の待機
    public float speed = 2f;       // 移動速度
    public float Ypos = 2f;        // 下がる距離（スケール前）

    private Vector3 basePos;       // 元の位置
    private Vector3 downPos;       // 下がった位置
    private float timer = 0f;
    private bool started = false;
    private bool goingDown = true;

    void Start()
    {
        basePos = transform.position;

        // スケールを考慮した移動量
        float scaledY = Ypos * transform.localScale.y;

        downPos = basePos - new Vector3(0, scaledY, 0);
    }

    void Update()
    {
        // オブジェクトが active の時だけ動く
        if (!gameObject.activeInHierarchy)
            return;

        // 最初のディレイ
        if (!started)
        {
            timer += Time.deltaTime;
            if (timer >= startDelay)
            {
                started = true;
                timer = 0f;
            }
            return;
        }

        // 移動処理
        if (goingDown)
        {
            transform.position = Vector3.MoveTowards(transform.position, downPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, downPos) < 0.01f)
            {
                goingDown = false;
                timer = 0f;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, basePos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, basePos) < 0.01f)
            {
                goingDown = true;
                timer = 0f;
                started = false; // 再び startDelay → cool の流れへ
                startDelay = cool; // 2回目以降は cool を使う
            }
        }
    }
}
