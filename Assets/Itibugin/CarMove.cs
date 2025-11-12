using UnityEngine;

public class CarMove : MonoBehaviour
{
    public float moveSpeed = 25f;

    // インスペクターから設定するデスポーンするX座標の限界値
    public float despawnXBoundary = 2f;

    void Update()
    {
        // 車の前方（Z軸方向）に一定速度で移動させる
        transform.Translate(Vector3.forward* moveSpeed * Time.deltaTime);

        // X座標が指定した境界値を超えたら（ここでは絶対値で処理）
        if (transform.position.x > despawnXBoundary)
        {
            // このゲームオブジェクトを破棄（デスポーン）する
            Destroy(gameObject);
        }
    }
}
