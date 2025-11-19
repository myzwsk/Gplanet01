using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    // ★この static 変数が、全スクリプトから共有されるリスポーン座標です。
    public static Vector3 lastCheckpointPosition;

    void Start()
    {
        // ゲーム開始時、まだリスポーン地点が設定されていない場合
        // このチェックポイントの位置を初期リスポーン地点とします。
        if (lastCheckpointPosition == Vector3.zero)
        {
            lastCheckpointPosition = transform.position;
        }
    }

    // プレイヤーが触れたらリスポーン地点を更新
    private void OnTriggerEnter(Collider other) // 3Dでは Collider を引数にする
    {
        // プレイヤーのタグ("Player")を持つオブジェクトが触れたら
        if (other.CompareTag("Player"))
        {
            // このチェックポイントの位置を新しいリスポーン地点に設定
            lastCheckpointPosition = transform.position;

            Debug.Log("Checkpoint Reached! New Respawn Point: " + lastCheckpointPosition);

            // 例: チェックポイントのベルを鳴らす、光らせるなどの演出を入れる
        }
    }
}