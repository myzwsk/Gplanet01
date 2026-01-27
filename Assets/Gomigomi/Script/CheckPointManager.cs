using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    
    public static Vector3 lastCheckpointPosition;// 全スクリプトから共有されるリスポーン座標
    public Vector3 respawnOffset = new Vector3(0f, 0f, -2.0f); //リスポーン位置のオフセット
    public static Quaternion lastCheckpointRotation; //リスポーン時の回転（向き）を保存する変数

    private static　GameObject currentlyActiveFire;//全チェックポイントで「今光っているライト」を共有するメモ

    [Header("演出の設定")]
    [SerializeField] private GameObject fireEffect; //炎アセット
    [SerializeField] private UnityEngine.Light CPLight; //ライト
    [SerializeField] private AudioSource audioSource; // 音を鳴らすコンポーネント
    [SerializeField] private AudioClip igniteSound;   // 鳴らしたい音ファイル
    public float activeIntensity = 3.0f; // 普段の明るさ
    public float flashIntensity = 10.0f; // 「ぼっ」と点いた瞬間の一瞬の眩しさ
    public float flashSpeed = 20.0f;     // 眩しさが戻る速さ


    void Start()
    {
        // ゲーム開始時、まだリスポーン地点が設定されていない場合
        // このチェックポイントの位置を初期リスポーン地点とします。
        if (fireEffect != null) fireEffect.SetActive(false);
        if (CPLight != null) CPLight.enabled = false;//ライトなし

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
            // すでに自分の炎がついているなら無視
            if (fireEffect != null && fireEffect.activeSelf) return;

            // プレイヤーのRigidbodyを取得
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //プレイヤーがチェックポイントに到達した時点で速度と勢いをリセット
                // これにより、死ぬ直前の移動や回転の影響をほぼ受けなくなります。
                rb.linearVelocity = Vector3.zero;      // 速度（移動の勢い）をリセット
                rb.angularVelocity = Vector3.zero; // 角速度（回転の勢い）をリセット
            }
            // ★ 炎の点火実行
            IgniteFire();

            SetCheckpoint(); // 位置と回転をまとめて設定
            Debug.Log("チェックポイント");

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
    private void IgniteFire()
    {
        // 1. 前の場所の炎を消す
        if (currentlyActiveFire != null && currentlyActiveFire != fireEffect)
        {
            currentlyActiveFire.SetActive(false);
        }

        // 2. 自分の炎を出し、ライトを「ぼっ」とさせる
        if (fireEffect != null)
        {
            fireEffect.SetActive(true);
            currentlyActiveFire = fireEffect; // メモ帳を更新
        }

        if (CPLight != null)
        {
            CPLight.enabled = true;
            CPLight.intensity = flashIntensity;
        }

        // 3. 音を鳴らす処理
        if (audioSource != null && igniteSound != null)
        {
            audioSource.PlayOneShot(igniteSound);
        }
    }

    void Update()
    {
        // ライトの眩しさを落ち着かせる（炎のゆらぎに見える）
        if (CPLight != null && CPLight.enabled)
        {
            CPLight.intensity = Mathf.MoveTowards(CPLight.intensity, activeIntensity, Time.deltaTime * flashSpeed);
        }
    }
}