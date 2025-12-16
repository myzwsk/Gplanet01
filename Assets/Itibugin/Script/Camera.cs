using UnityEngine;

public class camera : MonoBehaviour
{
    // ★ PlayerのTransformコンポーネントを格納する変数
    public Transform playerTarget;

    // ★ Playerと自分との間に保ちたい「オフセット（ずれ）」を格納する変数
    // 例: new Vector3(0, 5, -7) の場合、Playerより上方向に5、奥（後ろ）方向に7の位置
    public Vector3 offset;

    // Start関数内で処理するのではなく、フレームの更新処理内で実行する
    // 物理演算の影響を受ける場合はFixedUpdate、カメラの場合はLateUpdateが適しています。
    void LateUpdate()
    {
        // Playerが設定されているか確認
        if (playerTarget == null)
        {
            Debug.LogWarning("追跡対象のPlayerが設定されていません。");
            return; // 処理を中断
        }

        // ① 追従する目標の位置を計算
        // 目標位置 = Playerの位置 + オフセット
        Vector3 targetPosition = playerTarget.position + offset;

        // ② 自分の位置を目標の位置に移動させる
        transform.position = targetPosition;

        // オプション：常にPlayerの方向を向く（カメラの場合）
        // transform.LookAt(playerTarget);
    }
}
