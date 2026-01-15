using UnityEngine;

public class Shooter: MonoBehaviour
{
    public float rotateSpeed = 90f;
    public GameObject bulletPrefab;
    public float fireRate = 3.0f; // 何秒に一回撃つか
    private float nextFireTime;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        // 範囲内にプレイヤーがいて、かつ発射間隔を満たしていたら
        if (Time.time >= nextFireTime)
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
}
