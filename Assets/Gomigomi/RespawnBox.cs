using UnityEngine;
using UnityEngine.Audio;

public class RespawnBox : MonoBehaviour
{
    public GameObject playerPrefab; // 出現させるプレイヤーのプレハブをここに設定

    // ポッドのアニメーション時間などに応じて、この時間を調整
    public float emergeDelay = 1.5f;

    private GameObject playerInstance; // 生成されたプレイヤーインスタンス

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // プレイヤーをポッドから出現させる処理
    public void StartRespawnSequence(GameObject playerRef)
    {
        // 既存のプレイヤーインスタンスを参照（既にSceneにいるプレイヤー）
        // LBPでは新しいサックボーイがポッドから出てくるため、
        // プレイヤーの新しいインスタンスを生成する方式がより近いです。

        playerInstance = playerRef;
        // プレイヤーの物理挙動を完全に停止させる（重要）
        Rigidbody rb = playerInstance.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;      // 速度リセット
            rb.angularVelocity = Vector3.zero; // 角速度（回転の勢い）リセット
        }

        // 🌟【修正点 1】位置と回転を CheckPointManager の情報でリセット
        // プレイヤーをチェックポイントの「手前」の座標に配置
        playerInstance.transform.position = CheckPointManager.lastCheckpointPosition;
        // プレイヤーをチェックポイントの「正しい向き」に設定
        playerInstance.transform.rotation = CheckPointManager.lastCheckpointRotation;

        // プレイヤーをポッドの位置に配置し、非表示にしておく
        //playerInstance.transform.position = transform.position;
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
            // Rigidbody の速度設定には linearVelocity ではなく velocity を使用するのが一般的です
            rb.linearVelocity = Vector3.up * 5f;
            
        }
        GetComponent<AudioSource>().Play();
        // ポッドは役目を終えたら破棄
        Destroy(gameObject, 5.0f);
    }

}
