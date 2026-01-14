using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public Color colorA = Color.yellow; // 黄色
    public Color colorB = Color.white;  // 白

    public float speed = 3.5f;   // ★ 点滅速度（ここが重要）
    public float intensity = 2f;

    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // 0～1を往復する値
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // 黄色 ↔ 白 を補間
        Color emissionColor = Color.Lerp(colorA, colorB, t) * intensity;

        mat.SetColor("_EmissionColor", emissionColor);
    }
}
