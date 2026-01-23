using UnityEngine;

public class Shot: MonoBehaviour
{
    public int damage = 1;
    public float speed = 5f;
    public float lifeTime = 3f; // 3秒後に自動消滅

    private BattleMana hp;
    void Start()
    {
        Destroy(gameObject, lifeTime);
        hp = FindAnyObjectByType<BattleMana>();
    }

    void Update()
    {
        // 前方向に進み続ける
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hp != null)
            {
                hp.PDamage(damage);
            }
            Debug.Log("プレイヤー死亡");
            Destroy(gameObject);
        }
        
    }
}
