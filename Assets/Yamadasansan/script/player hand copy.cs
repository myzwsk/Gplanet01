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

    private Rigidbody grabbedObject;


    [HideInInspector] public playercatch currentDetector;
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
    void FixedUpdate()
    {
        if (handPoint)
        {
            handPoint.position = transform.position + transform.forward * 0.8f;
            handPoint.rotation = transform.rotation;
        }
        if (isGrabbing && grabbedObject != null)
        {
            grabbedObject.position = Vector3.Lerp(
                grabbedObject.position,
                handPoint.position,
                moveSpeed * Time.deltaTime * catchObjectMove
            );
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
        isGrabbing = false;
        catchObjectFlag = 0;
        grabbedObject = null;
        currentDetector = null;
    }
}
