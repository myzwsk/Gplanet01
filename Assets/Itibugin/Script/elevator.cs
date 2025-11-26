using UnityEngine;

public class elevator : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float maxHeight = 5f; // 上昇する最高点のY座標
    public float minHeight = 0f; // ★新設：下降する最低点のY座標

    private Vector3 initialPosition; // ★変更：スクリプト開始時の位置を保存
    private bool isMovingUp = true;
    public bool isMoving = false;

    void Start()
    {
        // 初期位置を保存
        initialPosition = transform.position;
    }

    // ★重要: Update() から FixedUpdate() に移動します！
    void FixedUpdate()
    {
        if (isMoving)
        {
            Vector3 targetPosition;
            if (isMovingUp)
            {
                // 上昇時の目標位置 (X/Zは初期位置を保持し、YをmaxHeightにする)
                targetPosition = new Vector3(initialPosition.x, maxHeight, initialPosition.z);
            }
            else
            {
                // 下降時の目標位置 (X/Zは初期位置を保持し、YをminHeightにする)
                targetPosition = new Vector3(initialPosition.x, minHeight, initialPosition.z); // ★修正
            }

            // ... (移動処理と到達判定はそのまま)
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            if (transform.position == targetPosition)
            {
                isMovingUp = !isMovingUp;
                isMoving = false;
            }
        }
    }

    // ... (OnCollisionEnter や StartElevator() はそのまま残します)
    // OnCollisionEnterは削除済み、StartElevator()はUpdate()から呼び出す想定です
    public void StartElevator()
    {
        if (!isMoving)
        {
            isMoving = true;
            Debug.Log(gameObject.name + ": エレベーター動作開始！");
        }
    }
}
