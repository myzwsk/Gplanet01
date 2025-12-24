using UnityEngine;
using UnityEngine.InputSystem;

public class playerhandcopy : MonoBehaviour
{
    [Header("Settings")]
    public float grabRange = 1.5f;
    public float HoldThreshold = 0.2f;
    private const float ForceGain = 20f;
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

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started) { isPressing = true; pressTimer = 0f; }
        else if (context.canceled) { isPressing = false; if (isGrabbing) Release(); }
    }

    void Update()
    {
        if (isPressing && !isGrabbing)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer >= HoldThreshold) TryGrab();
        }

        if (isPositionLocked && grabbedObject != null && grabbedObject.isKinematic)
        {
            grabbedObject.position = frozenPosition;
            grabbedObject.rotation = frozenRotation;
        }

        if (isGrabbing) isPositionLocked = false;
    }

    void FixedUpdate()
    {
        if (handPoint != null)
        {
            float distance = (currentDetector != null) ? currentDetector.fromCenter : 0.8f;
            handPoint.position = transform.position + transform.forward * distance;
            handPoint.rotation = transform.rotation;
        }

        if (isGrabbing && grabbedObject != null)
        {
            grabbedObject.isKinematic = false;
            Vector3 positionError = handPoint.position - grabbedObject.position;
            Vector3 targetVelocity = positionError * SnapStrength;
            Vector3 velocityError = targetVelocity - grabbedObject.linearVelocity;
            grabbedObject.AddForce(velocityError * ForceGain * grabbedObject.mass);
            grabbedObject.rotation = transform.rotation * rotationOffset;
            grabbedObject.angularVelocity = Vector3.zero;
        }
        else if (grabbedObject != null && grabbedObject.isKinematic)
        {
            grabbedObject = null;
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
                    rotationOffset = Quaternion.Inverse(transform.rotation) * grabbedObject.rotation;

                    // 🔴 修正点：オブジェクトにプレイヤーの情報を渡す
                    currentDetector = grabbedObject.GetComponent<playercatch>();
                    if (currentDetector != null) currentDetector.playerTransform = this.transform;

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
        if (currentDetector != null) currentDetector.ResetStates();
        currentDetector = null;
        ForceFreezeAndLock();
    }

    private void ForceFreezeAndLock()
    {
        if (grabbedObject == null) return;
        grabbedObject.linearVelocity = Vector3.zero;
        grabbedObject.angularVelocity = Vector3.zero;
        grabbedObject.isKinematic = true;
        if (handPoint != null) grabbedObject.position = handPoint.position;
        frozenPosition = grabbedObject.position;
        frozenRotation = grabbedObject.rotation;
        isPositionLocked = true;
    }
}