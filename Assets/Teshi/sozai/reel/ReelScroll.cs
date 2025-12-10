using UnityEngine;

public class ReelScroll : MonoBehaviour
{
    public float speed = 1f;      // 回転速度
    public bool isSpinning = false; // 回転中かどうか

    private Material mat;         // Quad のマテリアル
    private float offset = 0f;    // UV オフセット
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Quad の Material を取得（Unity が自動生成したやつ）
        //mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            // 毎フレーム UV の Y 値を動かす
            offset += Time.deltaTime * speed;

            // 0～1 以内に収める（無限ループ）
            offset %= 1f;

            // Material にオフセットを適用
            mat.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
    }
    public void StartSpin()
    {
        isSpinning = true;
    }

    public void StopSpin()
    {
        isSpinning = false;
    }
}
