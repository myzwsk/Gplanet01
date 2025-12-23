using UnityEngine;

public class karimove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.81f; // 重力

    private CharacterController controller;
    private Vector3 velocity; // 重力の影響などを管理する速度ベクトル

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. 入力の取得
        float moveZ = Input.GetAxis("Horizontal");
        float moveX = Input.GetAxis("Vertical");

        // 2. 移動方向の計算（ローカル座標系: プレイヤーが向いている方向基準）
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // 3. CharacterControllerを使って移動を実行
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 4. 重力の適用
        // 地面にいるかどうかを判定
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 地面にいるときはわずかに下に押し付ける
        }

        if (!controller.isGrounded)
        {
            // 親要素が設定されていれば解除する
            if (transform.parent != null)
            {
                // エレベーターに乗っている間は重力の影響を受けず、
                // プレイヤーがジャンプや落下を始めたら解除するのが自然です。
                transform.SetParent(null);
            }
        }
        // 重力による加速
        velocity.y += gravity * Time.deltaTime;

        // 最終的な速度を適用（重力など）
        controller.Move(velocity * Time.deltaTime);
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 1. 衝突相手からエレベーターのスクリプトを取得
        elevator elevatorScript = hit.gameObject.GetComponent<elevator>();

        // 衝突相手がエレベータータグを持っているか確認 (タグ設定が必須)
        if (hit.gameObject.CompareTag("Elevator"))
        {
            // 衝突面が上向き（床に乗った）かを確認
            if (hit.normal.y > 0.8f)
            {
                // ガクガク防止：プレイヤーをエレベーターの子要素にする
                if (transform.parent != hit.transform)
                {
                    transform.SetParent(hit.transform);
                }

                // 動作開始：エレベーターの公開関数を呼び出す
                if (elevatorScript != null) // スクリプトが付いているか最終確認
                {
                  //  elevatorScript.StartElevator(); // ★これがないと動きません！
                }
            }
            else // 側面衝突などの場合
            {
                // 側面衝突でエレベーターの子要素になっていたら解除
                if (transform.parent == hit.transform)
                {
                    transform.SetParent(null);
                }
            }
        }
    }

}
