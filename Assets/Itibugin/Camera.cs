using UnityEngine;

public class camera : MonoBehaviour
{
    // 追尾対象のPlayerオブジェクト
    public Transform target;
    // Playerとカメラの間のオフセット（距離と角度）
    public Vector3 offset;
    // 追尾の滑らかさを調整する係数
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // 目的のポジションを計算: Playerの位置 + オフセット
        Vector3 desiredPosition = target.position + offset;

        // 現在の位置から目的のポジションへ滑らかに移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Playerの方向を見る（オプション：追尾カメラのタイプによる）
        // transform.LookAt(target); 
    }
}
