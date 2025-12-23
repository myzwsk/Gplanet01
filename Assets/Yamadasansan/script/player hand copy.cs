using UnityEngine;
using UnityEngine.InputSystem;

public class playerhandcopy : MonoBehaviour
{
    [Header("Settings")]
    public float grabRange = 1.5f;
    public float moveSpeed = 5f;
    public float HoldThreshold = 0.2f; // 0.2秒以上で掴み
    public const float ForceGain = 20f;
    private const float SnapStrength = 10f;

    [Header("State")]
    public float catchObjectFlag = 0;
    public bool isGrabbing = false;
    public Transform handPoint;

    private Rigidbody grabbedObject;
    private Vector3 frozenPosition;
    private Quaternion frozenRotation;
    private Quaternion rotationOffset;
    private bool isPositionLocked = false;
    private float pressTimer = 0f;
    private bool isPressing = false;

    [HideInInspector] public playercatch currentDetector;

    // --- Input System イベント ---
    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isPressing = true;
            pressTimer = 0f;
        }
        else if (context.canceled)
        {
            isPressing = false;
            if (isGrabbing) Release();
        }
    }

    void Update()
    {
        // 1. 長押し判定
        if (isPressing && !isGrabbing)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= HoldThreshold)
            {
                TryGrab();
            }
        }

        // 2. 座標・回転のロック（ここを1つに統合しました）
        if (isPositionLocked && grabbedObject != null && grabbedObject.isKinematic)
        {
            grabbedObject.position = frozenPosition;
            grabbedObject.rotation = frozenRotation;
        }

        if (isGrabbing)
        {
            isPositionLocked = false;
        }
    }

    void FixedUpdate()
    {
        // 1. handPoint の位置更新（Detectorの距離設定を反映）
        if (handPoint != null)
        {
            float distance = (currentDetector != null) ? currentDetector.fromCenter : 0.8f;
            handPoint.position = transform.position + transform.forward * distance;
            handPoint.rotation = transform.rotation;
        }

        // 2. 掴んでいる最中の物理挙動
        if (isGrabbing && grabbedObject != null)
        {
            grabbedObject.isKinematic = false;

            // 位置の追従計算
            Vector3 positionError = handPoint.position - grabbedObject.position;
            Vector3 targetVelocity = positionError * SnapStrength;
            Vector3 velocityError = targetVelocity - grabbedObject.linearVelocity;
            Vector3 force = velocityError * ForceGain * grabbedObject.mass;
            grabbedObject.AddForce(force);

            // 角度の維持（オフセットを適用）
            grabbedObject.rotation = transform.rotation * rotationOffset;
            grabbedObject.angularVelocity = Vector3.zero;
        }
        else
        {
            // 離した後の後始末：フリーズが完了していたら参照を外す
            if (grabbedObject != null && grabbedObject.isKinematic)
            {
                grabbedObject = null;
            }
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.rigidbody != null)
            {
                string grabbedTag = hit.collider.tag;
                if (grabbedTag == "Object01" || grabbedTag == "Object02" || grabbedTag == "Object03")
                {
                    grabbedObject = hit.rigidbody;
                    isGrabbing = true;

                    // 角度オフセットの保存（掴んだ瞬間の向きをキープ）
                    rotationOffset = Quaternion.Inverse(transform.rotation) * grabbedObject.rotation;
                    grabbedObject.linearVelocity = Vector3.zero;

                    // Detectorの取得
                    currentDetector = grabbedObject.GetComponent<playercatch>();

                    // フラグ設定
                    switch (grabbedTag)
                    {
                        case "Object01": catchObjectFlag = 1f; break;
                        case "Object02": catchObjectFlag = 2f; break;
                        case "Object03": catchObjectFlag = 3f; break;
                    }
                }
            }
        }
    }

    void Release()
    {
        isGrabbing = false;
        catchObjectFlag = 0;
        currentDetector = null;

        if (grabbedObject != null)
        {
            ForceFreezeAndLock();
        }
    }

    private void ForceFreezeAndLock()
    {
        if (grabbedObject == null || grabbedObject.isKinematic) return;

        // 物理挙動を止める
        grabbedObject.linearVelocity = Vector3.zero;
        grabbedObject.angularVelocity = Vector3.zero;
        grabbedObject.isKinematic = true;

        // 手の位置に補正して記憶
        if (handPoint != null)
        {
            grabbedObject.position = handPoint.position;
        }

        frozenPosition = grabbedObject.position;
        frozenRotation = grabbedObject.rotation;

        isPositionLocked = true;
        grabbedObject.Sleep();
    }
}
