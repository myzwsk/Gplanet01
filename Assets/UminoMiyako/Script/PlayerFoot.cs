using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private Player player; // 親のPlayerスクリプト参照

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlopeTop"))
        {
            Debug.Log("うえ！");
            player.ExitSlopeTop(other.transform.position.y);
        }
        else if (other.CompareTag("SlopeBottom"))
        {
            Debug.Log("した！");
            player.ExitSlopeBottom(other.transform.position.y);
        }
        else if (other.CompareTag("Slope")) // 梯子接触時
        {
            Debug.Log("はしご！");
            player.Slope();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slope")) // 梯子と離れたとき
        {
            Debug.Log("離れた！");
            player.ExitSlope();
        }
    }
}
