using UnityEngine;

public class elevator : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float maxHeight = 5f;
    private float minHeight;
    private bool isPlayerOnBoard = false;

    void Start()
    {
        minHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        float targetY = isPlayerOnBoard ? maxHeight : minHeight;
        Vector3 currentPos = transform.position;

        if (Mathf.Abs(currentPos.y - targetY) > 0.001f)
        {
            float newY = Mathf.MoveTowards(currentPos.y, targetY, moveSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(currentPos.x, newY, currentPos.z);
        }
    }

    // CharacterControllerが「トリガー」に入った時に反応
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerOnBoard = true;
            other.transform.SetParent(transform); // 親子関係にする
            Debug.Log("playerが乗りました");
        }
    }

    // CharacterControllerが「トリガー」から出た時に反応
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player"))
        {
            isPlayerOnBoard = false;
            other.transform.SetParent(null); // 親子関係を解除
            Debug.Log("playerが降りました");
        }
    }

}
