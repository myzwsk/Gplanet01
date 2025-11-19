using UnityEngine;

public class RespawnBox : MonoBehaviour
{
    public GameObject playerPrefab; // 出現させるプレイヤーのプレハブをここに設定

    private GameObject playerInstance; // 生成されたプレイヤーインスタンス

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // プレイヤーをポッドから出現させる処理
    public void StartRespawnSequence(GameObject playerRef)
    {
        // 既存のプレイヤーインスタンスを参照（既にSceneにいるプレイヤー）
        // LBPでは新しいサックボーイがポッドから出てくるため、
        // プレイヤーの新しいインスタンスを生成する方式がより近いです。

        // ★今回は「既存のプレイヤーを動かす」方式でシンプルに実装します。
        playerInstance = playerRef;

        // プレイヤーをポッドの位置に配置し、非表示にしておく
        playerInstance.transform.position = transform.position;
        playerInstance.SetActive(false);

        // ここで扉が開く、ポッドが割れるなどのアニメーションを開始（Animatorで設定）
        // GetComponent<Animator>().Play("PodOpen"); 

        // アニメーションの終了後（または一定時間後）にプレイヤーを出現させる
        Invoke("EmergePlayer", 1.5f); // 1.5秒後に実行
    }

    void EmergePlayer()
    {
        // プレイヤーをアクティブ化（出現）
        playerInstance.SetActive(true);

        // プレイヤーに上向きの初速を与え、飛び出させる（任意）
        Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.up * 5f;
        }

        // ポッドは役目を終えたら破棄
        Destroy(gameObject);
    }

}
