using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public float rotateSpeed = 90f;
    public GameObject bulletPrefab;
    public float fireRate = 3.0f; // 何秒に一回撃つか
    private float nextFireTime;
    private bool playerInRange = false;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        // 範囲内にプレイヤーがいて、かつ発射間隔を満たしていたら
        if (playerInRange && Time.time >= nextFireTime)
        {
            ShootSixDirections();
            nextFireTime = Time.time + fireRate;
        }
        
    }
    void ShootSixDirections()
    {

        for (int i = 0; i < 6; i++)
        {
            // 1. 60度ずつの基本角度を計算
            float angle = i * 60f;

            // 2. 【重要】オブジェクト自身の現在の回転(transform.eulerAngles.y)を加算する
            // これにより、本体が回れば発射方向も一緒に回ります
            float finalAngle = angle + transform.eulerAngles.y;

            Quaternion rotation = Quaternion.Euler(0, finalAngle, 0);

            // 3. 計算した回転値で弾を生成
            Instantiate(bulletPrefab, transform.position, rotation);
        }
    }

    // プレイヤーが入った判定（Playerタグがついている前提）
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player")) playerInRange = true;
    }

    // プレイヤーが出た判定
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("player")) playerInRange = false;
    }
}
