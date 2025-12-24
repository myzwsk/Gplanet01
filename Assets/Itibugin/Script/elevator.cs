using UnityEngine;

public class elevator : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float maxHeight = 5f;
    private float minHeight;
    private bool isPlayerOnBoard = false;
    private Transform playerTransform; // 乗っているプレイヤーを記録しておく

    void Start()
    {
        minHeight = transform.position.y;
    }

    void FixedUpdate()
    {
        // ★追加：プレイヤーが乗っているはずなのに、非アクティブ（死亡）になったら強制リセット
        if (isPlayerOnBoard && (playerTransform == null || !playerTransform.gameObject.activeInHierarchy))
        {
            ResetElevator();
        }

        float targetY = isPlayerOnBoard ? maxHeight : minHeight;
        Vector3 currentPos = transform.position;

        if (Mathf.Abs(currentPos.y - targetY) > 0.001f)
        {
            float newY = Mathf.MoveTowards(currentPos.y, targetY, moveSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(currentPos.x, newY, currentPos.z);
        }
    }

    // ★追加：エレベーターを安全にリセットする関数
    private void ResetElevator()
    {
        isPlayerOnBoard = false;
        if (playerTransform != null)
        {
            // 親子関係を解除（プレイヤーが消えていても安全のため）
            if (playerTransform.parent == transform)
            {
                playerTransform.SetParent(null);
            }
            playerTransform = null;
        }
        Debug.Log("プレイヤーの消失を検知したためエレベーターを戻します");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnBoard = true;
            playerTransform = other.transform; // プレイヤーを記録
            other.transform.SetParent(transform);
            Debug.Log("playerが乗りました");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnBoard = false;
            playerTransform = null; // 記録を消す
            other.transform.SetParent(null);
            Debug.Log("playerが降りました");
        }
    }

}
