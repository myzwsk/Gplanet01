using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
{
    public float catchObjectFlag = 0;
    public float grabRange = 2.0f; // 少し余裕を持たせる
    public float moveSpeed = 10f;
    public float catchObjectMove = 1f;
    public bool isGrabbing = false;
    public Transform handPoint;

    // ★ プレイヤー自身の半径（カプセルコライダーの半径など）
    public float playerRadius = 0.4f;
    // ★ 追加の安全マージン
    public float safetyMargin = 0.2f;

    private Rigidbody grabbedObject;
    private Vector3 grabOffset;
    private bool originalKinematicState;

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started) TryGrab();
        else if (context.canceled) Release();
    }

    void Update()
    {
        if (handPoint)
        {
            handPoint.position = transform.position + transform.forward * 0.8f;
            handPoint.rotation = transform.rotation;
        }

        if (isGrabbing && grabbedObject != null)
        {
            Collider col = grabbedObject.GetComponent<Collider>();
            Vector3 closest = col.ClosestPoint(handPoint.position);

            // ★ 離し判定もオブジェクトのサイズに合わせて調整
            float maxSeparation = 0.5f;
            if (Vector3.Distance(closest, handPoint.position) > maxSeparation)
            {
                Release();
                return;
            }

            Vector3 targetPos = handPoint.position + grabOffset;
            Vector3 direction = targetPos - grabbedObject.position;
            float distance = direction.magnitude;

            grabbedObject.linearVelocity = direction.normalized * moveSpeed * distance * catchObjectMove;
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
                    originalKinematicState = grabbedObject.isKinematic;
                    grabbedObject.isKinematic = false;

                    // --- 長方形の面に対応した動的押し出し ---

                    // 1. プレイヤーの位置から、オブジェクトのコライダー上の「最も近い点」を探す
                    Collider objCol = hit.collider;
                    Vector3 closestPoint = objCol.ClosestPoint(transform.position);

                    // 2. 「オブジェクトの中心」から「最も近い表面」までの距離を計算（これがその面における厚み）
                    // 水平方向(XZ)のみで計算するのがコツ
                    Vector3 centerToSurface = closestPoint - grabbedObject.position;
                    centerToSurface.y = 0;
                    float objectThickness = centerToSurface.magnitude;

                    // 3. 必要な距離 = プレイヤーの半径 + オブジェクトの厚み + 余裕
                    float dynamicMinDist = playerRadius + objectThickness + safetyMargin;

                    // 4. 現在の距離をチェックして押し出し
                    Vector3 playerToObj = grabbedObject.position - transform.position;
                    Vector3 directionXZ = new Vector3(playerToObj.x, 0, playerToObj.z);
                    float currentDistXZ = directionXZ.magnitude;

                    if (currentDistXZ < dynamicMinDist)
                    {
                        Vector3 pushPos = transform.position + (directionXZ.normalized * dynamicMinDist);
                        pushPos.y = grabbedObject.position.y;
                        grabbedObject.position = pushPos;
                    }

                    // 5. 補正後にオフセットを記録
                    grabOffset = grabbedObject.position - handPoint.position;

                    // --- 以下、タグ判定などは同じ ---
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
        if (grabbedObject != null)
        {
            grabbedObject.isKinematic = originalKinematicState;
            grabbedObject.linearVelocity = Vector3.zero;
        }
        isGrabbing = false;
        catchObjectFlag = 0;
        grabbedObject = null;
    }
}