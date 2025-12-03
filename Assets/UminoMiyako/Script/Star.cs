using UnityEngine;

public class Star : MonoBehaviour
{
    public Vector3 center = Vector3.zero; // 公転中心
    public float radius = 5f;             // 半径
    public float speed = 1f;              // 公転速度
    public float angle;

    void Start()
    {
    }

    void Update()
    {
        // 公転角度を更新
        angle += speed * Time.deltaTime;

        // 新しい位置を計算
        float x = center.x + Mathf.Cos(angle) * radius;
        float z = center.z + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(x, center.y, z);
    }
}
