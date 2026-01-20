using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 5f;

    public float mouseSensitivity = 3f;
    public bool enableFollow = true;

    [Header("Look Settings")]
    public float lookHeight = 2.2f;

    [Header("Vertical Clamp")]
    public float minPitch = -20f; // 下を見る限界
    public float maxPitch = 40f;  // 上を見る限界

    float currentYaw;
    float currentPitch;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        currentPitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!enableFollow || target == null) return;

        // マウス入力
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        currentYaw += mouseX * mouseSensitivity;
        currentPitch -= mouseY * mouseSensitivity; // 上下は反転が自然

        // 上下制限
        currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

        // 回転
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // 位置追従
        Vector3 desiredPosition = target.position + rotation * offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        // 視線
        Vector3 lookTarget = target.position + Vector3.up * lookHeight;
        transform.LookAt(lookTarget);
    }
}
