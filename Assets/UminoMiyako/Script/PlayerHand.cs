using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHand : MonoBehaviour
{
    public float catchObjectFlag = 0;
    public float grabRange = 3f;
    public float moveSpeed = 5f;
    public bool isGrabbing = false;
    public Transform handPoint;
    public KeyCode grabKey = KeyCode.F;
    
    private Rigidbody grabbedObject;
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
            Vector3 direction = (handPoint.position - grabbedObject.position).normalized;
            grabbedObject.MovePosition(grabbedObject.position + direction * moveSpeed * Time.deltaTime);
            
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabRange))
        {
            if (hit.rigidbody != null)
            {
                grabbedObject = hit.rigidbody;
                isGrabbing = true;
                grabbedObject.linearVelocity = Vector3.zero;

                // タグを取得
                string grabbedTag = hit.collider.tag;
                Debug.Log("Grabbed object tag: " + grabbedTag);
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
        }
    }

    void Release()
    {
        isGrabbing = false;
        catchObjectFlag = 0;
        grabbedObject = null;
    }
}