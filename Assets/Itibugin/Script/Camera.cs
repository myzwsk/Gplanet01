using UnityEngine;

public class camera : MonoBehaviour
{
    // 追尾対象のPlayerオブジェクト
    public Transform target;

    [Header("通常の追尾設定")]
    // 通常のPlayer追尾時のオフセット (Inspectorで設定)
    public Vector3 offset = new Vector3(0f, 3f, -7f);
    // 追尾の滑らかさを調整する係数
    public float smoothSpeed = 0.125f;

    [Header("視点移動時の固定設定")]
    // 直方体乗車時の固定オフセット (Playerを軸にしたカメラ位置)
    public Vector3 fixedOffset = new Vector3(0f, 5f, -10f);
    // 視点回転速度
    public float rotationSpeed = 3f;

    [Header("直方体検出設定")]
    public string targetBoxTag = "TargetBoxTag"; // 直方体のTag名
    public float detectionRadius = 1.5f; // Playerが直方体の上にいると判定する水平距離の許容範囲

    private float currentYaw = 0f;
    private float currentPitch = 0f;
    private Transform targetBox; // 検出された直方体のTransform

    void Start()
    {
        // シーン内の直方体オブジェクトを検索
        GameObject boxObject = GameObject.FindWithTag(targetBoxTag);
        if (boxObject != null)
        {
            targetBox = boxObject.transform;
        }
        else
        {
            Debug.LogError("直方体 (Tag: " + targetBoxTag + ") が見つかりません。タグ設定を確認してください。");
        }
    }

    void LateUpdate()
    {
        // 1. 直方体の上にPlayerがいるか判定
        bool isPlayerOnBox = CheckIfPlayerIsOnBox();

        if (isPlayerOnBox && targetBox != null)
        {
            // --- 視点固定と回転の処理 ---

            bool isRightMouseDown = Input.GetMouseButton(1);

            // 制御開始時の初期化
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                currentYaw = transform.eulerAngles.y;
                currentPitch = transform.localEulerAngles.x;
                if (currentPitch > 180) currentPitch -= 360; // 角度を-180～180に正規化
            }

            // 視点回転は右クリック長押し中のみ有効
            if (isRightMouseDown)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                // マウス入力による回転量の更新
                currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;
                currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
                currentPitch = Mathf.Clamp(currentPitch, -90f, 90f);
            }
            else
            {
                // 右クリックを離したらカーソルを元に戻す
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            // カメラの位置、向きを設定
            Quaternion cameraYawRotation = Quaternion.Euler(0, currentYaw, 0);

            // Playerの位置を原点とし、カメラの水平回転から目的位置を計算
            // fixedOffset を使用
            Vector3 desiredPosition = target.position + cameraYawRotation * new Vector3(0, fixedOffset.y, fixedOffset.z);

            transform.position = desiredPosition;
            transform.LookAt(target);
        }
        else
        {
            // --- 通常の追尾処理に戻す ---

            // カーソルを元に戻す
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // 目的のポジションを計算: Playerの位置 + offset
            Vector3 desiredPosition = target.position + offset;

            // 現在の位置から目的のポジションへ滑らかに移動
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            // Playerの方向を見る
            transform.LookAt(target);
        }
    }

    /// <summary>
    /// Playerが直方体の上にいるかをチェックします。
    /// </summary>
    private bool CheckIfPlayerIsOnBox()
    {
        if (targetBox == null || target == null) return false;

        // PlayerとBoxの水平距離をチェック
        Vector3 horizontalDelta = target.position - targetBox.position;
        horizontalDelta.y = 0;

        // detectionRadius 以内にいるか確認
        if (horizontalDelta.magnitude < detectionRadius)
        {
            // 直方体の上面のY座標を計算
            float boxTopY = targetBox.position.y + (targetBox.localScale.y / 2f);

            // Playerの足元（または中心）と直方体上面の高さの差をチェック
            // Playerの高さ（約1.8～2.0を想定）を考慮し、足元がBox上面の近くにあるか判定
            float heightDelta = Mathf.Abs(target.position.y - boxTopY);

            // Playerの中心がBox上面からごくわずかに上にいる場合 (例: 0.1m～1.0m)
            // CharacterControllerのサイズにもよるため、適宜調整
            if (heightDelta > 0.1f && heightDelta < 1.0f)
            {
                return true;
            }
        }
        return false;
    }
}
