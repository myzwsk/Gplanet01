using UnityEngine;

public class BLOCKBar : MonoBehaviour
{
    public int damage=1;
    private BattleMana hp;
    void Start()
    {
        hp = FindAnyObjectByType<BattleMana>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hp != null)
            {
                hp.PDamage(damage);
            }
            Debug.Log("プレイヤー死亡");
        }
    }
}
