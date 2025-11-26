using UnityEngine;

public class Kariwalk : MonoBehaviour
{
    // 移動速度
    public float moveSpeed = 5f;
    // ジャンプ力
    public float jumpPower = 7f;
    // 地面にいるか判定するためのフラグ
    private bool isGrounded;

    // Rigidbodyコンポーネントへの参照を格納する変数
    private Rigidbody rb;

    void Start()
    {
        // 最初にRigidbodyコンポーネントを取得しておく
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. 移動入力の処理
        // D: +1 (前進), A: -1 (後退)
        float forwardBackwardInput = Input.GetAxis("Horizontal");
        // S: +1, W: -1
        float sideInput = Input.GetAxis("Vertical");

        // Z軸 (Vector3.forward) に D/A の入力（前後移動）を適用
        // X軸 (Vector3.right) に W/S の入力（左右移動）を適用
        Vector3 moveDirection = new Vector3(sideInput, 0, forwardBackwardInput);

        // Sで右、Wで左になるようにX軸の入力を反転
        moveDirection.x = -moveDirection.x;

        // 斜め移動の速度を一定に保つための正規化
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        // 3. 最終的な移動の実行 (TranslateはUpdateで実行)
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 4. ジャンプ入力の処理
        // Spaceキーが押され、かつ地面についている場合
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // 上方向にジャンプ力を加える
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isGrounded = false; // ジャンプしたので、地面から離れた状態にする
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 衝突した相手が「地面」であると判断できる場合（ここではTagは無視）
        // Rigidbodyを使用しているため、他のオブジェクトと接触したら地面と見なす
        isGrounded = true;
    }

}
