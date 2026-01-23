using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // リスポーン時に出現させるポッドのプレハブをInspectorで設定
    public GameObject respawnPodPrefab;
    public bool isDie=false;

    // プレイヤーが死亡エリアに触れたときの処理（3D用）
    private void OnTriggerEnter(Collider other)
    {
        // "DeathZone"タグを持つColliderに触れたら死亡処理を実行
        if (other.CompareTag("DeathZone"))
        {
            DieAndRespawn();
        }
    }

    // プレイヤーの死亡とリスポーンポッド生成の処理
    public void DieAndRespawn()
    {
        isDie = true;
        // ポッドプレハブが設定されているか確認
        if (respawnPodPrefab == null)
        {
            Debug.LogError("Respawn Pod Prefab が設定されていません！Inspectorで設定してください。");
            return;
        }

        Debug.Log("Player Died! Spawning Respawn Pod...");

        // 1. プレイヤーを即座に非アクティブ化し、操作不能にする
        // （見た目も消え、ポッドが出現するまでの待機状態になる）
        gameObject.SetActive(false);

        // 2. リスポーンポッドを現在のチェックポイント位置に生成
        Vector3 spawnPosition = CheckPointManager.lastCheckpointPosition;

        // ポッドが地面に埋まらないよう、Y軸を少し上げた位置に生成する（モデルの大きさによって調整）
        GameObject pod = Instantiate(respawnPodPrefab, spawnPosition + new Vector3(0, 0.5f, 0), Quaternion.identity);

        // 3. ポッドスクリプトを取得し、リスポーン処理を開始
        RespawnBox podScript = pod.GetComponent<RespawnBox>();
        if (podScript != null)
        {
            // 生成したポッドに、自分自身（このプレイヤーのGameObject）を渡す
            podScript.StartRespawnSequence(this.gameObject);
        }
        else
        {
            // ポッドのプレハブに RespawnPod スクリプトが付いていない場合のエラー
            Debug.LogError("RespawnPod Prefab に RespawnPod スクリプトがアタッチされていません！");

            // エラー時のフォールバックとして即座にリスポーン
            ForceImmediateRespawn();
        }

    }

    // エラー時やデバッグ用の強制リスポーン関数（ポッドを使わない場合）
    private void ForceImmediateRespawn()
    {
        isDie=false;
        transform.position = CheckPointManager.lastCheckpointPosition;
        gameObject.SetActive(true);
        Debug.LogWarning("Fallback: Player Respawned Immediately due to error.");
    }
}