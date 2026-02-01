using UnityEngine;

public class FloatingArrow : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public float floatHeight = 0.5f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        float y = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = startPos + new Vector3(0, y, 0);
    }
}
