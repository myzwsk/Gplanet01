using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 3f; // 3秒後に自動消滅
    

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 前方向に進み続ける
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        // 衝突してきたオブジェクトのタグが「Player」であるかを確認
        // collision引数には、衝突に関する詳細情報が含まれています。
        if (collision.gameObject.CompareTag("Player"))
        {
            // Playerとの衝突が確認された場合

            // このGameObject（オブジェクト自体）を破壊してシーンから消去
            Destroy(gameObject);
            
        }
    }
}
