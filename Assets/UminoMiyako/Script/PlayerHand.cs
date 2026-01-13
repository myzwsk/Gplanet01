using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
{
    public float catchObjectFlag = 0;
    public float grabRange = 1.5f;
    public float moveSpeed = 10f;
    public float catchObjectMove = 1f;
    public bool isGrabbing = false;
    public Transform handPoint;

    private Rigidbody grabbedObject;
    private Vector3 grabOffset;   // 掴んだ位置のオフセットを保持
    private bool originalKinematicState;

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            TryGrab();
        }
        else if (context.canceled)
        {
            Release();
        }
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
            Vector3 targetPos = handPoint.position + grabOffset;

            // 距離に応じて速度を自動調整
            float distance = Vector3.Distance(grabbedObject.position, targetPos);
            float dynamicSpeed = moveSpeed * distance * catchObjectMove;

            Vector3 newPos = Vector3.MoveTowards(
                grabbedObject.position,
                targetPos,
                dynamicSpeed * Time.deltaTime
            );

            grabbedObject.MovePosition(newPos);
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
                Debug.Log("Ray hit object tag: " + grabbedTag);

                if (grabbedTag == "Object01" || grabbedTag == "Object02" || grabbedTag == "Object03")
                {
                    grabbedObject = hit.rigidbody;
                    isGrabbing = true;

                    // 掴んだ位置のオフセットを記録
                    grabOffset = grabbedObject.position - handPoint.position;

                    // 元の Kinematic 状態を保存してから Kinematic にする
                    originalKinematicState = grabbedObject.isKinematic;
                    grabbedObject.isKinematic = true;

                    switch (grabbedTag)
                    {
                        case "Object01": catchObjectFlag = 1f; break;
                        case "Object02": catchObjectFlag = 2f; break;
                        case "Object03": catchObjectFlag = 3f; break;
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
        if (grabbedObject != null)
        {
            // Kinematic を元に戻す
            grabbedObject.isKinematic = originalKinematicState;
        }

        isGrabbing = false;
        catchObjectFlag = 0;
        grabbedObject = null;
    }
}
