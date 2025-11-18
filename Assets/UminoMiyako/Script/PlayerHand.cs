using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public Transform handPoint;
    public float grabRange = 3f;
    public bool isGrabbing = false;
    public KeyCode grabKey = KeyCode.F;

    private Rigidbody grabbedObject;
    public float moveSpeed = 5f; // �����ŋ߂Â��鑬�x

    void Update()
    {
        if (Input.GetKeyDown(grabKey))
        {
            TryGrab();
        }
        else if (Input.GetKeyUp(grabKey))
        {
            Release();
        }

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
            }
        }
    }

    void Release()
    {
        isGrabbing = false;
        grabbedObject = null;
    }
}