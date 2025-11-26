using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    // ★この static 変数が、全スクリプトから共有されるリスポーン座標です。
    public static Vector3 lastCheckpointPosition;
    //リスポーン位置のオフセット
    public Vector3 respawnOffset = new Vector3(0f, 0f, -2.0f);
    //リスポーン時の回転（向き）を保存する変数
    public static Quaternion lastCheckpointRotation;

    void Start()
    {
        // ゲーム開始時、まだリスポーン地点が設定されていない場合
        // このチェックポイントの位置を初期リスポーン地点とします。
        if (lastCheckpointPosition == Vector3.zero)
        {
            SetCheckpoint();
        }
    }

    // プレイヤーが触れたらリスポーン地点を更新
    private void OnTriggerEnter(Collider other) // 3Dでは Collider を引数にする
    {
        // プレイヤーのタグ("Player")を持つオブジェクトが触れたら
        if (other.CompareTag("Player"))
        {
            // プレイヤーのRigidbodyを取得
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //プレイヤーがチェックポイントに到達した時点で速度と勢いをリセット
                // これにより、死ぬ直前の移動や回転の影響をほぼ受けなくなります。
                rb.linearVelocity = Vector3.zero;      // 速度（移動の勢い）をリセット
                rb.angularVelocity = Vector3.zero; // 角速度（回転の勢い）をリセット
            }
            SetCheckpoint(); // 位置と回転をまとめて設定

            Debug.Log("Checkpoint Reached! New Respawn Point: " + lastCheckpointPosition);

            // 例: チェックポイントのベルを鳴らす、光らせるなどの演出を入れる
        }
    }
    // リスポーン位置と回転をまとめて設定するメソッド
    private void SetCheckpoint()
    {
        // 位置の計算（オフセット適用済み）
        lastCheckpointPosition = GetRespawnPosition();

        //このチェックポイントの現在の回転を保存
        //これで、プレイヤーはチェックポイントと同じ向き
        lastCheckpointRotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f); // 向きを反転
    }

    // 新しいリスポーン座標を計算するプライベートメソッド
    private Vector3 GetRespawnPosition()
    {
        // ローカルのオフセットをワールド座標に変換して位置に加算
        return transform.position + (transform.rotation * respawnOffset);
    }
}