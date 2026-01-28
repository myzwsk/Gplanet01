using UnityEngine;

public class PathLota : MonoBehaviour
{
    public float startDelay = 5f;   // 最初の待機
    public float cool = 3f;         // クールタイム
    public float speed = 15f;       // 回転速度
    public int lotadire = 1;        // 回転方向（1 or -1）
    public float rotateDuration = 0.2f; // 回転時間

    public Vector3 rotateAxis = Vector3.up; // ← 回転軸を指定できるようにした

    private float timer = 0f;
    private CapsuleCollider capsule;
    private bool started = false;
    private bool rotating = false;

    private float rotateTimer = 0f;

    private Quaternion startRot; // 最初の角度を保存

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        capsule.enabled = false;

        startRot = transform.rotation;
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;

        // 最初の待機
        if (!started)
        {
            timer += Time.deltaTime;
            if (timer >= startDelay)
            {
                started = true;
                timer = 0f;
                StartRotate();
            }
            return;
        }

        // 回転中
        if (rotating)
        {
            rotateTimer += Time.deltaTime;

            // スケール補正
            float scaleFix = 1f / transform.localScale.magnitude;

            // ★ 回転軸を使って回転
            transform.Rotate(rotateAxis.normalized * (speed * lotadire * scaleFix));

            if (rotateTimer >= rotateDuration)
            {
                rotating = false;
                rotateTimer = 0f;

                capsule.enabled = false;

                // 元の角度に戻す
                transform.rotation = startRot;

                timer = 0f;
            }
            return;
        }

        // クールタイム
        timer += Time.deltaTime;
        if (timer >= cool)
        {
            timer = 0f;
            StartRotate();
        }
    }

    void StartRotate()
    {
        rotating = true;
        capsule.enabled = true;
    }
}
