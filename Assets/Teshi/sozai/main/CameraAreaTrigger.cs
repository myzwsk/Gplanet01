using UnityEngine;
using System.Collections;

public class CameraAreaTrigger : MonoBehaviour
{
    public SmoothFollowCamera cameraFollow;
    public Transform cameraPoint;
    public float moveTime = 1.2f;

    private bool isMoving = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            StartCoroutine(MoveAndFixCamera());
        }
    }

    IEnumerator MoveAndFixCamera()
    {
        isMoving = true;

        // ★ 追従を止める
        cameraFollow.enableFollow = false;

        Transform cam = cameraFollow.transform;
        Vector3 startPos = cam.position;
        Quaternion startRot = cam.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            cam.position = Vector3.Lerp(startPos, cameraPoint.position, t);
            cam.rotation = Quaternion.Slerp(startRot, cameraPoint.rotation, t);
            yield return null;
        }

        // ★ 完全に固定
        cam.position = cameraPoint.position;
        cam.rotation = cameraPoint.rotation;

        isMoving = false;
    }
}
