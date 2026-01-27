using UnityEngine;

public class SimpleFollowCamera : MonoBehaviour
{
    public Transform target;

    [Header("Position")]
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smooth = 8f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2.5f;
    public float minPitch = -30f;
    public float maxPitch = 60f;

    public bool follow = true;

    float yaw;
    float pitch;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!follow || target == null) return;

        // ===== マウス入力 =====
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * mouseSensitivity * 100f * Time.deltaTime;
        pitch -= mouseY * mouseSensitivity * 100f * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // ===== 位置追従 =====
        Vector3 desiredPos = target.position + rotation * offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            smooth * Time.deltaTime
        );

        // ===== 視線 =====
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
