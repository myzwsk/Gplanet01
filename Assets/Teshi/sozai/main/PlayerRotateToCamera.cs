using UnityEngine;

public class PlayerRotateToCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float rotateSpeed = 10f;
    public bool enableRotate = true;

    void Update()
    {
        if (!enableRotate || cameraTransform == null) return;

        float targetYaw = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0f, targetYaw, 0f);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }
}
