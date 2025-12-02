using UnityEngine;

public class MoveingCube : MonoBehaviour
{
    [Header("設定")]
    public float rotationSpeed = 3f;
    public string playerTag = "Player";
    public float raycastDistance = 0.5f;

    //  新しく追加: Playerからのローカルオフセット (例: (0, 3, -7) )
    public Vector3 cameraOffset = new Vector3(0f, 3f, -7f);

    private Transform currentPlayer;
    private Transform mainCamera;
    private bool isPlayerOnTop = false;

    private float currentYaw = 0f;
    private float currentPitch = 0f;

    void Start()
    {
        if (Camera.main != null)
        {
            mainCamera = Camera.main.transform;
        }
    }

    void Update()
    {
        CheckPlayerOnTop();

        bool isRightMouseDown = Input.GetMouseButton(1);
        bool enableFreeLook = isPlayerOnTop && isRightMouseDown && currentPlayer != null;

        if (enableFreeLook)
        {
            // --- 自由視点操作の実行 ---

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                currentYaw = currentPlayer.eulerAngles.y;
                currentPitch = mainCamera.localEulerAngles.x;
                if (currentPitch > 180) currentPitch -= 360;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // マウス入力による回転量の更新
            currentYaw += Input.GetAxis("Mouse X") * rotationSpeed;
            currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentPitch = Mathf.Clamp(currentPitch, -90f, 90f);

            //  修正点 1: カメラの親を一時的にPlayerの子であるかのように扱う
            // Playerの回転によってカメラの位置が決まる
            Quaternion playerRotation = Quaternion.Euler(0, currentYaw, 0);

            // Playerの位置とPlayerの回転から、カメラの目的位置（World Position）を計算
            Vector3 desiredPosition = currentPlayer.position + playerRotation * cameraOffset;

            // カメラの位置を目的位置に設定（Playerからの距離を固定）
            mainCamera.position = desiredPosition;

            //  修正点 2: Playerオブジェクト自体をY軸で回転（Playerが回転軸）
            currentPlayer.rotation = playerRotation;

            //  修正点 3: カメラの傾き（見下ろす/見上げる）を設定
            // カメラのローカル回転を更新
            mainCamera.localRotation = Quaternion.Euler(currentPitch, 0, 0);
        }
        else
        {
            // --- 通常時の状態に戻す ---
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (currentPlayer != null && !isPlayerOnTop)
            {
                currentPlayer = null;
            }
        }
    }

    // ... (CheckPlayerOnTop() メソッドは省略、前回と同じRaycast版を使用) ...
    private void CheckPlayerOnTop()
    {
        Vector3 boxTopCenter = transform.position + Vector3.up * (transform.localScale.y / 2f);

        RaycastHit hit;

        if (Physics.Raycast(boxTopCenter, Vector3.up, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag(playerTag))
            {
                currentPlayer = hit.collider.transform;
                isPlayerOnTop = true;
                return;
            }
        }

        isPlayerOnTop = false;
    }
}