using UnityEngine;

public class SlideFloar : MonoBehaviour
{
    public float sX = 0;
    public float sY = 0;
    public float sZ = 0;
    public float Speed = 0;
    public bool Circular = false;

    private Vector3 lastPosition;
    private Vector3 initialPosition;

    private CharacterController playerCC;

    void Start()
    {
        initialPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        float time = Time.time * Speed;

        // --- 床の移動処理 ---
        if (Circular)
        {
            float x = Mathf.Cos(time) * sX;
            float z = Mathf.Sin(time) * sZ;
            transform.position = initialPosition + new Vector3(x, sY, z);
        }
        else
        {
            transform.position = new Vector3(
                Mathf.Sin(Time.time) * sX + initialPosition.x,
                Mathf.Sin(Time.time) * sY + initialPosition.y,
                Mathf.Sin(Time.time) * sZ + initialPosition.z
            );
        }

        // --- 移動量を計算 ---
        Vector3 moveAmount = transform.position - lastPosition;
        lastPosition = transform.position;

        // --- プレイヤー追従（CharacterController.Move） ---
        if (playerCC != null)
        {
            playerCC.Move(moveAmount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCC = other.GetComponent<CharacterController>();
            Debug.Log("playerが乗りました");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCC = null;
            Debug.Log("playerが降りました");
        }
    }
}
