using UnityEngine;
using UnityEngine.InputSystem;

public class playerhandcopy : MonoBehaviour
{
    public float catchObjectFlag = 0;
    public float grabRange = 1.5f;
    public float moveSpeed = 5f;
    public float catchObjectMove = 1f;
    public bool isGrabbing = false;
    public Transform handPoint;
    private const float SnapStrength = 10f;
    private Rigidbody grabbedObject;
    private bool hasReleasedObject = false;
    public const float ForceGain = 20f; // ゲイン値を調整
    private Quaternion frozenRotation;
    private float pressTimer = 0f;
    private const float HoldThreshold = 0.2f; // 0.2秒以上押し続けたら「掴み」とみなす
    private bool isPressing = false; // ボタンが押されているかどうかのフラグ


    // 🔴 追加: オブジェクトを離した瞬間の正確な座標を保持
    private Vector3 frozenPosition;

    // 🔴 追加: オブジェクトが現在完全にフリーズロックされているかを示すフラグ
    private bool isPositionLocked = false;
    [HideInInspector] public playercatch currentDetector;
    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isPressing = true;
            pressTimer = 0f; // タイマーリセット
        }
        else if (context.canceled)
        {
            isPressing = false;
            // 離したときに、もし掴んでいたなら「離す」処理を実行
            if (isGrabbing)
            {
                Release();
            }
        }
    }
    void FixedUpdate()
    {
        // 1. handPoint の位置更新 (これはそのままでOK)
        if (handPoint)
        {
            if(currentDetector != null)
            {
                // プレイヤーのTransform + プレイヤーの前方 * 0.8f の位置に設定
                handPoint.position = transform.position + transform.forward * currentDetector.fromCenter;
                handPoint.rotation = transform.rotation;
                handPoint.position = transform.position + transform.forward * currentDetector.fromCenter;
            }
            else
            {
                // プレイヤーのTransform + プレイヤーの前方 * 0.8f の位置に設定
                handPoint.position = transform.position + transform.forward * 0.8f;
                handPoint.rotation = transform.rotation;
                handPoint.position = transform.position + transform.forward * 0.8f;
            }
        }

        if (isGrabbing && grabbedObject != null)
        {
            grabbedObject.isKinematic = false;

            if (handPoint != null)
            {
                Vector3 positionError = handPoint.position - grabbedObject.position;

                // 1. 誤差に基づいて目標速度を計算 (P制御)
                Vector3 targetVelocity = positionError * SnapStrength; // SnapStrengthを流用

                // 2. 現在の速度と目標速度の差 (速度エラー)
                Vector3 velocityError = targetVelocity - grabbedObject.linearVelocity;

                // 3. 速度エラーを解消するために必要な力 (AddForce) を加える
                // Rigidbodyの質量で割ることで、質量によらず同じ追従特性を得る
                Vector3 force = velocityError * ForceGain * grabbedObject.mass;

                grabbedObject.AddForce(force);

                // 慣性を完全に打ち消すために、角速度はゼロにしておく
                grabbedObject.angularVelocity = Vector3.zero;
                grabbedObject.rotation = handPoint.rotation;
            }
        }
        else // 掴んでいない時 (isGrabbing == false)
        {
            // 🚨 修正: ここでは参照クリアのみを行う
            if (grabbedObject != null && grabbedObject.isKinematic)
            {
                // FixedUpdateでフリーズが確認できたら、参照をクリア
                grabbedObject = null;
            }
        }
    }

    void Update()
    {
        // 🔴 長押し判定のロジック
        if (isPressing && !isGrabbing)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= HoldThreshold)
            {
                TryGrab(); // 一定時間経ったら掴みに行く
            }
        }

        // 🔴 既存の座標・回転ロック処理
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

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.rigidbody != null)
            {
                // タグを取得
                string grabbedTag = hit.collider.tag;
                Debug.Log("Ray hit object tag: " + grabbedTag);

                // 掴めるタグを限定する
                if (grabbedTag == "Object01" || grabbedTag == "Object02" || grabbedTag == "Object03")
                {
                    grabbedObject = hit.rigidbody;
                    isGrabbing = true;
                    grabbedObject.linearVelocity = Vector3.zero;

                    currentDetector = grabbedObject.GetComponent<playercatch>();


                    if (currentDetector == null) // Detectorが見つからなかった場合
                    {
                        // ⚠️ この警告がコードに含まれていなければ、当然表示されません。
                        Debug.LogWarning($"掴んだオブジェクト ({grabbedObject.name}) に PushPullDetector_Final がありません。");
                    }

                    switch (grabbedTag)
                    {
                        case "Object01":
                            catchObjectFlag = 1f;
                            break;
                        case "Object02":
                            catchObjectFlag = 2f;
                            break;
                        case "Object03":
                            catchObjectFlag = 3f;
                            break;
                    }
                }
                else
                {
                    Debug.Log("このタグのオブジェクトは掴めません: " + grabbedTag);
                }
            }
        }
    }


    void Release()
    {
        // 1. 掴みフラグをオフ
        isGrabbing = false;
        catchObjectFlag = 0;
        currentDetector = null;

        // 2. 🔴 NEW: オブジェクトへの参照が残っていれば、即座にフリーズ処理を実行
        if (grabbedObject != null)
        {
            ForceFreezeAndLock();
        }
    }

    private void ForceFreezeAndLock()
    {
        if (grabbedObject == null || grabbedObject.isKinematic) return;

        // 1. 速度を完全にリセット
        grabbedObject.linearVelocity = Vector3.zero;
        grabbedObject.angularVelocity = Vector3.zero;

        // 2. KinematicをONにして物理演算を遮断
        grabbedObject.isKinematic = true;

        // 3. 手の目標位置・回転に強制的にテレポートさせる (傾きとズレの同時解消)
        if (handPoint != null)
        {
            // 🔴 位置の強制補正
            grabbedObject.position = handPoint.position;

            // 🔴 回転の強制補正 (斜めになるのを防ぐ)
            grabbedObject.rotation = handPoint.rotation;
            // もしワールド軸と平行にしたい場合は、grabbedObject.rotation = Quaternion.identity; を使用
        }

        // 4. Update() でのロックのために座標と回転を記憶
        frozenPosition = grabbedObject.position;
        // 🔴 回転も記憶
        frozenRotation = grabbedObject.rotation;
        isPositionLocked = true;

        // 5. 物理エンジンを休止させる
        grabbedObject.Sleep();
    }
}
