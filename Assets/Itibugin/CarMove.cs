using UnityEngine;

public class CarMove : MonoBehaviour
{
    public float moveSpeed = 25f;

    void Start()
    {
        // ゲームオブジェクトがシーンに生成されてから2秒後に自身を破棄する
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        // 車の前方（Z軸方向）に一定速度で移動させる
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);


    }
}