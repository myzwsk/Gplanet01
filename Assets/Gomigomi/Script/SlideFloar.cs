using UnityEngine;

public class SlideFloor : MonoBehaviour
{
    [Header("Movement Settings")]
    public float sX = 5.0f; // 動きがわかるよう初期値を設定
    public float sY = 0;
    public float sZ = 0;
    public float Speed = 1.0f;
    public bool Circular = false;

    private Vector3 initialPosition;
    private Vector3 lastPosition;
    private CharacterController playerCC;

    void Start()
    {
        initialPosition = transform.position;
        lastPosition = transform.position;
    }

    void FixedUpdate() // 移動処理はFixedUpdateが安定します
    {
        // 1. 時間経過
        float time = Time.time * Speed;

        // 2. 次のフレームの床の位置を計算
        Vector3 nextPosition;
        if (Circular)
        {
            float x = Mathf.Cos(time) * sX;
            float z = Mathf.Sin(time) * sZ;
            nextPosition = initialPosition + new Vector3(x, sY, z);
        }
        else
        {
            nextPosition = new Vector3(
                Mathf.Sin(time) * sX + initialPosition.x,
                Mathf.Sin(time) * sY + initialPosition.y,
                Mathf.Sin(time) * sZ + initialPosition.z
            );
        }

        // 3. 床を動かす
        transform.position = nextPosition;

        // 4. 【重要】「今回、床がどれだけ動いたか」を計算
        Vector3 platformMoveAmount = transform.position - lastPosition;

        // 5. プレイヤーが乗っていれば、床の移動量と同じだけ「押す」
        if (playerCC != null)
        {
            // Moveを使うことで、プレイヤー自身の移動入力と合算され、自然に動く
            playerCC.Move(platformMoveAmount);
        }

        // 次の計算のために位置を記憶
        lastPosition = transform.position;
    }

    // --- プレイヤー検知（GetComponentInParentを使用） ---

    private void OnTriggerEnter(Collider other)
    {
        // 足や武器に当たっても、親のCharacterControllerを探し出す
        var cc = other.GetComponentInParent<CharacterController>();
        if (cc != null)
        {
            playerCC = cc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var cc = other.GetComponentInParent<CharacterController>();
        if (cc != null && playerCC == cc)
        {
            playerCC = null;
        }
    }
}