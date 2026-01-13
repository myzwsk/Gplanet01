using UnityEngine;

public class yamadacamera : MonoBehaviour
{
    // ★ PlayerのTransform
    public Transform playerTarget;

    [Header("Offset Settings")]
    public Vector3 normalOffset = new Vector3(0, 5, -7);
    public Vector3 zoomOffset = new Vector3(0, 3, -3);
    public float zoomSpeed = 5f;

    [Header("Zoom Area (World Position)")]
    public Vector3 areaCenter;   // ズームエリア中心座標
    public Vector3 areaSize;     // ズームエリアの大きさ

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = normalOffset;
    }

    void LateUpdate()
    {
        if (playerTarget == null)
        {
            Debug.LogWarning("追跡対象のPlayerが設定されていません。");
            return;
        }

        // プレイヤーがズームエリア内か？
        bool isInZoomArea = IsInsideArea(playerTarget.position);

        // offsetをスムーズに切り替え
        Vector3 targetOffset = isInZoomArea ? zoomOffset : normalOffset;
        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            Time.deltaTime * zoomSpeed
        );

        // カメラ位置更新
        transform.position = playerTarget.position + currentOffset;

        // 必要なら向きを固定
        // transform.LookAt(playerTarget);
    }

    bool IsInsideArea(Vector3 pos)
    {
        Vector3 half = areaSize * 0.5f;

        return
            pos.x >= areaCenter.x - half.x && pos.x <= areaCenter.x + half.x &&
            pos.y >= areaCenter.y - half.y && pos.y <= areaCenter.y + half.y &&
            pos.z >= areaCenter.z - half.z && pos.z <= areaCenter.z + half.z;
    }

    // Sceneビューでズーム範囲を可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}
