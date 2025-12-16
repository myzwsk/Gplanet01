using UnityEngine;

public class keyRate : MonoBehaviour
{
    // 公開変数として回転速度を定義します。
    // Unityエディター上から値を変更できます。
    public float rotationSpeed = 50f;

    // Update関数は毎フレーム呼び出されます
    void Update()
    {
        // オブジェクト自身のY軸（垂直方向）を中心に回転させる
        // rotationSpeed * Time.deltaTime を掛けることで、
        // 異なるPC環境でも同じ速さで回転するようになります（フレームレート依存の解消）。

        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}

