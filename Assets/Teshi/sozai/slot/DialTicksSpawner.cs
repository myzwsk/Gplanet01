using UnityEngine;

public class DialTicksSpawner : MonoBehaviour
{
    [SerializeField] private Transform tickPrefab; // Tick_0 を入れる
    [SerializeField] private int count = 12;
    [SerializeField] private float radius = 1.2f;  // 円周の半径

    [ContextMenu("Spawn Ticks")]
    public void Spawn()
    {
        // 既存の子Tickを消す（重複防止）
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var c = transform.GetChild(i);
            if (c.name.StartsWith("Tick_")) DestroyImmediate(c.gameObject);
        }

        float step = 360f / count;
        for (int i = 0; i < count; i++)
        {
            var t = Instantiate(tickPrefab, transform);
            t.name = $"Tick_{i}";

            float ang = i * step * Mathf.Deg2Rad;

            // 円周上に配置（XZ平面）
            Vector3 pos = new Vector3(Mathf.Sin(ang) * radius, 0.15f, Mathf.Cos(ang) * radius);
            t.localPosition = pos;

            // 外向きに回転（見た目整える）
            t.localRotation = Quaternion.Euler(0f, i * step, 0f);
        }
    }
}
